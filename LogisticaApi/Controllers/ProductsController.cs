using LogisticaApi.Models;
using LogisticaApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductsController(ProductService productService)
    {
        _productService = productService;
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddProduct([FromBody] Product product)
    {
        try
        {
            string productId = await _productService.AddProductAsync(product);
            return Ok(new { Id = productId, Message = "Produto adicionado com sucesso" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(string id)
    {
        try
        {
            var products = await _productService.GetProductByIdAsync(id);
            if (products == null)
                return NotFound($"Entrega com ID {id} não encontrada.");

            return Ok(products);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao buscar produto: {ex.Message}");
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        try
        {
            var products = await _productService.GetAllProductsAsync();
            return Ok(products);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao listar produtos: {ex.Message}");
        }
    }
}