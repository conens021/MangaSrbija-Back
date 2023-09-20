using MangaSrbija.DAL.Entities.Chapter;

namespace MangaSrbija.DAL.Repositories.chapters
{
    public interface IChapterPhotoRepository
    {
        public IEnumerable<ChapterPhoto> SaveAll(IEnumerable<ChapterPhoto> chapterPhoto);
        public IEnumerable<ChapterPhoto> GetPhotosByChapter(int chapterId);
        public void DeleteAll(List<int> chapterPhotos);
    }
}
