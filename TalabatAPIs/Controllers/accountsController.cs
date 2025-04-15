using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Talabat.Core.Entites.Identity;
using Talabat.Core.Services;
using TalabatAPIs.DTOS;
using TalabatAPIs.Errors;
using TalabatAPIs.Extensions;

namespace TalabatAPIs.Controllers
{
    public class accountsController : ApiBaseController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenServieces _token;
        private readonly IMapper _mapper;
        public accountsController(UserManager<AppUser> userManager
            , SignInManager<AppUser> signInManager, ITokenServieces token , IMapper mapper)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this._token = token;
            this._mapper = mapper;
        }
        // Register 
        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> Register(RegisterDTO Model)
        {
            if (CheckEmailExist(Model.Email).Result.Value)
            {
                return BadRequest(new ApiResponse(400 , "Email is Already in use "));
            } 
                                    
            var User = new AppUser()
            {
                DisplayName = Model.DisplayName,
                Email = Model.Email,
                UserName = Model.Email.Split('@')[0],
                PhoneNumber = Model.PhoneNumber,
            };
            var Result = await _userManager.CreateAsync(User, Model.Password);
            if (!Result.Succeeded) return BadRequest(new ApiResponse(400));

            var ReturnedUser = new UserDTO()
            {
                DisplayName = Model.DisplayName,
                Email = Model.Email,
                Token = await _token.CreateTokenAsync(User, _userManager)
            };
            return Ok(ReturnedUser);
        }


        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO Model)
        {
            var User = await _userManager.FindByEmailAsync(Model.Email);
            if (User is null) return Unauthorized(new ApiResponse(401));

            var Result = await _signInManager.CheckPasswordSignInAsync(User, Model.Password, false);
            if (!Result.Succeeded) return Unauthorized(new ApiResponse(401));

            return Ok(new UserDTO()
            {
                DisplayName = User.DisplayName,
                Email = User.Email,
                Token = await _token.CreateTokenAsync(User, _userManager)
            });

        }
        [Authorize]
        [HttpGet("GetCurrentUser")]
        public async Task<ActionResult<UserDTO>> GetCurrentUser()
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(Email);
            var ReturnedObject = new UserDTO()
            {
                Email = Email,
                DisplayName = user.DisplayName,
                Token = await _token.CreateTokenAsync(user, _userManager)
            };
            return Ok(ReturnedObject);
        }
        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDto>> GetCurrentUserAddress()
        {
            //var Email = User.FindFirstValue(ClaimTypes.Email);
            //var user = await _userManager.FindByEmailAsync(Email);

            var user = await _userManager.FindUserWithAddressAsync(User);
            var MappedAddress = _mapper.Map<Address,AddressDto>(user.Address);
            return Ok(MappedAddress);

        }
        [Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDto>> UpdateAddress(AddressDto UpdatedAddress)
        {
            var user = await _userManager.FindUserWithAddressAsync(User);
            var MappedAddress = _mapper.Map<AddressDto, Address>(UpdatedAddress);
            MappedAddress.Id = user.Address.Id;
            user.Address = MappedAddress;
            var Result = await _userManager.UpdateAsync(user);
            if (!Result.Succeeded)return BadRequest(new ApiResponse(400));
            return Ok(UpdatedAddress);
        }
        [HttpGet("EmailExist")]
        public async Task<ActionResult<bool>> CheckEmailExist (string Email)
        {
            //var user = await _userManager.FindByEmailAsync(Email);
            return await _userManager.FindByEmailAsync(Email) is not null ;
        }




    }
}
