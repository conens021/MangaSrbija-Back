namespace MangaSrbija.DAL.Entities.EManga
{
    public class Manga
    {
        public int Id { get; set; } = 0;
        public string Title { get; set; } = "";
        public string Summary { get; set; } = "";
        public string CoverPhoto { get; set; } = "";
        public string Type { get; set; } = "";
        public int Views { get; set; } = 0;
        public double Rating { get; set; } = 1.00;
        public string Categories { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime LastChapterRd { get; set; }

    }
}
