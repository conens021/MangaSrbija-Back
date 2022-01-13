using MangaSrbija.DAL.Entities.Chapter;

namespace MangaSrbija.BLL.mappers.Chapters
{
    public class ChapterSingle
    {

        public ChapterSingle(CreateChapterDTO createChapterDTO,int id) 
        {
            Id = id;
            Name = createChapterDTO.Name;
            MangaId = createChapterDTO.MangaId;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public ChapterSingle(Chapter chapter)
        { 
            Id = chapter.Id;
            Name = chapter.Name;
            MangaId = chapter.MangaId;
            CreatedAt = chapter.CreatedAt;
            UpdatedAt = chapter.UpdatedAt;
        }

        public Chapter ToChapter()
        { 
            Chapter chapter = new Chapter();
            chapter.Id = Id;
            chapter.Name = Name;
            chapter.MangaId = MangaId;
            chapter.CreatedAt = CreatedAt;
            chapter.UpdatedAt = UpdatedAt;

            return chapter;
        }

        public int Id { get; set; } = 0;
        public string Name { get; set; } = string.Empty;
        public int MangaId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
