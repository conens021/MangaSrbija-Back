using MangaSrbija.DAL.Entities.Chapter;

namespace MangaSrbija.BLL.mappers.Chapters
{
    public class CreateChapterDTO
    {
        public string Name { get; set; } = string.Empty;
        public bool isPrime { get; set; } = true;
        public int MangaId { get; set; } = 0;


        public Chapter ToChapter()
        {

            Chapter chapter = new Chapter();
            chapter.Name = Name;
            chapter.MangaId = MangaId;
            chapter.isPrime = isPrime;


            return chapter;
        }

    }
}
