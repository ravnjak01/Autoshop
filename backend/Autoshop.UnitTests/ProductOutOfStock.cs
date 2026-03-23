using Duende.IdentityModel.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using RS1_2024_25.API.Data;
using RS1_2024_25.API.Data.DTOs;
using RS1_2024_25.API.Data.Models;
using RS1_2024_25.API.Data.Models.ShoppingCart;
using System.Security.Claims;

namespace Autoshop.UnitTests
{
    public class CheckoutTests
    {
        [Fact]
        public async Task Checkout_ShouldFail_WhenProductOutOfStock()
        {
            var connection = new SqliteConnection("Filename=:memory:");
            connection.Open(); 
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .EnableSensitiveDataLogging()
                .Options;

            var context = new ApplicationDbContext(options);
            context.Database.EnsureCreated();

            var user = new User 
            { 
                Id = "user1", 
                UserName = "test@test.com",
                FirstName = "Test",    
                LastName = "User"
            };

            var category = new Category
            {
                Id = 1,
                Code = "TEST",
                Name = "Category"
            };

            context.Users.Add(user);
            context.Categories.Add(category);
            await context.SaveChangesAsync();

            var product = new Product
            {
                Id = 1,
                Name = "Gume",
                StockQuantity = 2,
                Price = 100,
                Brend = "Continental",                 
                Description = "Test product",   
                ImageUrl = "/images/test.jpg", 
                SKU = "TEST-001",
                CategoryId = 1
            };

            context.Products.Add(product);
            await context.SaveChangesAsync();

            var cart = new Cart
            {
                UserId = user.Id,
                Items = new List<CartItem>()
            };

            context.Carts.Add(cart);
            await context.SaveChangesAsync();

            var cartItem = new CartItem
            {
                CartId = cart.Id,
                ProductId = product.Id,
                Quantity = 5
            };
            context.CartItems.Add(cartItem);
            await context.SaveChangesAsync();

            var userManager = TestHelpers.GetUserManager(user);

            var controller = new CheckoutController(context, userManager, TestHelpers.GetLogger());
            controller.ControllerContext = TestHelpers.GetControllerContext(user.Id);

            var dto = new CheckoutDTO
            {
                Adresa = new AddressDTO
                {
                    Street = "Test",
                    City = "Sarajevo",
                    PostalCode = "71000",
                    Country = "BiH",
                    TelephoneNumber = "123"
                }
            };

            var result = await controller.Checkout(dto, CancellationToken.None);

            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Contains("Insufficient stock", badRequest.Value!.ToString());

            connection.Close();
        }
    }
}

public static class TestHelpers
{
    public static UserManager<User> GetUserManager(User? user = null)
    {
        var store = new Mock<IUserStore<User>>();
        var userManager = new Mock<UserManager<User>>(
            store.Object, null, null, null, null, null, null, null, null
        );

        userManager.Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>()))
                   .Returns(user?.Id ?? "test-user");

        return userManager.Object;
    }

    public static ControllerContext GetControllerContext(string userId)
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId)
        }));

        return new ControllerContext
        {
            HttpContext = new DefaultHttpContext { User = user }
        };
    }

    public static ILogger<CheckoutController> GetLogger()
    {
        return new Mock<ILogger<CheckoutController>>().Object;
    }
}