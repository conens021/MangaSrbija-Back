using MangaSrbija.BLL.mappers;
using MangaSrbija.BLL.mappers.Categories;
using MangaSrbija.BLL.services.MangaServices;
using Microsoft.AspNetCore.Mvc;

namespace MangaSrbija.Presentation.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {

        private readonly CategoryService _categoryService;
        private readonly MangasCategoriesService _mangasCategoriesSerivce;

        public CategoriesController(CategoryService categoryService, MangasCategoriesService mangasCategoriesSerivce)
        {
            _categoryService = categoryService;
            _mangasCategoriesSerivce = mangasCategoriesSerivce;
        }


        [HttpGet]
        public ActionResult Get()
        {


            return Ok(_categoryService.GetAll());
        }

        [HttpGet("{name}")]
        public ActionResult GetMangasPageByCategoryName(string name, [FromQuery] int page = 1, [FromQuery] int size = 20)
        {

            CategoryMangasDTO mangas = _categoryService.GetCategoryMangas(name, page, size);


            return Ok(mangas);
        }

        [HttpPost]
        public ActionResult Post([FromBody] CreateCategoryDTO createCategory)
        {

            CategorySingle categorySingle = _categoryService.Create(createCategory);


            return Ok(categorySingle);
        }

        [HttpPatch]
        public ActionResult Update([FromBody] CategorySingle categorySingle)
        {

            _categoryService.ChangeCategoryName(categorySingle);


            return Ok(categorySingle);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {

            List<MangaSingle> categorySingle = _mangasCategoriesSerivce.DeleteCategory(id);


            return Ok(categorySingle);
        }
    }
}
