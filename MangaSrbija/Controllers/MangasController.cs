using MangaSrbija.BLL.mappers;
using MangaSrbija.BLL.mappers.Categories;
using MangaSrbija.BLL.services.MangaServices;
using Microsoft.AspNetCore.Mvc;


namespace MangaSrbija.Presentation.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MangasController : ControllerBase
    {

        private readonly MangaService _mangaSerivce;
        private readonly MangasCategoriesService _mangasCategoriesSerivce;


        public MangasController(MangaService mangaService, MangasCategoriesService mangasCategoriesSerivce)
        {
            _mangaSerivce = mangaService;
            _mangasCategoriesSerivce = mangasCategoriesSerivce;
        }


        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
           
            MangaSingle manga = _mangaSerivce.GetById(id);


            return Ok(manga);
        }

        [HttpGet]
        public ActionResult GetRecentlyUpdated()
        {
            
            List<MangaSingle> recentlyUpdated = _mangaSerivce.GetRecentlyUpdated(20);


            return Ok(recentlyUpdated);
        }

        [HttpGet("{id}/categories")]
        public ActionResult GetMangaCategories(int id)
        {

            List<CategorySingle> categories = _mangasCategoriesSerivce.GetMangaCategories(id);


            return Ok(categories);
        }

        [HttpPost("{id}/categories")]
        public ActionResult SetMangasCategories(int id,[FromBody]List<CategorySingle> mangaCategories)
        {

            MangaSingle mangaSingle = _mangasCategoriesSerivce.SetMangaCategories(mangaCategories,id);


            return Ok(mangaSingle);
        }

        [HttpDelete("{id}/categories")]
        public ActionResult SetMangasCategories(int id, [FromBody] CategorySingle mangaCategories)
        {

            MangaSingle mangaSingle = _mangasCategoriesSerivce.DeleleMangaCategory(mangaCategories, id);


            return Ok(mangaSingle);
        }

        [HttpPost]
        public async Task<ActionResult> CreateManga([FromBody] CreateMangaDTO createMangaDTO)
        {

            MangaSingle mangaSingle = await _mangaSerivce.CreateManga(createMangaDTO);


            return Created($"/mangas/{mangaSingle.Id}", mangaSingle);
        }

        [HttpPost("cover")]
        public async Task<ActionResult> UploadCoverPhoto([FromBody] CreateMangaCoverDTO mangaCoverDTO)
        {

            MangaSingle mangaSingle = await _mangaSerivce.UploadMangaCover(mangaCoverDTO);


            return Created($"/mangas/{mangaSingle.Id}", mangaSingle);

        }

        [HttpPatch]
        public ActionResult UpdateManga([FromBody] UpdateMangaDTO updateMangaDTO)
        {
            MangaSingle mangaSingle = _mangaSerivce.UpdateManga(updateMangaDTO);


            return Ok(mangaSingle);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {

            MangaSingle mangaSingle = _mangaSerivce.DeleteManga(id);


            return Ok(mangaSingle);
        }
    }
}
