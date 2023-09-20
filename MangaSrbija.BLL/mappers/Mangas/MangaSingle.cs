
using MangaSrbija.DAL.Entities.EManga;

namespace MangaSrbija.BLL.mappers.Mangas
{
    public class MangaSingle
    {

        public MangaSingle() { }

        public MangaSingle(Manga manga)
        {
            Id = manga.Id;
            Title = manga.Title;
            Summary = manga.Summary;
            CoverPhoto = manga.CoverPhoto;
            Type = manga.Type;
            Views = manga.Views;
            Rating = manga.Rating;
            Categories = manga.Categories;
            CreatedAt = manga.CreatedAt;
            UpdatedAt = manga.UpdatedAt;
        }

        public Manga ToManga()
        {
            Manga manga = new Manga();
            manga.Id = Id;
            manga.Title = Title;
            manga.Summary = Summary;
            manga.CoverPhoto = CoverPhoto;
            manga.Type = Type;
            manga.Views = Views;
            manga.Rating = Rating;
            manga.Categories = Categories;
            manga.UpdatedAt = UpdatedAt;
            manga.CreatedAt = CreatedAt;

            return manga;
        }

        public UpdateMangaDTO ToUpdateMangaDTO() 
        {
            UpdateMangaDTO uMangaDTO = new UpdateMangaDTO();
            uMangaDTO.Id = Id;
            uMangaDTO.Title = Title;
            uMangaDTO.Summary = Summary;
            uMangaDTO.Type = Type;


            return uMangaDTO;
        }

        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Summary { get; set; } = "";
        public string CoverPhoto { get; set; } = "";
        public string Type { get; set; } = "";
        public int Views { get; set; } = 0;
        public double Rating { get; set; } = 0.00;
        public string Categories { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
