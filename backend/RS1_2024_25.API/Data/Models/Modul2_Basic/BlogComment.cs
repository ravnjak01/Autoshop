namespace RS1_2024_25.API.Data.Models.Modul2_Basic
{
    public class BlogComment
    {
        public int Id { get; set; }
        public int BlogPostId { get; set; }
        public BlogPost BlogPost { get; set; }
        public int? UserId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } 
    }
}
