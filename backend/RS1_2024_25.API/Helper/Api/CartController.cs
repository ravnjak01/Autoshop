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
                    Quantity = request.Quantity,
                    Cart = cart
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
        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout()
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

            if (!cart.Items.Any())
                return BadRequest("Cannot checkout with empty cart.");

            foreach (var item in cart.Items)
            {
                if (item.Product.StockQuantity < item.Quantity)
                {
                    return BadRequest($"Insufficient stock for {item.Product.Name}. Available: {item.Product.StockQuantity}, Requested: {item.Quantity}");
                }
                if(item.Product.Active == false)
                {
                    return BadRequest($"Product {item.Product.Name} is no longer available.");
                }
            }
         
            var order = new Order
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                Status = "Pending",
                Items = cart.Items.Select(i => new OrderItem
                {
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    UnitPrice = i.Product.Price
                }).ToList(),
                TotalAmount = cart.Items.Sum(i => i.Product.Price * i.Quantity)
            };

            _context.Orders.Add(order);

            foreach (var item in cart.Items)
            {
                item.Product.StockQuantity -= item.Quantity;
            }
           
            _context.CartItems.RemoveRange(cart.Items);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Order successfully created.",
                OrderId = order.Id,
                Total = order.TotalAmount,
                Status = order.Status
            });
        }

        //[HttpPut("update-quantity/{itemId}")]
        //public async Task<IActionResult> UpdateQuantity(int itemId, [FromBody] UpdateCartItemDTO request)
        //{
        //    var userId = _userManager.GetUserId(User);
        //    if (userId == null)
        //        return Unauthorized();

        //    var item = await _context.CartItems
        //        .Include(i => i.Cart)
        //        .FirstOrDefaultAsync(i => i.Id == itemId && i.Cart.UserId == userId);

        //    if (item == null)
        //        return NotFound("Item not found in your cart.");

        //    item.Quantity = request.Quantity;
        //    await _context.SaveChangesAsync();

        //    return Ok(new
        //    {
        //        Id = item.Id,
        //        ProductId = item.ProductId,
        //        ProductName = item.Product.Name,
        //        Price = item.Product.Price,
        //        Quantity = item.Quantity,
        //        ImageUrl = item.Product.ImageUrl
        //    });
        //}

       


    }



}
