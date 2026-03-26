using System.ComponentModel.DataAnnotations;

namespace RS1_2024_25.API.Data.DTOs
{
    public class CheckoutDTO
    {
        [Required(ErrorMessage = "User ID is required.")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Address information is missing.")]
        public AddressDTO Adresa { get; set; } 

        [Required(ErrorMessage = "Payment method must be selected.")]
        public string PaymentMethod { get; set; }

        public string? PromoCode { get; set; }
    }

    
}