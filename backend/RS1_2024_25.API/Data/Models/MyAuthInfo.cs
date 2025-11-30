namespace RS1_2024_25.API.Data.Models
{
    public class MyAuthInfo
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsCustomer { get; set; }
        public bool IsManager { get; set; }
        public bool IsLoggedIn { get; set; }
       
    }
}
