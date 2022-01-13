using MangaSrbija.BLL.mappers;
using MangaSrbija.DAL.Repositories;
using MangaSrbija.DAL.Entities.EManga;
using MangaSrbija.BLL.helpers;
using MangaSrbija.BLL.exceptions;
using MangaSrbija.BLL.mappers.Categories;

namespace MangaSrbija.BLL.services.MangaServices
{
    public class MangaService
    {

        private readonly IMangaRepository _mangaRepository;

        public MangaService(IMangaRepository mangaRepository)
        {
            _mangaRepository = mangaRepository;
        }


        public MangaSingle GetById(int id)
        {

            Manga manga = _mangaRepository.GetById(id);

            if (manga.Id == 0) throw new BusinessException("Manga not found!", 404);


            return new MangaSingle(manga);
        }

        public async Task<MangaSingle> CreateManga(CreateMangaDTO createMangaDTO)
        {
            String coverPhotoPath = "";

            if (!string.IsNullOrEmpty(createMangaDTO.CoverPhotoFile))
            {
                coverPhotoPath = await FileHandler.Save(createMangaDTO.CoverPhotoFile,"manga-covers");
            }

            int id = _mangaRepository.SaveManga(createMangaDTO.ToManga(coverPhotoPath));

            MangaSingle mangaSingle = createMangaDTO.ToMangaSingle(coverPhotoPath);

            mangaSingle.Id = id;


            return mangaSingle;
        }


        public async Task<MangaSingle> UploadMangaCover(CreateMangaCoverDTO mangaCoverDTO)
        {

            MangaSingle mangaSingle = GetById(mangaCoverDTO.MangaId);

            String coverPhotoPath = "";

            if (!string.IsNullOrEmpty(mangaCoverDTO.MangaCoverFile))
            {
                coverPhotoPath = await FileHandler.Save(mangaCoverDTO.MangaCoverFile,"manga-covers");
            }

            mangaSingle.CoverPhoto = coverPhotoPath;

            _mangaRepository.UpdateMangaCoverPhoto(coverPhotoPath, mangaCoverDTO.MangaId);


            return mangaSingle;
        }

        public MangaSingle DeleteManga(int id)
        {

            MangaSingle mangaSingle = GetById(id);

            try
            {

                FileHandler.Delete(mangaSingle.CoverPhoto);

                _mangaRepository.DeleteManga(id);

            }
            catch (Exception ex)
            {
                throw new BusinessException("Cover photo of manga you trying to delete does not exists!", 404);
            }


            return mangaSingle;
        }


        public MangaSingle UpdateManga(UpdateMangaDTO updateMangaDTO)
        {
            MangaSingle mangaSingle = GetById(updateMangaDTO.Id);

            _mangaRepository.UpdateManga(updateMangaDTO.ToManga());


            return updateMangaDTO.ToMangaSingle(mangaSingle);
        }

        public List<MangaSingle> GetRecentlyUpdated(int perPage)
        {

            List<MangaSingle> mangas = _mangaRepository.GetRecentlyUpdated(perPage).Select(m => new MangaSingle(m)).ToList();


            return mangas;
        }

       
    }
}
