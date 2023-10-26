using Microsoft.AspNetCore.Mvc;
using RepositoryPattern.Services.TransactionService;

namespace test_blazor.Server.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _ITransactionService;
        private readonly ErrorHandlingUtility _errorUtility;
        private readonly ValidationMasterDto _masterValidationService;
        public TransactionController(ITransactionService TransactionService)
        {
            _ITransactionService = TransactionService;
            _errorUtility = new ErrorHandlingUtility();
            _masterValidationService = new ValidationMasterDto();
        }

        // [Authorize]
        [HttpGet]
        public async Task<object> Get()
        {
            try
            {
                var data = await _ITransactionService.Get();
                return Ok(data);
            }
            catch (CustomException ex)
            {
                int errorCode = ex.ErrorCode;
                var errorResponse = new ErrorResponse(errorCode, ex.Message, ex.ErrorHeader);
                return _errorUtility.HandleError(errorCode, errorResponse);
            }
        }

        [HttpGet("ByUser/{id}/{idStatus}")]
        public async Task<object> GetUserId([FromRoute] string id, string idStatus)
        {
            try
            {
                var data = await _ITransactionService.GetId(id, idStatus);
                return Ok(data);
            }
            catch (CustomException ex)
            {
                int errorCode = ex.ErrorCode;
                var errorResponse = new ErrorResponse(errorCode, ex.Message, ex.ErrorHeader);
                return _errorUtility.HandleError(errorCode, errorResponse);
            }
        }

        // [Authorize]
        [HttpPost]
        public async Task<object> Post([FromBody] TransactionDto item)
        {
            try
            {
                var data = await _ITransactionService.Post(item);
                return Ok(data);
            }
            catch (CustomException ex)
            {
                int errorCode = ex.ErrorCode;
                var errorResponse = new ErrorResponse(errorCode, ex.Message, ex.ErrorHeader);
                return _errorUtility.HandleError(errorCode, errorResponse);
            }
        }

        // [Authorize]
        [HttpPut("{id}")]
        public async Task<object> Put([FromRoute] string id, [FromBody] TransactionDto item)
        {
            try
            {
                var data = await _ITransactionService.Put(id, item);
                return Ok(data);
            }
            catch (CustomException ex)
            {
                int errorCode = ex.ErrorCode;
                var errorResponse = new ErrorResponse(errorCode, ex.Message, ex.ErrorHeader);
                return _errorUtility.HandleError(errorCode, errorResponse);
            }
        }

        // [Authorize]
        [HttpDelete("{id}")]
        public async Task<object> Delete([FromRoute] string id)
        {
            try
            {
                var data = await _ITransactionService.Delete(id);
                return Ok(data);
            }
            catch (CustomException ex)
            {
                int errorCode = ex.ErrorCode;
                var errorResponse = new ErrorResponse(errorCode, ex.Message, ex.ErrorHeader);
                return _errorUtility.HandleError(errorCode, errorResponse);
            }
        }


    }
}