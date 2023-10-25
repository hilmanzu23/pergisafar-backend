
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pergisafar.Shared.Models;
using static RepositoryPattern.Services.PaymentService.PaymentService;

namespace test_blazor.Server.Controllers
{
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _IPaymentService;
        private readonly ConvertJWT _ConvertJwt;
        public PaymentController(IPaymentService paymentService, ConvertJWT convert)
        {
            _IPaymentService = paymentService;
            _ConvertJwt = convert;
        }

        // [Authorize]
        [HttpGet("GetPayment/{id}")]
        public async Task<object> GetId([FromRoute] string id)
        {
            try
            {
                return await _IPaymentService.GetId(id);
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        // [Authorize]
        [HttpGet]
        [Route("PaymentMethod")]
        public async Task<IActionResult> PaymentMethod()
        {
            try
            {
                var dataList = await _IPaymentService.GetPayment();
                return Ok(dataList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        // [Authorize]
        [HttpPost]
        [Route("MakePayment")]
        public async Task<IActionResult> MakePayment([FromBody] CreatePaymentDto createPaymentDto)
        {
            try
            {
                var dataList = await _IPaymentService.MakePayment(createPaymentDto);
                return Ok(dataList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("ApprovalPayment")]
        public async Task<IActionResult> ApprovalPayment()
        {
            try
            {
                if (Request.HasFormContentType)
                {
                    // You can access the form data using the Form property of the Request object.
                    var merchantOrderId = Request.Form["merchantOrderId"].ToString();
                    // Call your service method with the merchantOrderId
                    var dataList = await _IPaymentService.ApprovalPayment(merchantOrderId);
                    return Ok(dataList);
                }
                else
                {
                    return BadRequest("No form data found in the request.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }

}