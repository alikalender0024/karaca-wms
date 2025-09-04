using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Karaca.Wms.Api.Data;
using Karaca.Wms.Api.DTOs;
using Karaca.Wms.Api.Models;

namespace Karaca.Wms.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(AppDbContext context, IMapper mapper, ILogger<ProductsController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        // GET: api/products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
        {
            try
            {
                var products = await _context.Products.ToListAsync();
                _logger.LogInformation("Ürünler Listelendi");
                return Ok(_mapper.Map<IEnumerable<ProductDto>>(products));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ürünler getirilirken hata oluştu.");
                return StatusCode(500, "Ürünler getirilirken beklenmeyen bir hata oluştu.");
            }
        }

        // GET: api/products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    _logger.LogWarning("Ürün bulunamadı: {Id}", id);
                    return NotFound($"Ürün bulunamadı: {id}");
                }
                _logger.LogInformation("Ürün getirildi: {Id} - {Name}", id,product.Name); 
                return Ok(_mapper.Map<ProductDto>(product));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ürün getirilirken hata oluştu: {Id}", id);
                return StatusCode(500, "Ürün getirilirken beklenmeyen bir hata oluştu.");
            }
        }

        // POST: api/products
        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct(ProductDto productDto)
        {
            try
            {
                var product = _mapper.Map<Product>(productDto);
                _context.Products.Add(product);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Yeni ürün oluşturuldu: {SKU}", product.SKU);

                return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, _mapper.Map<ProductDto>(product));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ürün oluşturulurken hata oluştu: {SKU}", productDto.SKU);
                return StatusCode(500, "Ürün oluşturulurken beklenmeyen bir hata oluştu.");
            }
        }

        // PUT: api/products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, ProductDto productDto)
        {
            if (id != productDto.Id)
            {
                return BadRequest("ID eşleşmedi.");
            }

            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    _logger.LogWarning("Güncellenecek ürün bulunamadı: {Id}", id);
                    return NotFound($"Ürün bulunamadı: {id}");
                }

                _mapper.Map(productDto, product);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Ürün güncellendi: {SKU}", product.SKU);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ürün güncellenirken hata oluştu: {Id}", id);
                return StatusCode(500, "Ürün güncellenirken beklenmeyen bir hata oluştu.");
            }
        }

        // DELETE: api/products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                {
                    _logger.LogWarning("Silinecek ürün bulunamadı: {Id}", id);
                    return NotFound($"Ürün bulunamadı: {id}");
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Ürün silindi: {SKU}", product.SKU);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ürün silinirken hata oluştu: {Id}", id);
                return StatusCode(500, "Ürün silinirken beklenmeyen bir hata oluştu.");
            }
        }
    }
}
