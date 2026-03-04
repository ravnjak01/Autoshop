using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.DTOs;
using RS1_2024_25.API.Data.Models;
using RS1_2024_25.API.Data.Models.ShoppingCart;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CheckoutController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<CheckoutController> _logger;

    public CheckoutController(ApplicationDbContext context,
        UserManager<User> userManager,
        ILogger<CheckoutController> logger)
    {
        _context = context;
        _userManager = userManager;
        _logger = logger;
    }

    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout([FromBody] CheckoutDTO dto,CancellationToken cancellationToken)
    {
       

        if (!ModelState.IsValid) return BadRequest(ModelState);
        var userId = _userManager.GetUserId(User);
        if (userId == null) return Unauthorized();



            var cart = await _context.Carts
            .Include(c => c.Items)
            .ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(c => c.UserId == userId,cancellationToken);

        if (cart == null) return NotFound("Cart not found.");

        var itemsToBuy = cart.Items.Where(i => !i.SavedForLater).ToList();

        if (!itemsToBuy.Any())
            return BadRequest("Your active cart is empty. Move items from 'Saved for later' to proceed.");

        if (!cart.Items.Any()) return BadRequest("Cannot checkout with empty cart.");

        foreach (var item in itemsToBuy)
        {
            if (!item.Product.Active)
                return BadRequest($"Product {item.Product.Name} is no longer available.");
            if (item.Product.StockQuantity < item.Quantity)
                return BadRequest($"Insufficient stock for {item.Product.Name}.");
        }

        using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        try
        {

            var existingAddress = await _context.Addresses
                                .FirstOrDefaultAsync(a => a.UserId == userId &&
                                   a.Street == dto.Adresa.Street &&
                                 a.City == dto.Adresa.City &&
                                 a.PostalCode == dto.Adresa.PostalCode &&
                                 a.Country == dto.Adresa.Country, cancellationToken);
            int finalAddressId;


            if (existingAddress != null)
            {
                finalAddressId = existingAddress.Id;

            }
            else
            {
                var newAddress = new Address
                {
                    Street = dto.Adresa.Street,
                    City = dto.Adresa.City,
                    PostalCode = dto.Adresa.PostalCode,
                    Country = dto.Adresa.Country,
                    TelephoneNumber = dto.Adresa.TelephoneNumber,
                    UserId = userId
                };
                _context.Addresses.Add(newAddress);
                await _context.SaveChangesAsync(cancellationToken);
                finalAddressId = newAddress.Id;
            }

        

            var order = new Order
            {
                UserId = userId,
                AdresaId = finalAddressId,
                CreatedAt = DateTime.UtcNow,
                Status = "Pending",
                Items = itemsToBuy.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.Product.Price
                }).ToList(),
                TotalAmount = cart.Items.Sum(i => i.Product.Price * i.Quantity)
            };

            _context.Orders.Add(order);

            foreach (var item in itemsToBuy)
                item.Product.StockQuantity -= item.Quantity;

            _context.CartItems.RemoveRange(itemsToBuy);

            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return Ok(new CheckoutResponseDTO
            {
                Message = "Order successfully created.",
                OrderId = order.Id,
                Total = order.TotalAmount,
                Status = order.Status
            });
        }
        catch (OperationCanceledException)
        {
            await transaction.RollbackAsync(cancellationToken);
            return StatusCode(499, "Request cancelled.");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Checkout failed for user {UserId}", userId);
            return StatusCode(500, "An error occurred during checkout. Please try again.");
        }
    }
}