
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
        [Route("auth/login")]
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
        [Route("auth/register")]
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

        [HttpGet]
        [Route("auth/aktifasi/{id}")]
        public async Task<object> VerifySeasonsAsync([FromRoute]string id)
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
        [Route("auth/verify")]
        public object Aktifasi()
        {
            try
            {
                var claims = User.Claims;
                if (claims == null)
                {
                    return Unauthorized();
                }
                var cek = CheckToken();
                return cek;
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        private object CheckToken()
        {
            string accessToken = HttpContext.Request.Headers["Authorization"];
            var checktoken = _ConvertJwt.ConvertString(accessToken);
            return checktoken;
        }
    }

}