using MangaSrbija.DAL.Entities.Category;

namespace MangaSrbija.BLL.mappers.Categories
{
    public class CategorySingle
    {
        public CategorySingle() { }

        public CategorySingle(Category category) 
        {
            Id = category.Id;
            Name = category.Name;
        }

        public Category ToCategory() 
        {
            Category category = new Category();
            category.Id = Id;
            category.Name = Name;


            return category;
        }

        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
    }
}
