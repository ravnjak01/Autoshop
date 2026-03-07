//using RS1_2024_25.API.Endpoints.ProductEndpoints;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Autoshop.UnitTests
//{
//    public class ProductGetAll_MinMaxPriceTests
//    {
//        [Theory]
//        [InlineData(200, 100)]   // ❌ max < min
//        [InlineData(10, 5)]      // ❌ max < min
//        public async Task HandleAsync_ShouldThrowException_WhenMaxPriceIsLowerThanMinPrice(
//            decimal minPrice,
//            decimal maxPrice)
//        {
//            var request = new ProductGetAllRequest
//            {
//                MinPrice = minPrice,
//                MaxPrice = maxPrice
//            };

           
//            var endpoint = new ProductGetAll(
//                db: null!,
//                userManager: null!,
//                httpContextAccessor: null!
//            );

//            // Act & Assert
//            await Assert.ThrowsAsync<Exception>(() =>
//                endpoint.HandleAsync(request)
//            );
//        }
//    }
//}
