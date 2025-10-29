using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.DTOs;
using RS1_2024_25.API.Data.Models;
using RS1_2024_25.API.Data.Models.ShoppingCart;

namespace RS1_2024_25.API.Helper.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public CheckoutController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] CheckoutDTO dto)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return Unauthorized();

            var cart = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return NotFound("Cart not found.");

            var items = cart.Items;
            if (!items.Any())
                return BadRequest("Cannot checkout with empty cart.");

            try
            {
                var address = new Address
                {
                    Street = dto.Adresa.Street,
                    City = dto.Adresa.City,
                    PostalCode = dto.Adresa.PostalCode,
                    Country = dto.Adresa.Country,
                    TelephoneNumber = dto.Adresa.TelephoneNumber,
                    UserId = userId
                };

                _context.Addresses.Add(address);
                await _context.SaveChangesAsync();

                foreach (var item in items)
                {
                    if (item.Product.StockQuantity < item.Quantity)
                        return BadRequest($"Insufficient stock for {item.Product.Name}.");

                    if (item.Product.Active == false)
                        return BadRequest($"Product {item.Product.Name} is no longer available.");
                }

                var order = new Order
                {
                    UserId = userId,
                    AdresaId = address.Id,
                    CreatedAt = DateTime.UtcNow,
                    Status = "Pending",
                    Items = items.Select(i => new OrderItem
                    {
                        ProductId = i.ProductId,
                        Quantity = i.Quantity,
                        UnitPrice = i.Product.Price
                    }).ToList(),
                    TotalAmount = items.Sum(i => i.Product.Price * i.Quantity)
                };

                _context.Orders.Add(order);

                foreach (var item in items)
                    item.Product.StockQuantity -= item.Quantity;

                _context.CartItems.RemoveRange(items);

                await _context.SaveChangesAsync();

                var responseDTO = new CheckoutResponseDTO
                {
                    Message = "Order successfully created.",
                    OrderId = order.Id,
                    Total = order.TotalAmount,
                    Status = order.Status
                };

                return Ok(responseDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Checkout failed: {ex.Message}");
            }
        }

    }
}
