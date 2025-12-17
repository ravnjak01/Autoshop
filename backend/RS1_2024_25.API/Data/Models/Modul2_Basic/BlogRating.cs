namespace RS1_2024_25.API.Data.Models.Modul2_Basic
{
    public class BlogRating
    {
        public int Id { get; set; }
        public int BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; }
        public string? UserId { get; set; }
        public User? User { get; set; }
        public int Rating { get; set; } 
        public DateTime CreatedAt { get; set; }
    }
}
