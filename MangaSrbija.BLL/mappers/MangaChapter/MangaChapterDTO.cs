
using MangaSrbija.DAL.Entities.EManga;
using MangaSrbija.DAL.Entities.Chapter;

namespace MangaSrbija.BLL.mappers.MangaChapter
{
    public class MangaChapterDTO
    {

        public MangaChapterDTO(Manga manga, Chapter chapter)
        {
            ChapterId = chapter.Id;
            ChapterName = chapter.Name;
            MangaId = manga.Id;
            MangaTitle = manga.Title;
            CoverPhoto = manga.CoverPhoto;
        }

        public int ChapterId { get; set; }
        public string ChapterName { get; set; } = string.Empty;
        public int MangaId { get; set; }
        public string MangaTitle { get; set; } = string.Empty;
        public string CoverPhoto { get; set; } = string.Empty;


    }
}
