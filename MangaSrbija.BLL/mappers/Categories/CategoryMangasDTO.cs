namespace MangaSrbija.BLL.mappers.Categories
{
    public class CategoryMangasDTO
    {
        public CategoryMangasDTO(CategorySingle categorySingle)
        {
            Category = categorySingle;
            Mangas = new List<MangaSingle>();
        }
        public CategoryMangasDTO(CategorySingle categorySingle, List<MangaSingle> mangaList)
        {
            Category = categorySingle;
            Mangas = mangaList;
        }

        public CategorySingle? Category { get; set; }
        public List<MangaSingle>? Mangas { get; set; }
    }
}
