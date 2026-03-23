using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.DTOs;
using RS1_2024_25.API.Data.Models;
using RS1_2024_25.API.Data.Models.ShoppingCart;

namespace RS1_2024_25.API.Helper.Api
{

    [Authorize(Roles ="Admin")]
    [Route("api/product")]
    [ApiController]
    public class ProductCRUDController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductCRUDController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]

        public async Task<ActionResult<ProductReadDTO>> GetProduct(int id, CancellationToken cancellationToken)
        {

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
            if (product == null)
                return NotFound();

            var dto = new ProductReadDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                SKU = product.SKU,
                Brend = product.Brend,
                Active = product.Active,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name, 
                CreatedAt = product.CreatedAt

              
            };


            return Ok(dto);
        }

        [HttpPost]
        public async Task<ActionResult<ProductReadDTO>> CreateProduct([FromBody] ProductCreateDTO dto,CancellationToken cancellationToken)
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
                Active = dto.StockQuantity > 0,
                CreatedAt = DateTime.UtcNow
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync(cancellationToken);

            var readDto=new ProductReadDTO
            {
                Id = product.Id,
                Name = product.Name,
                SKU = product.SKU,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                Description = product.Description,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId,
                Brend = product.Brend,
                Active = product.Active,
                CreatedAt = product.CreatedAt
            };


            return CreatedAtAction("GetProduct", new { id = product.Id }, readDto);
        }

        [HttpPut("{id}")]

        public async Task<ActionResult<ProductReadDTO>> UpdateProduct(int id, [FromBody] ProductUpdateDTO dto,CancellationToken cancellationToken)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != dto.Id)
                return BadRequest("Product ID mismatch");

            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

          if(dto.Name!=null)product.Name = dto.Name;
           if(dto.Price.HasValue) product.Price = dto.Price.Value;
            if (dto.StockQuantity.HasValue) product.StockQuantity = dto.StockQuantity.Value;
            if (dto.Description != null) product.Description = dto.Description;
            if (dto.ImageUrl != null) product.ImageUrl = dto.ImageUrl;
            if (dto.SKU != null) product.SKU = dto.SKU;
            if (dto.Brend != null) product.Brend = dto.Brend;
            if(dto.CategoryId.HasValue) product.CategoryId = dto.CategoryId.Value;
            if (dto.AdditionalImagesUrl != null) product.AdditionalImagesUrl = dto.AdditionalImagesUrl;
            if (dto.StockQuantity.HasValue)
            {
                product.StockQuantity = dto.StockQuantity.Value;
                product.Active = product.StockQuantity > 0;
            }

            await _context.SaveChangesAsync(cancellationToken);

            var response = new ProductReadDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                ImageUrl = product.ImageUrl,
                SKU = product.SKU,
                Brend = product.Brend,
                CategoryId = product.CategoryId,
                StockQuantity = product.StockQuantity 
            };

            return Ok(response);
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteProduct(int id, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FindAsync(id,cancellationToken);
            if (product == null || !product.Active)
                return NotFound();

            product.Active = false;

            await _context.SaveChangesAsync(cancellationToken);
            return Ok(new { Message = $"Product '{product.Name}' deactivated successfully." });
        }

        [HttpGet("images")]
        [AllowAnonymous]
        public IActionResult GetImages()
        {
            string imageFolderPath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot", "images","products");

            if (!Directory.Exists(imageFolderPath))
                return NotFound("Folder Images doesnt exist.");

            var baseUrl = $"{Request.Scheme}://{Request.Host}";

            var files = Directory.GetFiles(imageFolderPath)
               .Select(fileName => $"{baseUrl}/images/products/{Path.GetFileName(fileName)}")
                .ToList();


            return Ok(files);
        }

    }

}
