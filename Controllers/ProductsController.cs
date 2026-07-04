using Dot_Net_Basic.Models;
using Dot_Net_Basic.Services;
using Microsoft.AspNetCore.Mvc;

namespace Dot_Net_Basic.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductsController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var products = await _productService.GetAllAsync(cancellationToken);
        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
    {
        var product = await _productService.GetAsync(id, cancellationToken);
        return product is not null ? Ok(product) : NotFound();
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductCreateDto productDto, CancellationToken cancellationToken)
    {
        var product = await _productService.AddAsync(productDto, cancellationToken);
        return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] ProductUpdateDto productDto, CancellationToken cancellationToken)
    {
        return await _productService.UpdateAsync(id, productDto, cancellationToken) ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        return await _productService.DeleteAsync(id, cancellationToken) ? NoContent() : NotFound();
    }
}
