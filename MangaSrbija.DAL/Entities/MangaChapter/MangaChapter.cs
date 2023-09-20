
using MangaSrbija.DAL.Entities.EManga;

namespace MangaSrbija.DAL.Entities.MangaChapter
{
    public class MangaChapter
    {

        public Manga Manga { get; set; } = new Manga();
        public DAL.Entities.Chapter.Chapter Chapter { get; set; } = new DAL.Entities.Chapter.Chapter();
    } 
}
