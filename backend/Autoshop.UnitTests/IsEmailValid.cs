using RS1_2024_25.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoshop.UnitTests
{
    public class EmailValidationTests
    {
        [Theory]
        [InlineData("test@autoshop.com", true)]  // Validan
        [InlineData("kupac.sarajevo@bih.net.ba", true)] // Validan sa tačkama
        [InlineData("nevalidan-email.com", false)] // Fali @
        [InlineData("test@", false)] // Fali domena
        [InlineData("", false)] // Prazno
        [InlineData(null, false)] // Null


        public void IsValidEmail_CheckVariousFormats(string email, bool expectedResult)
        {
            var authService = new AuthService(null);

            var result = authService.IsValidEmail(email);

            Assert.Equal(expectedResult, result);
        }

    }
}
