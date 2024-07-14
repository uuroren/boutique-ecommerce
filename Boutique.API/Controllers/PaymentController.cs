using Boutique.API.Models;
using Boutique.Application.Dtos.PaymentDtos;
using Boutique.Application.Services.OrderServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Boutique.API.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController:ControllerBase {
        private readonly IOrderService _orderService;

        public PaymentController(IOrderService orderService) {
            _orderService = orderService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentRequestDTO request) {
            var response = await _orderService.CreateOrderAsync(request.CartId,request.PaymentCard);
            return Ok(response);
        }

        [HttpPost("callback")]
        public async Task<IActionResult> PaymentCallback([FromForm] string token) {
            if(string.IsNullOrEmpty(token)) {
                return BadRequest("Invalid token");
            }

            var order = await _orderService.HandlePaymentCallbackAsync(token);
            if(order == null) {
                return BadRequest(new BaseResponse() { Message = ""});
            }

            return Ok(new BaseResponse() { Message = "Siparişiniz başarıyla oluşturuldu!",Result = order,Success = true });
        }
    }
}
