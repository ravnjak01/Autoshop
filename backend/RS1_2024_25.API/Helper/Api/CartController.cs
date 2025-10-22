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


      
        [HttpPost("add-to-cart")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDTO request)
        {
            if (request == null || request.ProductId <= 0 || request.Quantity <= 0)
                return BadRequest("Invalid data.");

            var userId = _userManager.GetUserId(User); 
            if (userId == null)
                return Unauthorized();

            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == request.ProductId);
            if (product == null)
                return NotFound("Product not found.");

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    Items = new List<CartItem>()
                };
                _context.Carts.Add(cart);
            }

            var existingItem = cart.Items.FirstOrDefault(i => i.ProductId == request.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += request.Quantity;
                _context.CartItems.Update(existingItem);
            }
            else
            {
                var newItem = new CartItem
                {
                    ProductId = request.ProductId,
                    Product=product,
                    Quantity = request.Quantity,
                    Cart = cart,
                    UserId=userId
                };
                cart.Items.Add(newItem);
            }

            await _context.SaveChangesAsync();

            return Ok(new { Message = $"{_context.Products.Find(request.ProductId)?.Name} added to the cart!", 
            ProductId=request.ProductId});
        }

       
        [HttpGet("my-cart")]
        public async Task<IActionResult> GetMyCart()
        {

            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return Unauthorized();

            var cart = await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || cart.Items == null || !cart.Items.Any())
                return Ok(new List<CartItemDTO>());


            var items = cart.Items.Select(i => new CartItemDTO
            {
                Id = i.Id,
                ProductId = i.ProductId,
                ProductName = i.Product.Name,
                Quantity = i.Quantity,
                Price = i.Product.Price,
                Total = i.Product.Price * i.Quantity,
                imageUrl=i.Product.ImageUrl
            }).ToList();

            var result = cart.Items.Select(i => new CartResponseDTO
            {
                Items = items,
                ItemCount=items.Sum(x => x.Quantity),
                Total = items.Sum(x => x.Total)
            }).ToList();

            return Ok(result);
        }

        [HttpDelete("remove/{productId}")]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return Unauthorized();

            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return NotFound("Cart not found.");

            var item = cart.Items.FirstOrDefault(i => i.ProductId == productId);
            if (item == null)
                return NotFound("Product not in cart.");

            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Product removed from cart." });
        }


        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
                return Unauthorized();

            var cart = await _context.Carts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
                return NotFound("Cart not found.");

            _context.CartItems.RemoveRange(cart.Items);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Cart cleared." });
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
    }



    }




