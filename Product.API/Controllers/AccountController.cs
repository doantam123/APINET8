using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Product.API.Errors;
using Product.API.Extensions;
using Product.Core.Dto;
using Product.Core.Entities;
using Product.Core.Interface;

namespace Product.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ITokenServices _tokenServices;
        public AccountController(UserManager<AppUser> userManager, ITokenServices tokenServices,SignInManager<AppUser> signInManager, IMapper mapper)
        {
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenServices = tokenServices;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                return Unauthorized(new BaseCommonResponse(401));
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if(result is null || result.Succeeded == false)
            {
                return Unauthorized(new BaseCommonResponse(401)); 
            }
            return Ok(new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = _tokenServices.CreateToken(user)
            });
        }

        [HttpGet("check-email-exist")]
        public async Task<ActionResult<bool>> CheckEmailExist([FromQuery] string email)
        {
            var result = await _userManager.FindByEmailAsync(email);
            if(result is not null)
            {
                return true;
            }
            return false;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if(CheckEmailExist(registerDto.Email).Result.Value)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse
                {
                    Errors = new[] {"This Email is already"}
                });
            }

            var user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                UserName = registerDto.Email,
                Email = registerDto.Email,
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if(result.Succeeded == false)
            {
                return BadRequest(new BaseCommonResponse(400));
            }
            return Ok(new UserDto
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                Token = _tokenServices.CreateToken(user)
            }) ;
        }

        [Authorize]
        [HttpGet("test")]
        public ActionResult <string> Test()
        {
            return "test";
        }

        [Authorize]
        [HttpGet("get-current-user")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await _userManager.FindEmailByClaimPrincipal(HttpContext.User);
            return Ok(new UserDto
            {
                DisplayName= user.DisplayName,
                Email = user.Email,
                Token = _tokenServices.CreateToken(user)
            });
        }

        [Authorize]
        [HttpGet("get-user-address")]
        public async Task<IActionResult> GetUserAddress()
        {
            var user = await _userManager.FindUserByClaimPrincipalWithAddress(HttpContext.User);
            var result = _mapper.Map<Address, AddressDto>(user.Address);
            return Ok(result);
        }

        [Authorize]
        [HttpPut("update-user-address")]
        public async Task<IActionResult> UpdateUserAddress(AddressDto addressDto)
        {
            var user = await _userManager.FindUserByClaimPrincipalWithAddress(HttpContext.User);
            user.Address = _mapper.Map<AddressDto, Address>(addressDto);
            var result = await _userManager.UpdateAsync(user);
            if(result.Succeeded)
            {
                return Ok(_mapper.Map<Address, AddressDto>(user.Address));
            }
            return BadRequest("Problems:" + HttpContext.User);
        }
    }
}
