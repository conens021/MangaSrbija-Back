using MangaSrbija.DAL.Entities.EManga;

namespace MangaSrbija.DAL.Repositories
{
    public interface IMangaRepository
    {
        public Manga GetById(int id);
        public List<Manga> GetMostPopular(int page,int perPage);
        public int SaveManga(Manga manga);
        public void DeleteManga(int id);
        public void UpdateManga(Manga manga);
        public void UpdateMangaCoverPhoto(string coverPhoto, int id);
        public IEnumerable<Manga> UpdateAll(IEnumerable<Manga> mangas);
        double RateManga(int mangaId, int userId, double rating);
        void UpdateMangaChapterRD(Manga manga);
        public int CountAll();
        public List<Manga> GetByLetter(string letter, string orderBy, int page, int perPage);
        public List<Manga> GetAll(string orderBy, int page, int perPage);
        public List<int> GetAllIds();
        List<Manga> GetByNameOrSummary(string query);
    }
}
