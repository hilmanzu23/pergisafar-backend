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
        public UsersController(IUserService userService)
        {
            _IUserService = userService;
        }

        [HttpGet]
        public async Task<object> Get()
        {
            try
            {
                var data = await _IUserService.Get();
                return new { items = data, message = "Berhasil" };
            }
            catch (System.Exception data)
            {

                return new { error = data.Message };
            }
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<object> GetId([FromRoute] string id)
        {
            try
            {
                return await _IUserService.GetId(id);
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<object> Put([FromRoute] string id, [FromBody] User item)
        {
            try
            {
                var data = await _IUserService.Put(id, item);
                return await _IUserService.Put(id, item);
            }
            catch (System.Exception data)
            {

                return new { error = data.Message };
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<object> Delete([FromRoute] string id)
        {
            try
            {
                var data = await _IUserService.Delete(id);
                return new { items = data, message = "Berhasil" };
            }
            catch (System.Exception data)
            {

                return new { error = data.Message };
            }
        }
    }
}