using MangaSrbija.BLL.mappers.Chapters;
using MangaSrbija.BLL.mappers.Chapters.Photos;
using MangaSrbija.BLL.services.MangaServices;
using Microsoft.AspNetCore.Mvc;

namespace MangaSrbija.Presentation.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ChaptersController : ControllerBase
    {

        private readonly ChapterService _chapterService;
        private readonly ChapterPhotoService _chapterPhotoService;

        public ChaptersController(ChapterService chapterService
                                , ChapterPhotoService chapterPhotoService)
        {
            _chapterService = chapterService;
            _chapterPhotoService = chapterPhotoService;
        }


        [HttpGet("manga/{id}")]
        public ActionResult GetAllByManga(int id)
        {
            return Ok(_chapterService.GetAll(id));
        }

        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            return Ok(_chapterService.GetById(id));
        }

        [HttpPost]
        public ActionResult Post([FromBody] CreateChapterDTO createChapterDTO)
        {
            return Ok(_chapterService.CreateChapter(createChapterDTO));
        }

        [HttpPatch]
        public ActionResult Put([FromBody] UpdateChapterDTO updateChapterDTO)
        {

            ChapterSingle chapterSingle = _chapterService.UpdateChapter(updateChapterDTO);


            return Ok(chapterSingle);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {

            return Ok(_chapterService.DeleteChapter(id));

        }

        [HttpGet("{chapterId}/photos")]
        public ActionResult GetChapterPhotos(int chapterId)
        {

            List<ChapterPhotoSingle> chapterPhotos = _chapterPhotoService.GetChapterPhotos(chapterId);

            return Ok(chapterPhotos);
        }

        [HttpPost("{chapterId}/photos")]
        public async Task<ActionResult> PostChapterPhotos(int chapterId, [FromBody] UploadChapterPhotosDTO uploadChapterPhotosDTO)
        {

            List<ChapterPhotoPath> chapterPhotos
                = await _chapterPhotoService.UploadChapterPhotos(uploadChapterPhotosDTO, chapterId);

            return Ok(chapterPhotos);
        }

        [HttpDelete("{chapterId}/photos")]
        public ActionResult DeleteChapterPhotos(int chapterId, [FromBody] List<DeleteChapterPhotoDTO> deleteChapterPhotosDTO)

        {

            _chapterPhotoService.DeleteChapterPhotos(chapterId, deleteChapterPhotosDTO);


            return Ok();
        }

    }
}
