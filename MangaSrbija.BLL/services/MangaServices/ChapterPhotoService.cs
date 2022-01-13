using MangaSrbija.BLL.helpers;
using MangaSrbija.BLL.mappers;
using MangaSrbija.BLL.mappers.Chapters;
using MangaSrbija.BLL.mappers.Chapters.Photos;
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

        public async Task<List<ChapterPhotoPath>> UploadChapterPhotos(UploadChapterPhotosDTO uploadChapterPhotosDTO, int chapterId)
        {

            List<ChapterPhoto> chapterPhotos = new List<ChapterPhoto>();

            string folder = GetCoverPhotosFolder(chapterId, true);

            List<string> photosFiles = uploadChapterPhotosDTO.Photos;

            foreach (string file in photosFiles)
            {
                ChapterPhoto cp = new ChapterPhoto();

                cp.Path = await FileHandler.Save(file, folder);
                cp.ChapterId = chapterId;
                cp.CreatedAt = DateTime.Now;
                cp.UpdatedAt = DateTime.Now;
                chapterPhotos.Add(cp);
            }

            _chapterPhotoRepository.SaveAll(chapterPhotos);


            return chapterPhotos.Select(cp => new ChapterPhotoPath(cp)).ToList();
        }

        public List<ChapterPhotoSingle> GetChapterPhotos(int chapterId)
        {

            IEnumerable<ChapterPhoto> chapterPhotos = _chapterPhotoRepository.GetPhotosByChapter(chapterId);


            return chapterPhotos.Select(cp => new ChapterPhotoSingle(cp)).ToList();
        }

        public void DeleteChapterPhotos(int chapterId, List<DeleteChapterPhotoDTO> deleteChapterPhotosDTO)
        {

            foreach (DeleteChapterPhotoDTO photo in deleteChapterPhotosDTO)
            {
                FileHandler.Delete(photo.Path);
            }

            _chapterPhotoRepository.DeleteAll(deleteChapterPhotosDTO.Select(cp => cp.Id).ToList());
        }

        private string GetCoverPhotosFolder(int chapterId, bool updateManga)
        {

            DAL.Entities.Chapter.Chapter chapter = _chapterRepository.GetById(chapterId);

            Manga manga = _mangaRepository.GetById(chapter.MangaId);

            string folder = Path.Combine("chapters", manga.Title, chapter.Name);

            if (updateManga)
            {
                manga.UpdatedAt = DateTime.Now;

                _mangaRepository.UpdateManga(manga);
            }


            return folder;
        }

    }
}
