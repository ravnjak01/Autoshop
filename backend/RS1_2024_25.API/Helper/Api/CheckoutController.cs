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
    public async Task<IActionResult> Checkout([FromBody] CheckoutDTO dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var userId = _userManager.GetUserId(User);
        if (userId == null) return Unauthorized();

        var strategy = _context.Database.CreateExecutionStrategy();

        return await strategy.ExecuteAsync<IActionResult>(async () =>
        {
            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var cart = await _context.Carts
                    .Include(c => c.Items)
                    .ThenInclude(i => i.Product)
                    .FirstOrDefaultAsync(c => c.UserId == userId, cancellationToken);

                if (cart == null) return NotFound("Cart not found.");

                var itemsToBuy = cart.Items.Where(i => !i.SavedForLater).ToList();

                if (!itemsToBuy.Any())
                    return BadRequest("Your active cart is empty. Move items from 'Saved for later' to proceed.");

                var now = DateTime.UtcNow;
                
                var productIds = itemsToBuy.Select(i => i.ProductId).ToList();
                var categoryIds = itemsToBuy.Select(i => i.Product.CategoryId).Distinct().ToList();

                var categoryDiscounts = await _context.DiscountCategories
                .Where(dc => categoryIds.Contains(dc.CategoryId))
                .ToDictionaryAsync(dc => dc.CategoryId, dc => dc.Discount.DiscountPercentage, cancellationToken);

                var productDiscounts = await _context.DiscountProducts
                    .Where(dp => productIds.Contains(dp.ProductId))
                    .ToDictionaryAsync(dp => dp.ProductId, dp => dp.Discount.DiscountPercentage, cancellationToken);

                decimal promoDiscountPercent = 0;
                if (!string.IsNullOrEmpty(dto.PromoCode))
                {
                    var promo = await _context.DiscountCodes
                        .Include(p => p.Discount)
                        .FirstOrDefaultAsync(p => p.Code == dto.PromoCode, cancellationToken);

                    if (promo == null || promo.ValidFrom > now || promo.ValidTo < now)
                    {
                        return BadRequest("Invalid promo code.");
                    }

                    promoDiscountPercent = promo.Discount.DiscountPercentage;
                }

                decimal CalculateFinalUnitPrice(decimal basePrice, decimal categoryDisc, decimal productDisc)
                {
                    var finalDisc = Math.Max(categoryDisc, productDisc);
                    return basePrice * (1 - finalDisc / 100);
                }

                decimal finalOrderTotal = 0;
                var orderItems = new List<OrderItem>();

                foreach (var item in itemsToBuy)
                {
                    if (!item.Product.Active)
                        return BadRequest($"Product {item.Product.Name} is no longer available.");
                    if (item.Product.StockQuantity < item.Quantity)
                        return BadRequest($"Insufficient stock for {item.Product.Name}.");

                    var categoryDisc = categoryDiscounts.TryGetValue(item.Product.CategoryId, out var cd) ? cd : 0;
                    var productDisc = productDiscounts.TryGetValue(item.ProductId, out var pd) ? pd : 0;

                    var finalUnitPrice = CalculateFinalUnitPrice(item.Product.Price, categoryDisc, productDisc);

                    finalOrderTotal += finalUnitPrice * item.Quantity;

                    orderItems.Add(new OrderItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = finalUnitPrice 
                    });

                    item.Product.StockQuantity -= item.Quantity;
                }

                if (promoDiscountPercent > 0)
                {
                    finalOrderTotal -= finalOrderTotal * promoDiscountPercent / 100;
                }

                decimal shippingFee = 10;
                finalOrderTotal += shippingFee;

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
                    Status = Status.Pending,
                    Items = orderItems,
                    TotalAmount = finalOrderTotal
                };

                _context.Orders.Add(order);
                _context.CartItems.RemoveRange(itemsToBuy);

                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return Ok(new CheckoutResponseDTO
                {
                    Message = "Order successfully created.",
                    OrderId = order.Id,
                    Total = order.TotalAmount,
                    Status = order.Status.ToString()
                });
            }
            catch (OperationCanceledException)
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(CancellationToken.None);
                _logger.LogError(ex, "Checkout failed for user {UserId}", userId);
                throw; 
            }
        });
    }
}