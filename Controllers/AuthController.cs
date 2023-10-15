
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
        private readonly ErrorHandlingUtility _errorUtility;
        public AuthController(IAuthService authService, ConvertJWT convert)
        {
            _IAuthService = authService;
            _ConvertJwt = convert;
            _errorUtility = new ErrorHandlingUtility();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Auth/Login")]
        public async Task<object> LoginAsync([FromBody] UserForm login)
        {
            try
            {
                var response = await _IAuthService.LoginAsync(login);
                return Ok(response);
            }
            catch (CustomException ex)
            {
                int errorCode = ex.ErrorCode;
                var errorResponse = new ErrorResponse(errorCode, ex.Message);
                return _errorUtility.HandleError(errorCode, errorResponse);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Auth/Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] UserRegisterCustomer login)
        {
            try
            {
                if (string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.FullName) || string.IsNullOrEmpty(login.PhoneNumber) || string.IsNullOrEmpty(login.Password))
                {
                    var errorResponse = new ErrorResponse(400, "Data tidak boleh kosong");
                    return _errorUtility.HandleError(400, errorResponse);
                }
                var dataList = await _IAuthService.RegisterAsync(login);
                return Ok(dataList);
            }
            catch (CustomException ex)
            {
                int errorCode = ex.ErrorCode;
                var errorResponse = new ErrorResponse(errorCode, ex.Message);
                return _errorUtility.HandleError(errorCode, errorResponse);
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
                    throw new CustomException(400, "Password tidak sama");
                }
                var dataList = await _IAuthService.UpdatePassword(idUser, item);
                return Ok(dataList);
            }
            catch (CustomException ex)
            {
                int errorCode = ex.ErrorCode;
                var errorResponse = new ErrorResponse(errorCode, ex.Message);
                return _errorUtility.HandleError(errorCode, errorResponse);
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
            catch (CustomException ex)
            {
                int errorCode = ex.ErrorCode;
                var errorResponse = new ErrorResponse(errorCode, ex.Message);
                return _errorUtility.HandleError(errorCode, errorResponse);
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
            catch (CustomException ex)
            {
                int errorCode = ex.ErrorCode;
                var errorResponse = new ErrorResponse(errorCode, ex.Message);
                return _errorUtility.HandleError(errorCode, errorResponse);
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
            catch (CustomException ex)
            {
                int errorCode = ex.ErrorCode;
                var errorResponse = new ErrorResponse(errorCode, ex.Message);
                return _errorUtility.HandleError(errorCode, errorResponse);
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
            catch (CustomException ex)
            {
                int errorCode = ex.ErrorCode;
                var errorResponse = new ErrorResponse(errorCode, ex.Message);
                return _errorUtility.HandleError(errorCode, errorResponse);
            }
        }
    }

}