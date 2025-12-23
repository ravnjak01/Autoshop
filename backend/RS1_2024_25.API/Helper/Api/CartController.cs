using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.DTOs;
using RS1_2024_25.API.Data.Models;
using RS1_2024_25.API.Data.Models.ShoppingCart;
using System.Linq;
using System.Threading.Tasks;

namespace RS1_2024_25.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class CartController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public CartController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost("add-to-cart")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDTO request)
        {
            if (request == null || request.ProductId <= 0 || request.Quantity <= 0)
                return BadRequest("Invalid data.");

            // ako nije prijavljen - koristi temporary sessionId
            var userId = _userManager.GetUserId(User);
            string sessionId = Request.Cookies["guest_session"];

            if (userId == null && sessionId == null)
            {
                // kreiraj privremeni ID gosta
                sessionId = Guid.NewGuid().ToString();
                Response.Cookies.Append("guest_session", sessionId, new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddDays(7),
                    HttpOnly = true,
                    SameSite = SameSiteMode.None,
                    Secure = true
                });
            }

          
            Cart? cart = null;

            if (userId != null)
            {
                cart = await _context.Carts.Include(c => c.Items)
                                           .FirstOrDefaultAsync(c => c.UserId == userId);
            }
            else if (sessionId != null)
            {
                cart = await _context.Carts.Include(c => c.Items)
                                           .FirstOrDefaultAsync(c => c.GuestSessionId == sessionId);
            }

            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.ProductId);
            if (product == null)
                return NotFound("Product not found.");

            var existingItem = cart?.Items
                    .FirstOrDefault(i => i.ProductId == request.ProductId);

            int currentQuantityInCart = existingItem?.Quantity ?? 0;
            int requestedTotal = currentQuantityInCart + request.Quantity;

            if (requestedTotal > product.StockQuantity)
            {
                return BadRequest(new
                {
                    message = $"Only {product.StockQuantity} items available in stock."
                });
            }
            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    GuestSessionId = sessionId,
                    Items = new List<CartItem>()
                };
                _context.Carts.Add(cart);
            }

            if (existingItem != null)
                existingItem.Quantity = requestedTotal;
            else
                cart.Items.Add(new CartItem
                {
                    ProductId = request.ProductId,
                    Product = product,
                    Quantity = request.Quantity,
                    Cart = cart,
                    UserId = userId
                });

            await _context.SaveChangesAsync();
            return Ok(new { message = $"{product.Name} added to the cart!" });
        }
        [AllowAnonymous]
        [HttpGet("my-cart")]
        public async Task<IActionResult> GetMyCart()
        {
            var userId = _userManager.GetUserId(User);
            string? sessionId = Request.Cookies["guest_session"];

    
            if (userId == null && sessionId == null)
                return Ok(new CartResponseDTO
                {
                    Items = new List<CartItemDTO>(),
                    ItemCount = 0,
                    Total = 0
                });

            Cart? cart = null;

            if (userId != null)
            {
                cart = await _context.Carts
                    .Include(c => c.Items)
                    .ThenInclude(i => i.Product)
                    .FirstOrDefaultAsync(c => c.UserId == userId);
            }
            else if (sessionId != null)
            {
                cart = await _context.Carts
                    .Include(c => c.Items)
                    .ThenInclude(i => i.Product)
                    .FirstOrDefaultAsync(c => c.GuestSessionId == sessionId);
            }

            if (cart == null || cart.Items == null || !cart.Items.Any())
                return Ok(new CartResponseDTO
                {
                    Items = new List<CartItemDTO>(),
                    ItemCount = 0,
                    Total = 0
                });

            var items = cart.Items.Select(i => new CartItemDTO
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.Product.Name,
                Quantity = i.Quantity,
                Price = i.Product.Price,
                imageUrl = i.Product.ImageUrl,
                Total = i.Product.Price * i.Quantity
            }).ToList();

            return Ok(new CartResponseDTO
            {
                Items = items,
                ItemCount = items.Sum(x => x.Quantity),
                Total = items.Sum(x => x.Total)
            });
        }


       
      
       
        [HttpPost("save-for-later/{productId}")]
        public async Task<IActionResult> SaveForLater(int productId)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return Unauthorized();

            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(i => i.ProductId == productId && i.UserId == userId);

            if (cartItem == null)
                return NotFound("Product not found in cart.");

            cartItem.SavedForLater = true;
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Product saved for later." });
        }
        [HttpPost("move-to-cart/{productId}")]
        public async Task<IActionResult> MoveToCart(int productId)
        {
            var userId = _userManager.GetUserId(User);
            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null) return NotFound("Cart not found");

            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (item == null) return NotFound("Item not found");

            item.SavedForLater = false;
            await _context.SaveChangesAsync();

            return Ok();
        }
        [AllowAnonymous]
        [HttpPut("update/{itemId}")]
        public async Task<IActionResult> UpdateQuantity(int itemId, [FromBody] UpdateCartItemDTO request)
        {
            var userId = _userManager.GetUserId(User);
            string? sessionId = Request.Cookies["guest_session"];

            if (request.Quantity <= 0)
                return BadRequest("Quantity has to be higher than 0.");

            if (userId == null && sessionId == null)
                return Unauthorized("Cart not found.");

            var item = await _context.CartItems
                .Include(i => i.Cart)
                .Include(i => i.Product)
                .FirstOrDefaultAsync(i =>
                    i.Id == itemId &&
                    (i.Cart.UserId == userId || i.Cart.GuestSessionId == sessionId)
                );

            if (item == null)
                return NotFound("Item not found in your cart.");

         
            if (request.Quantity > item.Product.StockQuantity)
                return BadRequest($"In stock we have {item.Product.StockQuantity} pieces.");

            item.Quantity = request.Quantity;
            await _context.SaveChangesAsync();

            return Ok(new CartItemDTO
            {
                Id = item.Id,
                ProductId = item.ProductId,
                ProductName = item.Product.Name,
                Price = item.Product.Price,
                Quantity = item.Quantity,
                imageUrl = item.Product.ImageUrl,
                Total = item.Product.Price * item.Quantity
            });
        }
        [AllowAnonymous]
        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart()
        {
            try
            {
               
                var userId = _userManager.GetUserId(User);
                Cart? cart = null;

                if (!string.IsNullOrEmpty(userId))
                {
                    
                    cart = await _context.Carts
                        .Include(c => c.Items)
                        .FirstOrDefaultAsync(c => c.UserId == userId);
                }
                else
                {
                    
                    var guestSessionId = Request.Cookies["guest_session"];

                    if (string.IsNullOrEmpty(guestSessionId))
                    {
                        return NotFound(new { Message = "No cart found." });
                    }

                    cart = await _context.Carts
                        .Include(c => c.Items)
                        .FirstOrDefaultAsync(c => c.GuestSessionId == guestSessionId);
                }

                if (cart == null)
                    return NotFound(new { Message = "Cart not found." });

               
                if (cart.Items == null || !cart.Items.Any())
                    return Ok(new { Message = "Cart is already empty." });

                _context.CartItems.RemoveRange(cart.Items);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Cart cleared successfully." });
            }
            catch (Exception ex)
            {
          
                Console.WriteLine($"Error clearing cart: {ex.Message}");
                return StatusCode(500, new { Message = "Error clearing cart." });
            }
        }

        [AllowAnonymous]
        [HttpDelete("remove/{itemId}")]
        public async Task<IActionResult> RemoveFromCart(int itemId)
        {
            var userId = _userManager.GetUserId(User);
            string? sessionId = Request.Cookies["guest_session"];

            if (userId == null && sessionId == null)
                return Unauthorized("Cart not found.");

            Cart? cart = null;

            if (userId != null)
            {
                cart = await _context.Carts
                    .Include(c => c.Items)
                    .ThenInclude(i => i.Product)
                    .FirstOrDefaultAsync(c => c.UserId == userId);
            }
            else if (sessionId != null)
            {
                cart = await _context.Carts
                    .Include(c => c.Items)
                    .ThenInclude(i => i.Product)
                    .FirstOrDefaultAsync(c => c.GuestSessionId == sessionId);
            }

            if (cart == null)
                return NotFound(new { Message = "Cart not found." });

            var item = cart.Items.FirstOrDefault(i => i.Id == itemId);

            if (item == null)
                return NotFound(new { Message = "Item not found in cart." });

            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();

            var remainingItems = cart.Items
                .Where(i => i.Id != itemId) 
                .Select(i => new CartItemDTO
                {
                    Id = i.Id,
                    ProductId = i.ProductId,
                    ProductName = i.Product.Name,
                    Quantity = i.Quantity,
                    Price = i.Product.Price,
                    imageUrl = i.Product.ImageUrl,
                    Total = i.Product.Price * i.Quantity
                }).ToList();

            return Ok(new CartResponseDTO
            {
                Items = remainingItems,
                ItemCount = remainingItems.Sum(x => x.Quantity),
                Total = remainingItems.Sum(x => x.Total),
              
            });
        }




    }



}




