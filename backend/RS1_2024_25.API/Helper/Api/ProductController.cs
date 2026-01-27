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
        public async Task<IActionResult> CreateProduct([FromBody] ProductCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = new Product
            {
                Name = dto.Name,
                SKU = dto.SKU,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl,
                CategoryId = dto.CategoryId,
                Brend = dto.Brend,
                Active = dto.Active

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
           if(dto.Price.HasValue) product.Price = dto.Price.Value;
            if (dto.StockQuantity.HasValue) product.StockQuantity = dto.StockQuantity.Value;
            if (dto.Description != null) product.Description = dto.Description;
            if (dto.ImageUrl != null) product.ImageUrl = dto.ImageUrl;
            if(dto.Active!=null)product.Active = dto.Active.Value;
            if (dto.SKU != null) product.SKU = dto.SKU;
            if (dto.Brend != null) product.Brend = dto.Brend;
            if(dto.CategoryId.HasValue) product.CategoryId = dto.CategoryId.Value;
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


            if (_context.OrderItems.Any(o => o.ProductId == id))
                return BadRequest("Cant delete product thats already part of the order.");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return Ok(new { Message = "Product deleted successfully." });
        }

        [HttpGet("images")]
        public IActionResult GetImages()
        {
            string imageFolderPath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot", "images","products");

            if (!Directory.Exists(imageFolderPath))
                return NotFound("Folder Images doesnt exist.");

            var files = Directory.GetFiles(imageFolderPath)
                .Select(Path.GetFileName)
               
                .ToList();

            return Ok(files);
        }

    }

}
