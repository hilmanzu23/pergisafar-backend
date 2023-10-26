
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

        private readonly ValidationAuthDto _masterValidationService;
        public AuthController(IAuthService authService, ConvertJWT convert, ValidationAuthDto MasterValidationService)
        {
            _IAuthService = authService;
            _ConvertJwt = convert;
            _errorUtility = new ErrorHandlingUtility();
            _masterValidationService = MasterValidationService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Auth/Login")]
        public async Task<object> LoginAsync([FromBody] LoginDto login)
        {
            try
            {
                var validationErrors = _masterValidationService.ValidateLogin(login);
                if (validationErrors.Count > 0)
                {
                    var errorResponse = new { code = 400, errorMessage = validationErrors };
                    return BadRequest(errorResponse);
                }
                if (!IsValidEmail(login.Email))
                {
                    throw new CustomException(400, "Email", "Format Email Salah");
                }
                if (login.Password.Length < 8)
                {
                    throw new CustomException(400, "Password", "Password Harus Lebih 8 Karakter");
                }
                var response = await _IAuthService.LoginAsync(login);
                return Ok(response);
            }
            catch (CustomException ex)
            {
                int errorCode = ex.ErrorCode;
                var errorResponse = new ErrorResponse(errorCode, ex.Message, ex.ErrorHeader);
                return _errorUtility.HandleError(errorCode, errorResponse);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Auth/Register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto login)
        {
            try
            {
                if (!IsValidEmail(login.Email))
                {
                    throw new CustomException(400, "Email", "Format Email salah.");
                }
                if (login.Password.Length < 8)
                {
                    throw new CustomException(400, "Password", "Password harus 8 karakter");
                }
                var dataList = await _IAuthService.RegisterAsync(login);
                return Ok(dataList);
            }
            catch (CustomException ex)
            {
                int errorCode = ex.ErrorCode;
                var errorResponse = new ErrorResponse(errorCode, ex.Message, ex.ErrorHeader);
                return _errorUtility.HandleError(errorCode, errorResponse);
            }
        }

        [Authorize]
        [HttpPost]
        [Route("Auth/UpdatePassword")]
        public async Task<object> UpdatePassword([FromBody] UpdatePasswordDto item)
        {
            try
            {
                string accessToken = HttpContext.Request.Headers["Authorization"];
                string idUser = await _ConvertJwt.ConvertString(accessToken);
                if (item.Password != item.ConfirmPassword)
                {
                    throw new CustomException(400, "Password", "Password tidak sama");
                }
                var dataList = await _IAuthService.UpdatePassword(idUser, item);
                return Ok(dataList);
            }
            catch (CustomException ex)
            {
                int errorCode = ex.ErrorCode;
                var errorResponse = new ErrorResponse(errorCode, ex.Message, ex.ErrorHeader);
                return _errorUtility.HandleError(errorCode, errorResponse);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Auth/VerifyOtp/{id}")]
        public async Task<object> VerifyOtp([FromRoute] string id, [FromBody] OtpDto otp)
        {
            try
            {
                var dataList = await _IAuthService.VerifyOtp(id, otp);
                return Ok(dataList);
            }
            catch (CustomException ex)
            {
                int errorCode = ex.ErrorCode;
                var errorResponse = new ErrorResponse(errorCode, ex.Message, ex.ErrorHeader);
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
                var errorResponse = new ErrorResponse(errorCode, ex.Message, ex.ErrorHeader);
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
                var errorResponse = new ErrorResponse(errorCode, ex.Message, ex.ErrorHeader);
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
                    return new CustomException(400, "Error", "Unauthorized");
                }
                return new { code = 200, message = "Masih Berlaku" };
            }
            catch (CustomException ex)
            {
                int errorCode = ex.ErrorCode;
                var errorResponse = new ErrorResponse(errorCode, ex.Message, ex.ErrorHeader);
                return _errorUtility.HandleError(errorCode, errorResponse);
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }

}