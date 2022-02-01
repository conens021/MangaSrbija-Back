using MangaSrbija.BLL.helpers;
using MangaSrbija.BLL.mappers.Categories;
using MangaSrbija.BLL.mappers.Mangas;
using MangaSrbija.BLL.services.MangaServices;
using MangaSrbija.Presentation.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MangaSrbija.Presentation.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {

        private readonly CategoryService _categoryService;
        private readonly MangasCategoriesService _mangasCategoriesSerivce;
        private readonly JwtAuthenticationManager _jwtAuth;

        public CategoriesController(CategoryService categoryService,
            MangasCategoriesService mangasCategoriesSerivce,
            JwtAuthenticationManager jwtAuthenticationManager)
        {
            _categoryService = categoryService;
            _mangasCategoriesSerivce = mangasCategoriesSerivce;
            _jwtAuth = jwtAuthenticationManager;
        }


        [HttpGet]
        public ActionResult Get()
        {


            return Ok(_categoryService.GetAll());
        }

        [HttpGet("{name}")]
        public ActionResult GetMangasPageByCategoryName(string name, [FromQuery] int page = 1, [FromQuery] int size = 20,[FromQuery] string ob = "az")
        {

            CategoryMangasDTO mangas = _categoryService.GetCategoryMangas(name, page, size, ob);


            return Ok(mangas);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Post([FromBody] CreateCategoryDTO createCategory)
        {


            CategorySingle categorySingle = _categoryService.Create(createCategory,_jwtAuth.GetBearerUser());


            return Ok(categorySingle);
        }

        [HttpPatch]
        public ActionResult Update([FromBody] CategorySingle categorySingle)
        {

            _categoryService.ChangeCategoryName(categorySingle, _jwtAuth.GetBearerUser());


            return Ok(categorySingle);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {

            List<MangaSingle> categorySingle = _mangasCategoriesSerivce.DeleteCategory(id, _jwtAuth.GetBearerUser());


            return Ok(categorySingle);
        }
    }
}
