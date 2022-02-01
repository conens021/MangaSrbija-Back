using MangaSrbija.BLL.mappers.Chapters;
using MangaSrbija.BLL.mappers.Chapters.Photos;
using MangaSrbija.BLL.mappers.MangaChapter;
using MangaSrbija.BLL.services.MangaServices;
using MangaSrbija.Presentation.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MangaSrbija.Presentation.Controllers
{

    [Route("[controller]")]
    [ApiController]
    public class ChaptersController : ControllerBase
    {

        private readonly ChapterService _chapterService;
        private readonly ChapterPhotoService _chapterPhotoService;
        private readonly JwtAuthenticationManager _jwtAuth;

        public ChaptersController(ChapterService chapterService
                                , ChapterPhotoService chapterPhotoService,
                                JwtAuthenticationManager jwtAuth)
        {
            _chapterService = chapterService;
            _chapterPhotoService = chapterPhotoService;
            _jwtAuth = jwtAuth;
        }

        [HttpGet]
        public ActionResult GetAllIds()
        {
            return Ok(_chapterService.GetAllIds());
        }

        [HttpGet("manga/{id}")]
        public ActionResult GetAllByManga(int id)
        {


            return Ok(_chapterService.GetAll(id));
        }

        [HttpGet("new-releases")]
        public ActionResult GetRecentlyUpdated([FromQuery] int perPage = 20, [FromQuery] int page = 1)
        {

            List<MangaChapterDTO> recentlyUpdated = _chapterService.GetRecentlyUpdated(page, perPage);


            return Ok(recentlyUpdated);
        }

        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {


            return Ok(_chapterService.GetById(id));
        }

        [HttpPost]
        public ActionResult Post([FromBody] CreateChapterDTO createChapterDTO)
        {


            return Ok(_chapterService.CreateChapter(createChapterDTO, _jwtAuth.GetBearerUser()));
        }

        [HttpPatch("{id}")]
        public ActionResult Put(int id,[FromBody] UpdateChapterDTO updateChapterDTO)
        {

            ChapterSingle chapterSingle = _chapterService.UpdateChapter(id,updateChapterDTO, _jwtAuth.GetBearerUser());


            return Ok(chapterSingle);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {

            return Ok(_chapterService.DeleteChapter(id, _jwtAuth.GetBearerUser()));

        }

        [HttpGet("{chapterId}/photos")]
        public ActionResult GetChapterPhotos(int chapterId)
        {

            List<ChapterPhotoSingle> chapterPhotos = _chapterPhotoService.GetChapterPhotos(chapterId);


            return Ok(chapterPhotos);
        }

        [HttpGet("{chapterId}/photos/guest")]
        public ActionResult GetFreeChatperPhotos(int chapterId)
        {

            List<ChapterPhotoSingle> chapterPhotos = _chapterPhotoService.GetChapterPhotos(chapterId);


            return Ok(chapterPhotos);
        }

        [HttpPost("{chapterId}/photos")]
        public async Task<ActionResult> PostChapterPhotos(int chapterId, [FromBody] UploadChapterPhotosDTO uploadChapterPhotosDTO)
        {

            List<ChapterPhotoPath> chapterPhotos
                = await _chapterPhotoService.UploadChapterPhotos(uploadChapterPhotosDTO, chapterId,_jwtAuth.GetBearerUser());

            return Ok(chapterPhotos);
        }

        [HttpDelete("{chapterId}/photos")]
        public ActionResult DeleteChapterPhotos(int chapterId, [FromBody] List<DeleteChapterPhotoDTO> deleteChapterPhotosDTO)

        {

            _chapterPhotoService.DeleteChapterPhotos(chapterId, deleteChapterPhotosDTO, _jwtAuth.GetBearerUser());


            return Ok();
        }

    }
}
