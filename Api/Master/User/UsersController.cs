using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using pergisafar.Shared.Models;

namespace test_blazor.Server.Controllers
{
    [ApiController]
    [Route("User")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _IUserService;
        private readonly ErrorHandlingUtility _errorUtility;
        public UsersController(IUserService userService)
        {
            _IUserService = userService;
            _errorUtility = new ErrorHandlingUtility();
        }

        [Authorize]
        [HttpGet]
        public async Task<object> Get()
        {
            try
            {
                var data = await _IUserService.Get();
                return new { items = data, message = "Berhasil" };
            }
            catch (CustomException ex)
            {
                int errorCode = ex.ErrorCode;
                var errorResponse = new ErrorResponse(errorCode, ex.ErrorHeader, ex.Message);
                return _errorUtility.HandleError(errorCode, errorResponse);
            }
        }

        // [Authorize]
        [HttpGet("{id}")]
        public async Task<object> GetId([FromRoute] string id)
        {
            try
            {
                return await _IUserService.GetId(id);
            }
            catch (CustomException ex)
            {
                int errorCode = ex.ErrorCode;
                var errorResponse = new ErrorResponse(errorCode, ex.ErrorHeader, ex.Message);
                return _errorUtility.HandleError(errorCode, errorResponse);
            }
        }

        // [Authorize]
        [HttpPut("{id}")]
        public async Task<object> Put([FromRoute] string id, [FromBody] User item)
        {
            try
            {
                var data = await _IUserService.Put(id, item);
                return await _IUserService.Put(id, item);
            }
            catch (CustomException ex)
            {
                int errorCode = ex.ErrorCode;
                var errorResponse = new ErrorResponse(errorCode, ex.ErrorHeader, ex.Message);
                return _errorUtility.HandleError(errorCode, errorResponse);
            }
        }

        // [Authorize]
        [HttpDelete("{id}")]
        public async Task<object> Delete([FromRoute] string id)
        {
            try
            {
                var data = await _IUserService.Delete(id);
                return new { items = data, message = "Berhasil" };
            }
            catch (CustomException ex)
            {
                int errorCode = ex.ErrorCode;
                var errorResponse = new ErrorResponse(errorCode, ex.ErrorHeader, ex.Message);
                return _errorUtility.HandleError(errorCode, errorResponse);
            }
        }
    }
}