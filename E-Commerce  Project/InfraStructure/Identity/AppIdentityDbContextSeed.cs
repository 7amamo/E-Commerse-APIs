using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraStructure.Identity
{
    public static class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager )
        {
            //if (! roleManager.Roles.Any())
            //{
            //    await roleManager.CreateAsync(new IdentityRole("Admin"));
            //    await roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
            //}

            if (!userManager.Users.Any())
            {
                var superAdminUser = new AppUser()
                {
                    DissplayName = "Ahmed Hamamo",
                    Email = "AhmedHamamo095@gmail.com",
                    UserName = "AhmedHamamo095",
                    PhoneNumber = "01028981385"
                };
                var adminUser = new AppUser()
                {
                    DissplayName = "Ahmed Tarek",
                    Email = "AhmedTarek@gmail.com",
                    UserName = "AhmedTarek",
                    PhoneNumber = "0123465789"
                };
                await userManager.CreateAsync(superAdminUser, "Passw0rd");
                await userManager.CreateAsync(adminUser, "Passw0rd");

                //await userManager.AddToRoleAsync(superAdminUser, "SuperAdmin");
                //await userManager.AddToRoleAsync(adminUser, "Admin");

            }
        }
    }
}
