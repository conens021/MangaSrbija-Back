using MangaSrbija.BLL.exceptions;
using MangaSrbija.BLL.helpers;
using MangaSrbija.BLL.mappers;
using MangaSrbija.BLL.mappers.Chapters;
using MangaSrbija.BLL.mappers.Chapters.Photos;
using MangaSrbija.BLL.mappers.UserAuth;
using MangaSrbija.DAL.Entities.Chapter;
using MangaSrbija.DAL.Entities.EManga;
using MangaSrbija.DAL.Repositories;
using MangaSrbija.DAL.Repositories.chapters;

namespace MangaSrbija.BLL.services.MangaServices
{
    public class ChapterPhotoService
    {

        private readonly IChapterPhotoRepository _chapterPhotoRepository;
        private readonly IChapterRepository _chapterRepository;
        private readonly IMangaRepository _mangaRepository;


        public ChapterPhotoService(IChapterPhotoRepository chapterPhotoRepository,
                                    IChapterRepository chapterRepository,
                                    IMangaRepository mangaRepository)
        {
            _chapterPhotoRepository = chapterPhotoRepository;
            _chapterRepository = chapterRepository;
            _mangaRepository = mangaRepository;
        }

        public async Task<List<ChapterPhotoPath>> UploadChapterPhotos(
            UploadChapterPhotosDTO uploadChapterPhotosDTO,
            int chapterId,
            UserAuthorize user)
        {

            CheckCategoryPolicy(user);

            List<ChapterPhoto> chapterPhotos = new List<ChapterPhoto>();

            //Get chapter folder and update manga
            string folder = GetCoverPhotosFolder(chapterId, true);

            List<string> photosFiles = uploadChapterPhotosDTO.Photos;

            foreach (string file in photosFiles)
            {
                ChapterPhoto cp = new ChapterPhoto();

                byte[] data = FileHandler.GetFileBytes(file);

                int height = ImageHandler.GetHeight(data);
                int width = ImageHandler.GetWidth(data);

                cp.Path = await FileHandler.WriteBytes(data,file, folder);
                cp.Height = height;
                cp.Width = width;
                cp.ChapterId = chapterId;
                cp.CreatedAt = DateTime.Now;
                cp.UpdatedAt = DateTime.Now;
                chapterPhotos.Add(cp);
            }

            try
            {
                _chapterPhotoRepository.SaveAll(chapterPhotos);
            }
            catch (Exception e)
            {
                foreach (ChapterPhoto photo in chapterPhotos)
                {

                    FileHandler.Delete(photo.Path);

                    throw new BusinessException(e.Message, 500);
                }
            }


            return chapterPhotos.Select(cp => new ChapterPhotoPath(cp)).ToList();
        }

        public List<ChapterPhotoSingle> GetChapterPhotos(int chapterId)
        {

            IEnumerable<ChapterPhoto> chapterPhotos = _chapterPhotoRepository.GetPhotosByChapter(chapterId);


            return chapterPhotos.Select(cp => new ChapterPhotoSingle(cp)).ToList();
        }

        public void DeleteChapterPhotos(int chapterId, List<DeleteChapterPhotoDTO> deleteChapterPhotosDTO,UserAuthorize user)
        {

            CheckCategoryPolicy(user);

            foreach (DeleteChapterPhotoDTO photo in deleteChapterPhotosDTO)
            {
                FileHandler.Delete(photo.Path);
            }

            _chapterPhotoRepository.DeleteAll(deleteChapterPhotosDTO.Select(cp => cp.Id).ToList());
        }

        private string GetCoverPhotosFolder(int chapterId, bool updateManga)
        {

            DAL.Entities.Chapter.Chapter chapter = _chapterRepository.GetById(chapterId);

            if (chapter.Id == 0) throw new BusinessException("Chapter does not exists!", 404);

            Manga manga = _mangaRepository.GetById(chapter.MangaId);

            string folder = Path.Combine("chapters", manga.Title, chapter.Name);

            if (updateManga)
            {
                manga.LastChapterRd = DateTime.Now;

                _mangaRepository.UpdateMangaChapterRD(manga);
            }


            return folder;
        }

        private void CheckCategoryPolicy(UserAuthorize user)
        {
            if (!(UserPolicy.isUserAdmin(user) || UserPolicy.isUserAuthor(user)))
            {
                throw new BusinessException("Only stuff members are able to access this api", 401);
            }
        }

    }
}
