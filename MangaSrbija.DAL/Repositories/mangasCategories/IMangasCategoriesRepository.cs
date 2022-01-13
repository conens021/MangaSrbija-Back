using MangaSrbija.DAL.Entities.Category;
using MangaSrbija.DAL.Entities.EManga;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaSrbija.DAL.Repositories.mangasCategories
{
    public interface IMangasCategoriesRepository
    {
        public List<Category> GetMangaCategories(int mangaId);
        public void SetMangaCategories(List<Category>? categories, int mangaId);
        public void DeleteMangaCategorie(int categoryId, int mangaId);
        public IEnumerable<Manga> GetAllContatainingCategory(string categoryName);
    }
}
