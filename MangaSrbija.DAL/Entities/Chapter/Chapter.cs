
namespace MangaSrbija.DAL.Entities.Chapter
{
    public class Chapter
    {
        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public int MangaId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
