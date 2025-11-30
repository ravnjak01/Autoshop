using Duende.IdentityServer.Models;
using System.ComponentModel.DataAnnotations;

namespace RS1_2024_25.API.Data.DTOs
{
    public class UpdateCartItemDTO
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity has to be 1 or higher.")]
        public int Quantity { get; set; }
    }
}
