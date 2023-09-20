using MangaSrbija.DAL.Entities.EManga;

namespace MangaSrbija.DAL.Repositories.mangaUserFavorites
{
    public interface IMangaUserFavorites
    {
        public Manga GetById(int mangaId,int userId);
        public List<Manga> GetAllByUser(int userId);
        public int Save(Manga category,int userId);
        public void Delete(int id, int userId);
    }
}
