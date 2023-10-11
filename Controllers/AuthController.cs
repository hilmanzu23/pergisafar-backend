
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pergisafar.Shared.Models;
using static RepositoryPattern.Services.AuthService.AuthService;

namespace test_blazor.Server.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _IAuthService;
        private readonly ConvertJWT _ConvertJwt;
        public AuthController(IAuthService authService, ConvertJWT convert)
        {
            _IAuthService = authService;
            _ConvertJwt = convert;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Auth/Login")]
        public async Task<IActionResult> LoginAsync([FromBody] UserForm login)
        {
            try
            {
                var dataList = await _IAuthService.LoginAsync(login);
                return Ok(dataList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Auth/Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] UserRegisterCustomer login)
        {
            try
            {
                var claims = User.Claims;
                var dataList = await _IAuthService.RegisterAsync(login);
                return Ok(dataList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("Auth/UpdatePassword")]
        public async Task<object> UpdatePassword([FromBody] UpdatePasswordForm item)
        {
            try
            {
                string accessToken = HttpContext.Request.Headers["Authorization"];
                string idUser = await _ConvertJwt.ConvertString(accessToken);
                if (item.Password != item.ConfirmPassword)
                {
                    return new { success = false, errorMessage = "Password tidak sama" };
                }
                var dataList = await _IAuthService.UpdatePassword(idUser, item);
                return Ok(dataList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Auth/VerifyOtp/{id}")]
        public async Task<object> VerifyOtp([FromRoute] string id, [FromBody] OtpForm otp)
        {
            try
            {
                var dataList = await _IAuthService.VerifyOtp(id, otp);
                return Ok(dataList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Auth/RequestOtpEmail/{email}")]
        public async Task<object> RequestOtp([FromRoute] string email)
        {
            try
            {
                var dataList = await _IAuthService.RequestOtpEmail(email);
                return Ok(dataList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("Auth/Aktifasi/{id}")]
        public async Task<object> VerifySeasonsAsync([FromRoute] string id)
        {
            try
            {
                var dataList = await _IAuthService.Aktifasi(id);
                return Ok(dataList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [Authorize]
        [HttpGet]
        [Route("Auth/VerifySessions")]
        public object Aktifasi()
        {
            try
            {
                var claims = User.Claims;
                if (claims == null)
                {
                    return Unauthorized();
                }
                return new { success = true, message = "Masih Berlaku" };
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }

}