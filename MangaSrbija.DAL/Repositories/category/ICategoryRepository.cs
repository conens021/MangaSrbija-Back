using MangaSrbija.DAL.Entities.Category;
using MangaSrbija.DAL.Entities.EManga;

namespace MangaSrbija.DAL.Repositories.category
{
    public interface ICategoryRepository
    {
        public Category GetById(int id);
        public List<Category> GetAll();
        public int SaveCategory(Category category);
        public void DeleteCategory(int id);
        public void UpdateCategory(Category category);
        Category GetByName(string name);
        Dictionary<Category, List<Manga>> GetMangasByCategoryName(string name, int page, int size,string orderBy);
    }
}
