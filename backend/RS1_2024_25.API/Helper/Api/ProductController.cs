using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.DTOs;
using RS1_2024_25.API.Data.Models.ShoppingCart;

namespace RS1_2024_25.API.Helper.Api
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }
        [HttpGet("{id}")]

        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        [HttpPost]
       [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateProduct([FromBody] ProductUpdateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = new Product
            {
                Name=dto.Name,
                Price = dto.Price ?? 0,
                StockQuantity = dto.StockQuantity,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl,

            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return Ok(product);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] ProductUpdateDTO dto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

          if(dto.Name!=null)product.Name = dto.Name;
            if (dto.Price.HasValue) product.Price = dto.Price.Value;
            if (dto.StockQuantity.HasValue) product.StockQuantity = dto.StockQuantity.Value;
            if (dto.Description != null) product.Description = dto.Description;
            if (dto.ImageUrl != null) product.ImageUrl = dto.ImageUrl;
            if(dto.Active!=null)product.Active = dto.Active;
            if (dto.SKU != null) product.SKU = dto.SKU;
            if (dto.Brend != null) product.Brend = dto.Brend;
            if (dto.CategoryId != null) product.CategoryId = dto.CategoryId;
            if (dto.AdditionalImagesUrl != null) product.AdditionalImagesUrl = dto.AdditionalImagesUrl;


            await _context.SaveChangesAsync();
            return Ok(product);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Product deleted successfully." });
        }
    }

}
