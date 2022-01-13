
using MangaSrbija.BLL.mappers.Categories;
using MangaSrbija.DAL.Entities.Category;

namespace MangaSrbija.BLL.mappers
{
    public class SetMangaCategoriesDTO
    {
        public List<CategorySingle> Categories { get; set; } = new List<CategorySingle>();

        public List<Category> ToCategoryList()
        {
            return Categories.Select(cs => new Category() { Id = cs.Id, Name = cs.Name }).ToList();
        }
    }
}
