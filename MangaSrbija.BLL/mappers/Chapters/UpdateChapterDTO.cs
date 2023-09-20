using MangaSrbija.DAL.Entities.Chapter;

namespace MangaSrbija.BLL.mappers.Chapters
{
    public class UpdateChapterDTO
    {


        public void ToChapterSingle(ChapterSingle cs)
        {
            cs.UpdatedAt = DateTime.Now;
            cs.isPrime = isPrime;
            cs.Name = Name;
        }

        public Chapter ToChapter(int id)
        {

            Chapter chapter = new Chapter();
            chapter.Name = Name;
            chapter.isPrime = isPrime;
            chapter.Id = id;

            return chapter;
        }

        public string Name { get; set; } = string.Empty;
        public bool isPrime { get; set; } = true;

    }
}
