
namespace MangaSrbija.DAL.Entities.EManga
{
    public class Chapter
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int MangaId { get; set; }
        public Manga? Manga { get; set; }

    }
}
