namespace MangaSrbija.DAL.Entities.EManga
{
    public class ChapterCover
    {
        public int Id { get; set; }
        public string Path { get; set; } = string.Empty;
        public int ChapterId { get; set; }
        public Chapter? Chapter { get; set; }
    }
}
