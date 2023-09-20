
namespace MangaSrbija.DAL.Entities.Chapter
{
    public class Chapter
    {

        public Chapter() { }

        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public int MangaId { get; set; }
        public bool isPrime { get; set; } = true;   
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
