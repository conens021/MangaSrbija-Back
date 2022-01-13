using MangaSrbija.BLL.exceptions;
using MangaSrbija.BLL.mappers.Chapters;
using MangaSrbija.DAL.Entities.Chapter;
using MangaSrbija.DAL.Repositories.chapters;

namespace MangaSrbija.BLL.services.MangaServices
{
    public class ChapterService
    {
        private readonly IChapterRepository _chapterRepository;

        public ChapterService(IChapterRepository chapterRepository, MangaService mangService)
        {
            _chapterRepository = chapterRepository;
        }

        public ChapterSingle GetById(int id)
        {

            Chapter category = _chapterRepository.GetById(id);

            if (category.Id == 0) throw new BusinessException("Chapter not found!", 404);


            return new ChapterSingle(category);
        }

        public List<ChapterSingle> GetAll(int id)
        {
            return _chapterRepository.GetAllByMangaId(id).Select(ch => new ChapterSingle(ch)).ToList();
        }

        public ChapterSingle CreateChapter(CreateChapterDTO createChapterDto)
        {

            int id = _chapterRepository.Save(createChapterDto.ToChapter());


            return new ChapterSingle(createChapterDto, id);
        }

        public ChapterSingle UpdateChapter(UpdateChapterDTO updateChapterDTO)
        {
            GetById(updateChapterDTO.Id);

            Chapter chapter = _chapterRepository.Update(updateChapterDTO.ToChapter());

            chapter.UpdatedAt = DateTime.Now;

            ChapterSingle cs = new ChapterSingle(chapter);

            return cs;
        }

        public ChapterSingle DeleteChapter(int id)
        {
            ChapterSingle chapterSingle = GetById(id);

            _chapterRepository.Delete(id);

            return chapterSingle;
        }
    }
}
