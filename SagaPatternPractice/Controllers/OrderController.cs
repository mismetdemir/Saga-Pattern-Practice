using Microsoft.AspNetCore.Mvc;
using SagaPatternPractice.DTOs;
using SagaPatternPractice.Services;

namespace SagaPatternPractice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderSagaService _orderSagaService;

        public OrderController(OrderSagaService orderSagaService)
        {
            _orderSagaService = orderSagaService;
        }

        [HttpPost]
        public async Task<ActionResult<SagaResponseDto>> CreateOrder(
            OrderDto dto,
            [FromQuery] bool paymentFail,
            [FromQuery] bool cargoFail)
        {
            var result = await _orderSagaService.CreateOrderAsync(dto.ProductId, dto.Quantity, paymentFail, cargoFail);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
