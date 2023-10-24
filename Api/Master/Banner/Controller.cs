using Microsoft.AspNetCore.Mvc;

namespace test_blazor.Server.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class BannerController : ControllerBase
    {
        private readonly IBannerService _IBannerService;
        private readonly ErrorHandlingUtility _errorUtility;
        private readonly ValidationMasterDto _masterValidationService;
        public BannerController(IBannerService BannerService)
        {
            _IBannerService = BannerService;
            _errorUtility = new ErrorHandlingUtility();
            _masterValidationService = new ValidationMasterDto();
        }

        // [Authorize]
        [HttpGet]
        public async Task<object> Get()
        {
            try
            {
                var data = await _IBannerService.Get();
                return Ok(data);
            }
            catch (CustomException ex)
            {
                int errorCode = ex.ErrorCode;
                var errorResponse = new ErrorResponse(errorCode, ex.Message);
                return _errorUtility.HandleError(errorCode, errorResponse);
            }
        }

        // [Authorize]
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<object> Post([FromForm] ImageUploadViewModel model)
        {
            try
            {  
                var data = await _IBannerService.Post(model);
                return Ok(data);    
            }
            catch (CustomException ex)
            {
                int errorCode = ex.ErrorCode;
                var errorResponse = new ErrorResponse(errorCode, ex.Message);
                return _errorUtility.HandleError(errorCode, errorResponse);
            }
        }

        // [Authorize]
        [HttpPut("{id}")]
        public async Task<object> Put([FromRoute] string id, [FromBody] ImageUploadViewModel item)
        {
            try
            {
                var validationErrors = _masterValidationService.ValidateBannerInput(item);
                if (validationErrors.Count > 0)
                {
                    var errorResponse = new { code = 400, errorMessage = validationErrors };
                    return BadRequest(errorResponse);
                }
                var data = await _IBannerService.Put(id, item);
                return Ok(data);
            }
            catch (CustomException ex)
            {
                int errorCode = ex.ErrorCode;
                var errorResponse = new ErrorResponse(errorCode, ex.Message);
                return _errorUtility.HandleError(errorCode, errorResponse);
            }
        }

        // [Authorize]
        [HttpDelete("{id}")]
        public async Task<object> Delete([FromRoute] string id)
        {
            try
            {
                var data = await _IBannerService.Delete(id);
                return Ok(data);
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