using MangaSrbija.DAL.Entities.Chapter;

namespace MangaSrbija.DAL.Repositories.chapters
{
    public interface IChapterRepository
    {
        public Chapter GetById(int id);
        public List<Chapter> GetAllByMangaId(int mangaId);
        public int Save(Chapter category);
        public void Delete(int id);
        public Chapter Update(Chapter category);
    }
}
