using MangaSrbija.DAL.Entities.Category;
using MangaSrbija.DAL.Entities.EManga;

namespace MangaSrbija.DAL.Repositories
{
    public interface IMangaRepository
    {
        public Manga GetById(int id);
        public List<Manga> GetRecentlyUpdated(int perPage);
        public int SaveManga(Manga manga);
        public void DeleteManga(int id);
        public void UpdateManga(Manga manga);
        public void UpdateMangaCoverPhoto(string coverPhoto, int id);
        
        public IEnumerable<Manga> UpdateAll(IEnumerable<Manga> mangas);

    }
}
