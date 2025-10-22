using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.DTOs;
using RS1_2024_25.API.Data.Models.ShoppingCart;

namespace RS1_2024_25.API.Services
{

    public class CartService
    {

        private readonly ApplicationDbContext _context;

        public CartService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddToCartAsync(string userId, AddToCartDTO request)
        {
            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == request.ProductId && p.Active);

            if (product == null)
            {
                throw new Exception("Proizvod nije pronađen ili nije aktivan.");
            }

            if (product.StockQuantity < request.Quantity)
            {
                throw new Exception("Nema dovoljno proizvoda na lageru.");
            }

            // Dohvati korisnikovu korpu ili kreiraj novu
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    Items = new List<CartItem>()
                };
                _context.Carts.Add(cart);
            }

            // Provjeri da li proizvod već postoji u korpi
            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += request.Quantity;
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    ProductId = request.ProductId,
                    Quantity = request.Quantity
                });
            }

            // Smanji količinu proizvoda na lageru
            product.StockQuantity -= request.Quantity;

            await _context.SaveChangesAsync();
        }

    }
}