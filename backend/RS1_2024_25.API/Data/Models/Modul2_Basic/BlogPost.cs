namespace RS1_2024_25.API.Data.Models.Modul2_Basic
{
    public class BlogPost
    {
        public int Id { get; set; }  
        public string Title { get; set; }  
        public string Content { get; set; }
        public byte[]? Image { get; set; }
        public DateTime? PublishedDate { get; set; }  
        public string? Author { get; set; }  //zamijeniti sa id zaposlenika
        public bool IsPublished { get; set; }  
        public bool Active { get; set; }
    }
}
