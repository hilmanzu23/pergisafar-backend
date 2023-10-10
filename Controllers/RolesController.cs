using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using pergisafar.Shared.Models;
using static RepositoryPattern.Services.RoleService.RoleService;

namespace test_blazor.Server.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class RoleController: ControllerBase
    {
        private readonly IRoleService _IRoleService;
        public RoleController(IRoleService roleService)
        {
            _IRoleService = roleService;
        }

        [HttpGet]
        public async Task<object> Get()
        {
            try
            {
                var data = await _IRoleService.Get();                
                return new {items=data, message = "Berhasil"};
            }
            catch (System.Exception data)
            {
                
                return new {error = data.Message};
            }
        }

        [HttpPost]
        public async Task<object> Post([FromBody]RoleForm item)
        {
            try
            {
                var data = await _IRoleService.Post(item);                
                return new {data};
            }
            catch (System.Exception data)
            {
                
                return new {error = data.Message};
            }
        }

        [HttpPut("{id}")]
        public async Task<object> Put([FromRoute]string id, [FromBody]Role item)
        {
            try
            {
                var data = await _IRoleService.Put(id, item);                
                return new {data};
            }
            catch (System.Exception data)
            {
                
                return new {error = data.Message};
            }
        }

        [HttpDelete("{id}")]
        public async Task<object> Delete([FromRoute]string id)
        {
            try
            {
                var data = await _IRoleService.Delete(id);                
                return new {items=data, message = "Berhasil"};
            }
            catch (System.Exception data)
            {
                
                return new {error = data.Message};
            }
        }

       
    }
}