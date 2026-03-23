using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.DTOs;
using RS1_2024_25.API.Data.Models.ShoppingCart;

namespace RS1_2024_25.API.Services
{



    public class CartService: ICartService
    {

        private readonly ApplicationDbContext _context;
        private readonly ILogger<Cart> _logger;

        public CartService(ApplicationDbContext context, ILogger<Cart> logger)
        {
            _context = context;
            _logger = logger;
        }

       
        public async Task MergeGuestCartWithUser(string? userId, string? guestSessionId,CancellationToken cancellationToken)
        {
                if (string.IsNullOrEmpty(guestSessionId) || string.IsNullOrEmpty(userId))
                    return;


            var guestCart = await _context.Carts
               .Include(c => c.Items)
               .ThenInclude(p => p.Product)
               .FirstOrDefaultAsync(c => c.GuestSessionId == guestSessionId, cancellationToken);

            if (guestCart == null)
                return;

            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);


            try
            {

           

       
                if (!guestCart.Items.Any())
                {
                    _context.Carts.Remove(guestCart);
                    await _context.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                    return;
                }

                var userCart = await _context.Carts
                    .Include(c => c.Items)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

               
                if (userCart == null)
                {
                    guestCart.UserId = userId;
                    guestCart.GuestSessionId = null;

                    foreach (var item in guestCart.Items)
                    {
                        item.UserId = userId;
                    }

                  
                }

                // Merge logika

                else
                {
                    var guestItems = guestCart.Items.ToList();

                    foreach (var guestItem in guestItems)
                    {
                        var existingItem = userCart.Items
                            .FirstOrDefault(i => i.ProductId == guestItem.ProductId);

                        if (existingItem != null)
                        {
                            int totalRequested = existingItem.Quantity + guestItem.Quantity;
                            int stock = guestItem.Product?.StockQuantity ?? totalRequested;

                            existingItem.Quantity = Math.Min(totalRequested, stock);

                            _context.CartItems.Remove(guestItem);
                        }
                        else
                        {

                            guestItem.CartId = userCart.Id;
                            guestItem.UserId = userId;
                        }
                    }


                    _context.Carts.Remove(guestCart);
                }
               
                await _context.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error merging guest cart with user cart for userId {UserId} and guestSessionId {GuestSessionId}", userId, guestSessionId);
            }
        }

    }
}