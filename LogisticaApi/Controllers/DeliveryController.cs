using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LogisticaApi.Services;
using LogisticaApi.Models;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class DeliveryController : ControllerBase
{
    private readonly DeliveryService _deliveryService;

    public DeliveryController(DeliveryService deliveryService)
    {
        _deliveryService = deliveryService;
    }
       
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateDelivery([FromBody] Delivery delivery)
    {
        try
        {
            if (delivery == null || string.IsNullOrEmpty(delivery.ProductId))
                return BadRequest("Os dados da entrega estão inválidos ou sem um ProductId.");

            string deliveryId = await _deliveryService.AddDeliveryAsync(delivery);
            return CreatedAtAction(nameof(GetDeliveryById), new { id = deliveryId }, new { Id = deliveryId });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao criar entrega: {ex.Message}");
        }
    }
       
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDeliveryById(string id)
    {
        try
        {
            var delivery = await _deliveryService.GetDeliveryByIdAsync(id);
            if (delivery == null)
                return NotFound($"Entrega com ID {id} não encontrada.");

            return Ok(delivery);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao buscar entrega: {ex.Message}");
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllDeliveries()
    {
        try
        {
            var deliveries = await _deliveryService.GetAllDeliveriesAsync();
            return Ok(deliveries);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao listar entregas: {ex.Message}");
        }
    }
}
