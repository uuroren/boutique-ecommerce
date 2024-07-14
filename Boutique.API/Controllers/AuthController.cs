using Boutique.API.Models;
using Boutique.Application.Dtos;
using Boutique.Application.Interfaces;
using Boutique.Application.Services;
using Boutique.Application.Services.CartServices;
using Boutique.Application.Services.RefreshTokenServices;
using Boutique.Domain.Common;
using Boutique.Domain.Entities;
using Boutique.Infrastructure.ExternalServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Twilio.Types;

namespace Boutique.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController:ControllerBase {
        private readonly JwtService _jwtService;
        private readonly SmsService _smsService;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly VerificationService _verificationService;
        private readonly RefreshTokenService _refreshTokenService;
        private readonly ICartService _cartService;
        public AuthController(
            JwtService jwtService,
            SmsService smsService,
            SignInManager<User> signInManager,
            UserManager<User> userManager,VerificationService verificationService,RefreshTokenService refreshTokenService,ICartService cartService) {
            _jwtService = jwtService;
            _smsService = smsService;
            _signInManager = signInManager;
            _userManager = userManager;
            _verificationService = verificationService;
            _refreshTokenService = refreshTokenService;
            _cartService = cartService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto) {
            var user = await _userManager.FindByNameAsync(loginDto.PhoneNumber);
            if(user == null) {
                return Unauthorized(new BaseResponse() { Message = "No registered users found",Success = false,});
            }

            if(user.Blocked) {
                return NotFound(new BaseResponse { Message = "The user does not have permission to log into the system.",Success = false });
            }

            if(!user.PhoneNumberConfirmed) {
                return BadRequest(new BaseResponse { Message = "Please verify your phone number",Success = false });
            }

            // Kullanıcının doğrulama kodu göndermesini iste
            if(loginDto.PhoneNumber.StartsWith("0")) {
                loginDto.PhoneNumber = loginDto.PhoneNumber.Substring(1);
            }

            loginDto.PhoneNumber = $"+90{loginDto.PhoneNumber}";

            var existingRefreshToken = await _refreshTokenService.GetRefreshTokenAsync(user.Id.ToString());
            if(!string.IsNullOrEmpty(existingRefreshToken)) {
                var accessToken = _jwtService.GenerateToken(user);
                var newRefreshToken = _jwtService.GenerateRefreshToken();
                await _refreshTokenService.StoreRefreshTokenAsync(user.Id.ToString(),newRefreshToken);

                return Ok(new {
                    AccessToken = accessToken,
                    RefreshToken = newRefreshToken,
                    User = user
                });
            }

            var code = new Random().Next(1000,9999).ToString();
            _smsService.SendSms(loginDto.PhoneNumber,$"Your verification code: {code}");

            // Doğrulama kodunu veritabanında veya önbellekte sakla
            await _verificationService.StoreVerificationCodeAsync(loginDto.PhoneNumber,code);

            return Ok(new BaseResponse { Message = "Verification code sent." });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto userDto) {
            var user = new User {
                UserName = userDto.PhoneNumber,
                PhoneNumber = userDto.PhoneNumber,
                Email = userDto.Email,
                Age = userDto.Age,
                Blocked = false,
                Gender = userDto.Gender,
                Name = userDto.Name,
                Surname = userDto.Surname,
                PhoneNumberConfirmed = false,
                Role = Enum.Parse<RoleType>(userDto.Role),
                IdentityNumber = userDto.IdentityNumber,
            };

            var result = await _userManager.CreateAsync(user);
            if(!result.Succeeded) {
                return BadRequest(result.Errors);
            }

            if(userDto.PhoneNumber.StartsWith("0")) {
                userDto.PhoneNumber = userDto.PhoneNumber.Substring(1);
            }

            userDto.PhoneNumber = $"+90{userDto.PhoneNumber}";

            var code = new Random().Next(1000,9999).ToString();
            _smsService.SendSms(userDto.PhoneNumber,$"Your verification code: {code}");

            // Doğrulama kodunu veritabanında veya önbellekte sakla
            await _verificationService.StoreVerificationCodeAsync(userDto.PhoneNumber,code);

            return Ok(new BaseResponse { Message = "Registration successful! Please enter the verification code.",Result = user,Success = true });
        }

        [HttpPost("verify")]
        public async Task<IActionResult> Verify([FromBody] VerifyDto verifyDto) {
            if(verifyDto.PhoneNumber.StartsWith("0")) {
                verifyDto.PhoneNumber = verifyDto.PhoneNumber.Substring(1);
            }

            var redisKey = $"+90{verifyDto.PhoneNumber}";

            var isCodeExpired = await _verificationService.IsCodeExpiredAsync(redisKey);
            if(isCodeExpired) {
                return BadRequest(new BaseResponse { Message = "The verification code has expired, please log in again." });
            }

            var isValidCode = await _verificationService.ValidateVerificationCodeAsync(redisKey,verifyDto.Code);

            if(isValidCode) {
                var user = await _userManager.FindByNameAsync(verifyDto.PhoneNumber);
                user.PhoneNumberConfirmed = true;

                var token = _jwtService.GenerateToken(user);
                var refreshToken = _jwtService.GenerateRefreshToken();

                await _refreshTokenService.StoreRefreshTokenAsync(user.Id.ToString(),refreshToken);

                await _userManager.UpdateAsync(user);

                await _cartService.CreateCartAsync(new Application.Dtos.CartDtos.CreateCartDto() { UserId = user.Id.ToString() });

                return Ok(new { Token = token,RefreshToken = refreshToken,User = user });
            }

            return Unauthorized(new BaseResponse { Message = "Invalid verification code." });
        }
    }
}
