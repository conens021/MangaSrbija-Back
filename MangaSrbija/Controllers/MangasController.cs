using MangaSrbija.BLL.mappers.Mangas;
using MangaSrbija.BLL.mappers.Categories;
using MangaSrbija.BLL.services.MangaServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MangaSrbija.BLL.mappers;
using MangaSrbija.BLL.exceptions;
using MangaSrbija.BLL.mappers.MangaChapter;
using MangaSrbija.Presentation.Attributes;

namespace MangaSrbija.Presentation.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MangasController : ControllerBase
    {

        private readonly MangaService _mangaSerivce;
        private readonly MangasCategoriesService _mangasCategoriesSerivce;
        private readonly JwtAuthenticationManager _jwtAuth;

        public MangasController(MangaService mangaService,
            MangasCategoriesService mangasCategoriesSerivce,
            JwtAuthenticationManager jwtAuth)
        {
            _mangaSerivce = mangaService;
            _mangasCategoriesSerivce = mangasCategoriesSerivce;
            _jwtAuth = jwtAuth;

        }

        [HttpGet]
        public ActionResult GetAll(
            [FromQuery] string orderBy = "az", 
            [FromQuery] int page = 1, [FromQuery] int perPage = 20)
        {

            List<MangaSingle> mangas = _mangaSerivce.GetAll(orderBy,page,perPage);

            return Ok(mangas);
        }

        [HttpGet("count")]
        public ActionResult CountMangas(int id)
        {
            int count = _mangaSerivce.CountAllMangas();

            return Ok(count);
        }

        [HttpGet("/mangas-ids")]
        public ActionResult GetAll()
        {
            List<int> ids = _mangaSerivce.GetAllIds();

            return Ok(ids);
        }


        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {

            MangaSingle manga = _mangaSerivce.GetById(id);


            return Ok(manga);
        }

        [HttpGet("search/{query}")]
        public ActionResult SearchManga(string query)
        { 
            List<MangaSingle> mangas = _mangaSerivce.SearchMangas(query);


            return Ok(mangas);
        }


        [HttpGet("letter/{letter}")]
        public ActionResult Get(string letter,[FromQuery]string orderBy = "az", [FromQuery]int page = 1,[FromQuery]int perPage = 20)
        {

            List<MangaSingle> mangas = _mangaSerivce.GetByLetter(letter,orderBy,page,perPage);

            return Ok(mangas);
        }

        [HttpGet("most-popular")]
        public ActionResult GetMostPopular([FromQuery] int perPage = 20, [FromQuery] int page = 1)
        {

            List<MangaSingle> recentlyUpdated = _mangaSerivce.GetMostPopular(page,perPage);


            return Ok(recentlyUpdated);
        }

        [HttpGet("{id}/categories")]
        public ActionResult GetMangaCategories(int id)
        {

            List<CategorySingle> categories = _mangasCategoriesSerivce.GetMangaCategories(id);


            return Ok(categories);
        }

        [HttpPost("{id}/categories")]
        public ActionResult SetMangasCategories(int id, [FromBody] List<CategorySingle> mangaCategories)
        {

            MangaSingle mangaSingle = _mangasCategoriesSerivce.SetMangaCategories(mangaCategories, id, _jwtAuth.GetBearerUser());


            return Ok(mangaSingle);
        }

        [HttpDelete("{id}/categories")]
        public ActionResult SetMangasCategories(int id, [FromBody] CategorySingle mangaCategories)
        {

            MangaSingle mangaSingle = _mangasCategoriesSerivce.DeleleMangaCategory(mangaCategories, id, _jwtAuth.GetBearerUser());


            return Ok(mangaSingle);
        }

        [HttpPost]
        public async Task<ActionResult> CreateManga([FromBody] CreateMangaDTO createMangaDTO)
        {
            try
            {

                MangaSingle mangaSingle = await _mangaSerivce.CreateManga(createMangaDTO, _jwtAuth.GetBearerUser());


                return Created($"/mangas/{mangaSingle.Id}", mangaSingle);
            }
            catch (Exception e)
            {
                throw new BusinessException("Error occurred while trying to save file:\n" + e.Message, 500);
            }
        }

        [HttpPost("cover")]
        public async Task<ActionResult> UploadCoverPhoto([FromBody] CreateMangaCoverDTO mangaCoverDTO)
        {

            MangaSingle mangaSingle = await _mangaSerivce.UploadMangaCover(mangaCoverDTO, _jwtAuth.GetBearerUser());


            return Created($"/mangas/{mangaSingle.Id}", mangaSingle);

        }

        [HttpPatch]
        public ActionResult UpdateManga([FromBody] UpdateMangaDTO updateMangaDTO)
        {
            MangaSingle mangaSingle = _mangaSerivce.UpdateManga(updateMangaDTO, _jwtAuth.GetBearerUser());


            return Ok(mangaSingle);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {

            MangaSingle mangaSingle = _mangaSerivce.DeleteManga(id, _jwtAuth.GetBearerUser());


            return Ok(mangaSingle);
        }

        [HttpPost("{mangaId}/rate")]
        public ActionResult RateManga(int mangaId, [FromBody] MangaRatingDTO mangaRatingDTO)
        {

            double mangaRating = _mangaSerivce.RateManga(mangaId, _jwtAuth.GetBearerUser(), mangaRatingDTO);

            return Ok(mangaRating);
        }
    }
}
