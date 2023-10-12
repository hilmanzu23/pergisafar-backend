using Microsoft.AspNetCore.Mvc;
using static RepositoryPattern.Services.TransactionsService.TransactionsService;

namespace test_blazor.Server.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class TypeTransactionsController: ControllerBase
    {
        private readonly ITransactionsService _transactionsService;

        public TypeTransactionsController(ITransactionsService transactionsService)
        {
            _transactionsService = transactionsService;
        }

        [HttpGet]
        public async Task<object> Get()
        {
            try
            {
                var data = await _transactionsService.Get();                
                return new {items=data, message = "Berhasil"};
            }
            catch (System.Exception data)
            {
                
                return new {error = data.Message};
            }
        }

        [HttpPost]
        public async Task<object> Post([FromBody]TypeForm item)
        {
            try
            {
                var data = await _transactionsService.Post(item);                
                return new {data};
            }
            catch (System.Exception data)
            {
                
                return new {error = data.Message};
            }
        }

        [HttpPut("{id}")]
        public async Task<object> Put([FromRoute]string id, [FromBody]TypeForm item)
        {
            try
            {
                var data = await _transactionsService.Put(id, item);                
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
                var data = await _transactionsService.Delete(id);                
                return new {items=data, message = "Berhasil"};
            }
            catch (System.Exception data)
            {
                
                return new {error = data.Message};
            }
        }

       
    }
}