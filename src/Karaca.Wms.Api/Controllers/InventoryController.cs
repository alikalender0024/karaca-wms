using AutoMapper;
using Karaca.Wms.Api.Data;
using Karaca.Wms.Api.DTOs;
using Karaca.Wms.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Karaca.Wms.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<InventoryController> _logger;

        public InventoryController(AppDbContext context, IMapper mapper, ILogger<InventoryController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryDto>>> GetInventory()
        {
            var items = await _context.Inventories.Include(i => i.Product).Include(i => i.Location).ToListAsync();
            return Ok(_mapper.Map<IEnumerable<InventoryDto>>(items));
        }

        [HttpPost]
        public async Task<ActionResult<InventoryDto>> CreateInventory(InventoryDto dto)
        {
            try
            {
                var inventory = _mapper.Map<Inventory>(dto);
                _context.Inventories.Add(inventory);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Yeni inventory kaydı oluşturuldu: ProductId={ProductId}, LocationId={LocationId}", dto.ProductId, dto.LocationId);
                return CreatedAtAction(nameof(GetInventory), new { id = inventory.Id }, _mapper.Map<InventoryDto>(inventory));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Inventory oluşturulurken hata oluştu: ProductId={ProductId}, LocationId={LocationId}", dto.ProductId, dto.LocationId);
                return StatusCode(500, "Inventory oluşturulurken hata oluştu.");
            }
        }

        // PUT: api/inventory/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInventory(int id, InventoryDto dto)
        {
            if (id != dto.Id)
                return BadRequest("ID eşleşmedi.");

            try
            {
                var inventory = await _context.Inventories.FindAsync(id);
                if (inventory == null)
                {
                    _logger.LogWarning("Güncellenecek inventory bulunamadı: {Id}", id);
                    return NotFound($"Inventory bulunamadı: {id}");
                }

                _mapper.Map(dto, inventory);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Inventory güncellendi: ProductId={ProductId}, LocationId={LocationId}", dto.ProductId, dto.LocationId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Inventory güncellenirken hata oluştu: {Id}", id);
                return StatusCode(500, "Inventory güncellenirken hata oluştu.");
            }
        }

        // DELETE: api/inventory/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventory(int id)
        {
            try
            {
                var inventory = await _context.Inventories.FindAsync(id);
                if (inventory == null)
                {
                    _logger.LogWarning("Silinecek inventory bulunamadı: {Id}", id);
                    return NotFound($"Inventory bulunamadı: {id}");
                }

                _context.Inventories.Remove(inventory);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Inventory silindi: ProductId={ProductId}, LocationId={LocationId}", inventory.ProductId, inventory.LocationId);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Inventory silinirken hata oluştu: {Id}", id);
                return StatusCode(500, "Inventory silinirken hata oluştu.");
            }
        }

    }
}
