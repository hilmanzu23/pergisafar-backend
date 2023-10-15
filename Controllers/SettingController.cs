using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using pergisafar.Shared.Models;
using static RepositoryPattern.Services.SettingService.SettingService;

namespace test_blazor.Server.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class SettingController : ControllerBase
    {
        private readonly ISettingService _ISettingService;
        public SettingController(ISettingService settingService)
        {
            _ISettingService = settingService;
        }

        [Authorize]
        [HttpGet]
        public async Task<object> Get()
        {
            try
            {
                var data = await _ISettingService.Get();
                return new { items = data, message = "Berhasil" };
            }
            catch (System.Exception data)
            {

                return new { error = data.Message };
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<object> Post([FromBody] SettingForm item)
        {
            try
            {
                var data = await _ISettingService.Post(item);
                return new { data };
            }
            catch (System.Exception data)
            {

                return new { error = data.Message };
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<object> Put([FromRoute] string id, [FromBody] Setting item)
        {
            try
            {
                var data = await _ISettingService.Put(id, item);
                return new { data };
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
                var data = await _ISettingService.Delete(id);
                return new { items = data, message = "Berhasil" };
            }
            catch (System.Exception data)
            {

                return new { error = data.Message };
            }
        }


    }
}