using RS1_2024_25.API.Services;
using Moq;
using RS1_2024_25.API.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
namespace Autoshop.UnitTests
{
    public class PasswordTests
    {
        [Theory]
        [InlineData("abc", 0)]
        [InlineData("Sifra123", 3)]
        [InlineData("Sifra123!", 4)]
        public void PasswordScore_ShouldCalculateCorrectly(string password, int expectedScore)
        {
            var store = new Mock<IUserStore<User>>();
            var mockUserManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var authService = new AuthService(mockUserManager.Object, mockHttpContextAccessor.Object);


            var result = authService.GetPasswordStrengthScore(password);


            Assert.Equal(expectedScore, result);
        }
    }
}