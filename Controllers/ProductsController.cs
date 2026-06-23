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
    public IActionResult GetAll()
    {
        return Ok(_productService.GetAll());
    }

    [HttpGet("{id:int}")]
    public IActionResult Get(int id)
    {
        var product = _productService.Get(id);
        return product is not null ? Ok(product) : NotFound();
    }

    [HttpPost]
    public IActionResult Create(ProductCreateDto productDto)
    {
        var product = _productService.Add(productDto);
        return CreatedAtAction(nameof(Get), new { id = product.Id }, product);
    }

    [HttpPut("{id:int}")]
    public IActionResult Update(int id, ProductUpdateDto productDto)
    {
        return _productService.Update(id, productDto) ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        return _productService.Delete(id) ? NoContent() : NotFound();
    }
}
