using MangaSrbija.BLL.exceptions;
using MangaSrbija.BLL.helpers;
using MangaSrbija.BLL.mappers.Categories;
using MangaSrbija.BLL.mappers.Mangas;
using MangaSrbija.BLL.mappers.UserAuth;
using MangaSrbija.DAL.Entities.Category;
using MangaSrbija.DAL.Entities.EManga;
using MangaSrbija.DAL.Repositories.category;

namespace MangaSrbija.BLL.services.MangaServices
{
    public class CategoryService
    {

        private readonly ICategoryRepository _categoryRepository;

        private readonly MangaService _mangService;

        public CategoryService(ICategoryRepository categoryRepository, MangaService mangService)
        {
            _categoryRepository = categoryRepository;
            _mangService = mangService;
        }

        public CategorySingle GetById(int id)
        {

            Category category = _categoryRepository.GetById(id);

            if (category.Id == 0) throw new BusinessException("Category not found!", 404);


            return new CategorySingle(category);
        }

        public List<Category> GetAll()
        {
            return _categoryRepository.GetAll().ToList();
        }

        public CategorySingle GetCategoryByName(string name)
        {
            Category category = _categoryRepository.GetByName(name);

            if (category.Id == 0) throw new BusinessException("Category not found!", 404);


            return new CategorySingle(category);
        }

        public CategoryMangasDTO GetCategoryMangas(string name, int page, int size,string orderBy)
        {

            string orderByValue = OrderBy.Manga(orderBy);

            Dictionary<Category, List<Manga>> categoryMangas = _categoryRepository.GetMangasByCategoryName(name, page, size, orderByValue);

            if (categoryMangas.Count == 0)
            {

                CategorySingle categorySingle = GetCategoryByName(name);
                

                return new CategoryMangasDTO(categorySingle);
            }

            KeyValuePair<Category, List<Manga>> kv = categoryMangas.First();

            Category category = kv.Key;
            List<Manga> mangas = kv.Value;


            return new CategoryMangasDTO(new CategorySingle(category),
                                         mangas.Select(m => new MangaSingle(m)).ToList()
                                         );
        }

        public void Delete(CategorySingle category, UserAuthorize user)
        {

            CheckCategoryPolicy( user);

            _categoryRepository.DeleteCategory(category.ToCategory().Id);
        }

        public CategorySingle Create(CreateCategoryDTO createCategory,UserAuthorize user)
        {

            CheckCategoryPolicy(user);

            int id = _categoryRepository.SaveCategory(createCategory.ToCategory());


            return (createCategory.ToCategorySingle(id));
        }

        public CategorySingle ChangeCategoryName(CategorySingle categorySingle,UserAuthorize user)
        {

            CheckCategoryPolicy(user);

            //If category is not found GetById will throw error
            GetById(categorySingle.Id);

            _categoryRepository.UpdateCategory(categorySingle.ToCategory());


            return categorySingle;
        }

        private void CheckCategoryPolicy(UserAuthorize user)
        {
            if (!(UserPolicy.isUserAdmin(user) || UserPolicy.isUserAuthor(user)))
            {
                throw new BusinessException("Only stuff members are able to access this api", 401);
            }
        }

    }
}
