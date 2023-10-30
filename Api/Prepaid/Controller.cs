using System.Text.RegularExpressions;
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
                var errorResponse = new ErrorResponse(errorCode, ex.ErrorHeader, ex.Message);
                return _errorUtility.HandleError(errorCode, errorResponse);
            }
        }

        [HttpGet]
        [Route("GetPulsa/{phone}")]
        public async Task<object> GetPulsa([FromRoute]string phone)
        {
            try
            {
                GlobalValidator.PhoneValidator(phone);
                var data = await _IPricePrepaidService.GetPulsa(phone);
                return Ok(data);
            }
            catch (CustomException ex)
            {
                int errorCode = ex.ErrorCode;
                var errorResponse = new ErrorResponse(errorCode, ex.ErrorHeader, ex.Message);
                return _errorUtility.HandleError(errorCode, errorResponse);
            }
        }

        [HttpGet]
        [Route("GetData/{phone}")]
        public async Task<object> GetData([FromRoute]string phone)
        {
            try
            {
                GlobalValidator.PhoneValidator(phone);
                var data = await _IPricePrepaidService.GetData( phone);
                return Ok(data);
            }
            catch (CustomException ex)
            {
                int errorCode = ex.ErrorCode;
                var errorResponse = new ErrorResponse(errorCode, ex.ErrorHeader, ex.Message);
                return _errorUtility.HandleError(errorCode, errorResponse);
            }
        }

        [HttpGet]
        [Route("GetTokenPln/{notoken}")]
        public async Task<object> GetPln([FromRoute]string notoken)
        {
            try
            {
                GlobalValidator.NumerikValidator(notoken);
                var data = await _IPricePrepaidService.GetPln(notoken);
                return Ok(data);
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