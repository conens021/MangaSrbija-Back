using MangaSrbija.BLL.exceptions;
using MangaSrbija.BLL.mappers;
using MangaSrbija.BLL.mappers.Categories;
using MangaSrbija.DAL.Repositories;
using MangaSrbija.DAL.Repositories.mangasCategories;

namespace MangaSrbija.BLL.services.MangaServices
{
    public class MangasCategoriesService
    {

        private readonly IMangaRepository _mangaRepository;
        private readonly IMangasCategoriesRepository _mangasCategoriesRepository;
        private readonly MangaService _mangaService;
        private readonly CategoryService _categoryService;

        public MangasCategoriesService(IMangaRepository mangaRepository,MangaService mangaService,CategoryService categoryService,
                                        IMangasCategoriesRepository mangasCategoriesRepository)
        {
            _mangaRepository = mangaRepository;
            _mangaService = mangaService;
            _categoryService = categoryService;
            _mangasCategoriesRepository = mangasCategoriesRepository;
        }

        public MangaSingle SetMangaCategories(List<CategorySingle> mangaCategories, int mangaId)
        {

            MangaSingle mangaSingle = _mangaService.GetById(mangaId);
            List<string> categories = GetExistsingMangaCategories(mangaSingle);

            //get just new categores
            List<CategorySingle> newItems = mangaCategories.Where(mc => 
                !categories.Any(category => category.ToLower().Equals(mc.Name.ToLower()))).ToList();

            if (newItems.Count > 0)
            {
                foreach (CategorySingle newItem in newItems)
                {
                    categories.Add(newItem.Name);
                }

                string newMangaCategoriesString = string.Join(",", categories);

                mangaCategories = newItems;

                _mangasCategoriesRepository.SetMangaCategories(mangaCategories.Select(mcs => mcs.ToCategory()).ToList(), mangaId);

                mangaSingle.Categories = newMangaCategoriesString;

                _mangaRepository.UpdateManga(mangaSingle.ToManga());
            }


            return mangaSingle;
        }

        public MangaSingle DeleleMangaCategory(CategorySingle categorySingle, int mangaId)
        {

            MangaSingle mangaSingle = _mangaService.GetById(mangaId);

            List<string> categories = GetExistsingMangaCategories(mangaSingle);

            //filter out category from manga category list that we whant to delete
            //we get new list without that category
            List<string> newMangaCategoryList = categories.Where(c => !c.ToLower().Equals(categorySingle.Name.ToLower())).ToList();

            if (newMangaCategoryList.Count > 0)
            {
                mangaSingle.Categories = String.Join(",", newMangaCategoryList);
            }
            else
            {
                mangaSingle.Categories = String.Empty;
            }

            _mangaRepository.UpdateManga(mangaSingle.ToManga());

            _mangasCategoriesRepository.DeleteMangaCategorie(categorySingle.Id, mangaId);


            return mangaSingle;
        }

        public List<CategorySingle> GetMangaCategories(int id)
        {

            List<CategorySingle> categories =
                _mangasCategoriesRepository.GetMangaCategories(id).Select(c => new CategorySingle(c)).ToList();


            return categories;
        }

        public List<MangaSingle> GetAllMangasContainingCategory(string categoryName)
        {

            List<MangaSingle> mangas = _mangasCategoriesRepository.GetAllContatainingCategory(categoryName)
                .Select(m => new MangaSingle(m)).ToList();


            return mangas;
        }

        public List<MangaSingle> DeleteCategory(int id)
        {

            CategorySingle categorySingle = _categoryService.GetById(id);

            if (categorySingle == null) throw new BusinessException("Category Not Found!", 404);

            //get all mangas
            List<MangaSingle> mangas = GetAllMangasContainingCategory(categorySingle.Name);

            //loop through all mangas
            //and filter out category we whant to delete
            foreach (MangaSingle manga in mangas)
            {
                List<string> mangaCategoryStringList = manga.Categories.Split(',').ToList();

                List<string> filtered = mangaCategoryStringList.Where(c => !c.ToLower().Equals(categorySingle.Name.ToLower())).ToList();

                manga.Categories = string.Join(",", filtered);
            }

            //delete category
            _categoryService.Delete(categorySingle);
            //update manga list
            _mangaRepository.UpdateAll(mangas.Select(ms => ms.ToManga()).ToList());

            return mangas;
        }

        private List<string> GetExistsingMangaCategories(MangaSingle mangaSingle)
        {

            string[] categoriesArray = mangaSingle.Categories.Trim().Split(",");

            List<string> categories = new List<string>();

            foreach (string s in categoriesArray)
            {
                if (!string.IsNullOrEmpty(s))
                {
                    categories.Add(s);
                }

            }


            return categories;
        }
    }
}
