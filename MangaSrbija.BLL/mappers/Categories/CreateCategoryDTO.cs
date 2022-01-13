using MangaSrbija.DAL.Entities.Category;

namespace MangaSrbija.BLL.mappers.Categories
{
    public class CreateCategoryDTO
    {

        public Category ToCategory() {
            Category category = new Category();
            category.Name = Name;

            return category;
        }

        public CategorySingle ToCategorySingle(int id)
        {
            CategorySingle category = new CategorySingle();
            category.Name = Name;
            category.Id = id;

            return category;
        }

        public string Name { get; set; } = string.Empty;
    }
}
