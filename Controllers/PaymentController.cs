
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pergisafar.Shared.Models;
using static RepositoryPattern.Services.AuthService.AuthService;
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

        [AllowAnonymous]
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

        [AllowAnonymous]
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

    }

}