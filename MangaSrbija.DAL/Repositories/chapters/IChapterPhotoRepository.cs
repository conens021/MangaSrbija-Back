using MangaSrbija.DAL.Entities.Chapter;

namespace MangaSrbija.DAL.Repositories.chapters
{
    public interface IChapterPhotoRepository
    {
        public List<ChapterPhoto> GetAllByChapterId(int mangaId);
        public int Save(ChapterPhoto chapterPhotos);
        public IEnumerable<ChapterPhoto> SaveAll(IEnumerable<ChapterPhoto> chapterPhoto);
        public void Delete(int id);
        public IEnumerable<ChapterPhoto> GetPhotosByChapter(int chapterId);
        public void DeleteAll(List<int> chapterPhotos);
    }
}
