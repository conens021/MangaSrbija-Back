using MangaSrbija.DAL.Entities.Chapter;

namespace MangaSrbija.BLL.mappers.Chapters
{
    public class UpdateChapterDTO
    {

        public Chapter ToChapter()
        {
            Chapter chapter = new Chapter();
            chapter.Id = Id;
            chapter.Name = Name;
            chapter.MangaId = MangaId;

            return chapter;
        }

        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int MangaId { get; set; }

    }
}
