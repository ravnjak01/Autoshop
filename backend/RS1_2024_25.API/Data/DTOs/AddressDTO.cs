using System.ComponentModel.DataAnnotations;

public class AddressDTO
{
    [Required(ErrorMessage = "Street is required.")]
    public string Street { get; set; }

    [Required(ErrorMessage = "City is required.")]
    public string City { get; set; }

    [Required(ErrorMessage = "Postal code is required.")]
    public string PostalCode { get; set; }

    [Required(ErrorMessage = "Country is required.")]
    public string Country { get; set; }

    [Required(ErrorMessage = "Phone is required.")]
    public string TelephoneNumber { get; set; }
}