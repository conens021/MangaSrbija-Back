using MangaSrbija.BLL.mappers.Mangas;
using MangaSrbija.DAL.Entities.EManga;

namespace MangaSrbija.BLL.mappers.Mangas
{
    public class UpdateMangaDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Summary { get; set; } = "";
        public string Type { get; set; } = "";

        public Manga ToManga()
        {
            Manga manga = new Manga();
            manga.Title = Title;
            manga.Summary = Summary;
            manga.Type = Type;
            manga.Id = Id;


            return manga;
        }

        public MangaSingle ToMangaSingle(MangaSingle mangaSingle)
        {

            mangaSingle.Title = Title;
            mangaSingle.Summary = Summary;
            mangaSingle.Type = Type;


            return mangaSingle;
        }
    }
}
