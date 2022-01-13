using MangaSrbija.DAL.Entities.EManga;
using Microsoft.AspNetCore.Http;

namespace MangaSrbija.BLL.mappers
{
    public class CreateMangaDTO
    {

        public Manga ToManga(String coverPhotoPath)
        {

            Manga manga = new Manga();
            manga.Title = Title;
            manga.Summary = Summary;
            manga.Type = Type;
            manga.CoverPhoto = coverPhotoPath;


            return manga;
        }

        public MangaSingle ToMangaSingle(String coverPhotoPath)
        {

            MangaSingle mangaSingle = new MangaSingle();

            mangaSingle.Title = Title;
            mangaSingle.Summary = Summary;
            mangaSingle.Type = Type;
            mangaSingle.CoverPhoto = coverPhotoPath;

            return mangaSingle;
        }

        public string CoverPhotoFile { get; set; } = String.Empty;
        public string Title { get; set; } = "";
        public string Summary { get; set; } = "";
        public string Type { get; set; } = "";
    }
}
