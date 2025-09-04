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
    public class LocationsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<LocationsController> _logger;

        public LocationsController(AppDbContext context, IMapper mapper, ILogger<LocationsController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LocationDto>>> GetLocations()
        {
            try
            {
                var locations = await _context.Locations.ToListAsync();
                return Ok(_mapper.Map<IEnumerable<LocationDto>>(locations));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lokasyonlar getirilirken hata oluştu.");
                return StatusCode(500, "Lokasyonlar getirilirken hata oluştu.");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LocationDto>> GetLocation(int id)
        {
            try
            {
                var location = await _context.Locations.FindAsync(id);
                if (location == null)
                {
                    _logger.LogWarning("Lokasyon bulunamadı: {Id}", id);
                    return NotFound($"Lokasyon bulunamadı: {id}");
                }
                return Ok(_mapper.Map<LocationDto>(location));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lokasyon getirilirken hata oluştu: {Id}", id);
                return StatusCode(500, "Lokasyon getirilirken hata oluştu.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<LocationDto>> CreateLocation(LocationDto dto)
        {
            try
            {
                var location = _mapper.Map<Location>(dto);
                _context.Locations.Add(location);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Yeni lokasyon oluşturuldu: {Code}", location.Code);
                return CreatedAtAction(nameof(GetLocation), new { id = location.Id }, _mapper.Map<LocationDto>(location));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lokasyon oluşturulurken hata oluştu: {Code}", dto.Code);
                return StatusCode(500, "Lokasyon oluşturulurken hata oluştu.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLocation(int id, LocationDto dto)
        {
            if (id != dto.Id) return BadRequest("ID eşleşmedi.");
            try
            {
                var location = await _context.Locations.FindAsync(id);
                if (location == null)
                {
                    _logger.LogWarning("Güncellenecek lokasyon bulunamadı: {Id}", id);
                    return NotFound($"Lokasyon bulunamadı: {id}");
                }
                _mapper.Map(dto, location);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Lokasyon güncellendi: {Code}", location.Code);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lokasyon güncellenirken hata oluştu: {Id}", id);
                return StatusCode(500, "Lokasyon güncellenirken hata oluştu.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLocation(int id)
        {
            try
            {
                var location = await _context.Locations.FindAsync(id);
                if (location == null)
                {
                    _logger.LogWarning("Silinecek lokasyon bulunamadı: {Id}", id);
                    return NotFound($"Lokasyon bulunamadı: {id}");
                }
                _context.Locations.Remove(location);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Lokasyon silindi: {Code}", location.Code);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lokasyon silinirken hata oluştu: {Id}", id);
                return StatusCode(500, "Lokasyon silinirken hata oluştu.");
            }
        }
    }
}
