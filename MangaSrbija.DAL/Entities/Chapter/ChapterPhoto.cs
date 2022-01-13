namespace MangaSrbija.DAL.Entities.Chapter
{
    public class ChapterPhoto
    {
        public int Id { get; set; } 
        public string Path { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int ChapterId { get; set; }


    }
}
