
using Core.Contracts;
using Core.Entities.Identity;
using Core.Services;
using E_Commerce_Project.Errors;
using E_Commerce_Project.Middlewares;
using InfraStructure.Data;
using InfraStructure.Identity;
using InfraStructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Services;
using StackExchange.Redis;
using System.Text;
using Talabat.API.Helpers;


namespace E_Commerce_Project
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
                
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<StoreContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
            });

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    // modelState  => Dic [keyValuePair] 
                    // key => Name of Param
                    // value => Error

                    var errors = actionContext.ModelState.Where(p => p.Value.Errors.Count() > 0)
                    .SelectMany(p => p.Value.Errors)
                    .Select(E => E.ErrorMessage).ToArray();

                    var ApiValidationErrorResponse = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(ApiValidationErrorResponse);
                };

            });

            builder.Services.AddAutoMapper(typeof(MappingProfiles));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
            {
                var Connection = builder.Configuration.GetConnectionString("RedisConnection");

                return ConnectionMultiplexer.Connect(Connection);
            });
            builder.Services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));

            #region Identity


            builder.Services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("IdentitySQLConnection"));
            });

            builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppIdentityDbContext>();

            builder.Services.AddAuthentication(options =>          // userManager // RoleManager // signInManager
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; 
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;        
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["JWT:ValidAudience"],
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF32.GetBytes(builder.Configuration["JWT:Key"]))
                    };
                });

            builder.Services.AddScoped<ITokenService , TokenService>();
            #endregion

            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();

            builder.Services.AddSingleton<IResponseCacheService, ResponseCacheService>();







            var app = builder.Build();







            using var Scope = app.Services.CreateScope(); // Group Of services LifeTime Scoped
            var Services = Scope.ServiceProvider; // Services Itself 
            var LoggerFactory = Services.GetRequiredService<ILoggerFactory>();
            try
            {
                var dbContext = Services.GetRequiredService<StoreContext>(); // ask CLR for Creating object from StoreContext Explicitly
                await dbContext.Database.MigrateAsync(); // Update Database

                var dbContextIdentity = Services.GetRequiredService<AppIdentityDbContext>(); // ask CLR for Creating object from AppIdentityDbContext Explicitly
                await dbContextIdentity.Database.MigrateAsync(); // Update Database

                var UserManager = Services.GetRequiredService<UserManager<AppUser>>();

                //var RoleManager = Services.GetRequiredService<RoleManager<IdentityRole>>();

                await AppIdentityDbContextSeed.SeedUserAsync(UserManager );
                await StoreContextSeed.SeedAsync(dbContext); // To Seed StoreContext
            }
            catch (Exception ex)
            {
                var Logger = LoggerFactory.CreateLogger<Program>();
                Logger.LogError(ex, " An Error Ocuured During Applying The Migrations");
            }


            // Configure the HTTP request pipeline.
            app.UseMiddleware<ExceptionMiddleWare>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
