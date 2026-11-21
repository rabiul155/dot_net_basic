using Dot_Net_Basic.DTOs;
using Dot_Net_Basic.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dot_Net_Basic.Controllers;

[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    /// <summary>GET api/orders or GET api/orders?customerId=1</summary>
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<OrderResponse>>> GetAll([FromQuery] int? customerId, CancellationToken cancellationToken)
        => Ok(await _orderService.GetAllAsync(customerId, cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<OrderResponse>> GetById(int id, CancellationToken cancellationToken)
        => Ok(await _orderService.GetByIdAsync(id, cancellationToken));

    [HttpPost]
    public async Task<ActionResult<OrderResponse>> Create(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var order = await _orderService.CreateAsync(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _orderService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
