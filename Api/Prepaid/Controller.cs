using Microsoft.AspNetCore.Mvc;
using RepositoryPattern.Services.PricePrepaidService;

namespace test_blazor.Server.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class PricePrepaidController : ControllerBase
    {
        private readonly IPricePrepaidService _IPricePrepaidService;
        private readonly ErrorHandlingUtility _errorUtility;
        private readonly ValidationMasterDto _masterValidationService;
        public PricePrepaidController(IPricePrepaidService PricePrepaidService)
        {
            _IPricePrepaidService = PricePrepaidService;
            _errorUtility = new ErrorHandlingUtility();
            _masterValidationService = new ValidationMasterDto();
        }

        // [Authorize]
        [HttpGet]
        [Route("RefreshPrepaidPrice")]
        public async Task<object> RefreshData()
        {
            try
            {
                var data = await _IPricePrepaidService.RefreshData();
                return Ok(data);
            }
            catch (CustomException ex)
            {
                int errorCode = ex.ErrorCode;
                var errorResponse = new ErrorResponse(errorCode, ex.Message);
                return _errorUtility.HandleError(errorCode, errorResponse);
            }
        }

        [HttpGet]
        [Route("Get/{search}/{provider}")]
        public async Task<object> Get([FromRoute]string search, string provider)
        {
            try
            {
                var data = await _IPricePrepaidService.Get(search, provider);
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