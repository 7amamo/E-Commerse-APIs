using AutoMapper;
using Core.Entities.Identity;
using Core.Services;


using E_Commerce_Project.DTOS;
using E_Commerce_Project.Errors;
using E_Commerce_Project.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using System.Security.Claims;

namespace E_Commerce_Project.Controllers
{

    public class AuthenticationController : ApiBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;

        public AuthenticationController(UserManager<AppUser> userManager 
            ,ITokenService tokenService,
            SignInManager<AppUser> signInManager,
            IMapper mapper
            )
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO Data)
        {
            if (EmailExists(Data.Email).Result.Value)
            {
                return BadRequest(new ApiResponse(400, "Email is Already in Use"));

            }
            var user = new AppUser()
            {
                DissplayName = Data.DisplayName,
                PhoneNumber = Data.PhoneNumber,
                Email = Data.Email,
                UserName = Data.Email.Split('@')[0],
            };
            var result = await _userManager.CreateAsync(user, Data.Password);
            if (! result.Succeeded) return BadRequest(new ApiResponse(400));
            
            return Ok(new UserDTO()
            {
                DisplayName = user.DissplayName,
                Email = user.Email,
                Token =await _tokenService.CreateTokenAsync(user,_userManager),
            });

        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO model)
        {
            var User = await _userManager.FindByEmailAsync(model.Email);
            if (User is null) return Unauthorized(new ApiResponse(401));

            var Result = await _signInManager.CheckPasswordSignInAsync(User, model.Password, false);
            if (!Result.Succeeded) return Unauthorized(new ApiResponse(401));


            return Ok(new UserDTO()
            {
                DisplayName = User.DissplayName,
                Email = User.Email,
                Token = await _tokenService.CreateTokenAsync(User, _userManager)
            });

        }

        [HttpGet("EmailExists")]
        public async Task<ActionResult<bool>> EmailExists(string Email)
        {
            return await _userManager.FindByEmailAsync(Email) is not null;
        }

        [HttpGet("GetCurrentUser")]
        [Authorize]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            var user =await _userManager.FindByEmailAsync(email);

            var ReturnedObject = new UserDTO()
            {
                DisplayName = user.DissplayName,
                Email = user.Email,
                Token = await _tokenService.CreateTokenAsync(user, _userManager)
            };
            return Ok(ReturnedObject);

        }

        [HttpGet("Address")]
        [Authorize]

        public async Task<ActionResult<AddressDTO>> GetCurrentUserAddress()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);

            var user = await _userManager.FindUserWithAddressAsync(Email);  //  contain the address

            var MappedAddress = _mapper.Map<Address, AddressDTO>(user.Address);

            return Ok(MappedAddress);
        }


        [HttpPut("Address")]
        [Authorize]

        public async Task<ActionResult<AddressDTO>> UpdateAddress(AddressDTO UpdatedAddress)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindUserWithAddressAsync(Email);  //  contain the address

            var MappedAddress = _mapper.Map<AddressDTO, Address>(UpdatedAddress);
            MappedAddress.Id = user.Address.Id;

            user.Address = MappedAddress;

            var Result = await _userManager.UpdateAsync(user);

            if (!Result.Succeeded) return BadRequest(new ApiResponse(400));

            return Ok(UpdatedAddress);
        }

    }
}
