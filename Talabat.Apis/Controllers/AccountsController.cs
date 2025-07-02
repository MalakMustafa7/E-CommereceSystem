using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.Apis.DTOs;
using Talabat.Apis.Errors;
using Talabat.Apis.Extentions;
using Talabat.Core.Models.Identity;
using Talabat.Core.Services;

namespace Talabat.Apis.Controllers
{
    
    public class AccountsController : ApiBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountsController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,
                                  ITokenService tokenService,IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }
        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO model)
        {
            if(CheckEmailExists(model.Email).Result.Value) 
                return BadRequest(new ApiResponse(400,"This Email Is Already Exist"));
            var User = new AppUser()
            {
                DisplayName= model.DisplayName,
                Email= model.Email,
                UserName = model.Email.Split('@')[0],
                PhoneNumber = model.PhoneNumber
            };
           var Result= await _userManager.CreateAsync(User, model.Password);
            if (!Result.Succeeded) return BadRequest(new ApiResponse(400));
            var ReturnedUser = new UserDTO()
            {
                DisplayName = User.DisplayName,
                Email = User.Email,
                Token =await _tokenService.GetTokenAsync(User, _userManager)
            };
            return Ok(ReturnedUser);

        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO model)
        {
            var User = await _userManager.FindByEmailAsync(model.Email);
            if (User is null) return Unauthorized(new ApiResponse(401));
            var Result=  await _signInManager.CheckPasswordSignInAsync(User, model.Password,false);
            if(!Result.Succeeded) return Unauthorized(new ApiResponse(401));
            var ReturnedUser = new UserDTO()
            {
                DisplayName = User.DisplayName,
                Email = User.Email,
                Token = await _tokenService.GetTokenAsync(User, _userManager)
            };
            return Ok(ReturnedUser);
        }

        [Authorize]
        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(Email);
            var ReturnedUser = new UserDTO()
            {
                DisplayName=user.DisplayName,
                Email = user.Email,
                Token = await _tokenService.GetTokenAsync(user, _userManager)
            };
            return Ok(ReturnedUser);
        }
        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDTO>> GetCurrentUserAddress()
        {
            var user = await _userManager.FindByAddressAsync(User);
            var MappedAddress = _mapper.Map<Address, AddressDTO>(user.Address);
            return Ok(MappedAddress);
        }
        [Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDTO>> UpdateAddress(AddressDTO updatedaddress)
        {
            var user = await _userManager.FindByAddressAsync(User);
            if (user is null) return Unauthorized(new ApiResponse(401));
            var mappedaddress = _mapper.Map<AddressDTO,Address>(updatedaddress);
            mappedaddress.Id = user.Address.Id;
            user.Address = mappedaddress;
            var Result =await _userManager.UpdateAsync(user);
            if (!Result.Succeeded) return BadRequest(new ApiResponse(400));
            return Ok(updatedaddress);
        }

        [HttpGet("EmailExists")]
        public async Task<ActionResult<bool>> CheckEmailExists(string email)
        {
            return await _userManager.FindByEmailAsync(email) is not null;
        }
    }
}
