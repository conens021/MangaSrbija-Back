using MangaSrbija.BLL.exceptions;
using MangaSrbija.BLL.helpers;
using MangaSrbija.BLL.mappers.Chapters;
using MangaSrbija.BLL.mappers.MangaChapter;
using MangaSrbija.BLL.mappers.UserAuth;
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

        public List<MangaChapterDTO> GetRecentlyUpdated(int page, int perPage)
        {

            List<MangaChapterDTO> mangas = _chapterRepository.GetRecentlyUpdated(page, perPage)
                .Select(mc => new MangaChapterDTO(mc.Manga, mc.Chapter)).ToList();


            return mangas;
        }

        public List<int> GetAllIds()
        {

            List<int> chaptersIds = _chapterRepository.GetAll();


            return chaptersIds;
        }

        public ChapterSingle CreateChapter(CreateChapterDTO createChapterDto,UserAuthorize user)
        {

            CheckChapterService(user);

            int id = _chapterRepository.Save(createChapterDto.ToChapter());


            return new ChapterSingle(createChapterDto, id);
        }

        public ChapterSingle UpdateChapter(int id, UpdateChapterDTO updateChapterDTO, UserAuthorize user)
        {

            CheckChapterService(user);

            ChapterSingle chapterSingle = GetById(id);

            _chapterRepository.Update(updateChapterDTO.ToChapter(id));

            updateChapterDTO.ToChapterSingle(chapterSingle);

            return chapterSingle;
        }

        public ChapterSingle DeleteChapter(int id, UserAuthorize user)
        {

            CheckChapterService(user);

            ChapterSingle chapterSingle = GetById(id);

            _chapterRepository.Delete(id);

            return chapterSingle;
        }

        private void CheckChapterService(UserAuthorize user)
        {
            if (!(UserPolicy.isUserAdmin(user) || UserPolicy.isUserAuthor(user)))
            {
                throw new BusinessException("Only stuff members are able to access this api", 401);
            }
        }
    }
}
