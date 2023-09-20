using MangaSrbija.DAL.Entities.Chapter;
using MangaSrbija.DAL.Entities.MangaChapter;

namespace MangaSrbija.DAL.Repositories.chapters
{
    public interface IChapterRepository
    {
        public Chapter GetById(int id);
        public List<Chapter> GetAllByMangaId(int mangaId);
        public List<MangaChapter> GetRecentlyUpdated(int page, int perPage);
        public int Save(Chapter category);
        public void Delete(int id);
        public Chapter Update(Chapter chapter);
        List<int> GetAll();
    }
}
