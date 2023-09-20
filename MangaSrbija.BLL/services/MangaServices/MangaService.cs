using MangaSrbija.BLL.mappers.Mangas;
using MangaSrbija.DAL.Repositories;
using MangaSrbija.DAL.Entities.EManga;
using MangaSrbija.BLL.helpers;
using MangaSrbija.BLL.exceptions;
using MangaSrbija.BLL.mappers.UserAuth;

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

        public async Task<MangaSingle> CreateManga(CreateMangaDTO createMangaDTO, UserAuthorize user)
        {

            CheckMangaPolicy(user);

            String coverPhotoPath = "";

            if (!string.IsNullOrEmpty(createMangaDTO.CoverPhotoFile))
            {
                coverPhotoPath = await FileHandler.Save(createMangaDTO.CoverPhotoFile, "manga-covers");
            }

            int id = _mangaRepository.SaveManga(createMangaDTO.ToManga(coverPhotoPath));

            MangaSingle mangaSingle = createMangaDTO.ToMangaSingle(coverPhotoPath);

            mangaSingle.Id = id;


            return mangaSingle;
        }

        public List<int> GetAllIds()
        {

            List<int> ids = _mangaRepository.GetAllIds();


            return ids;
        }

        public List<MangaSingle> GetAll(string orderBy, int page, int perPage)
        {

            string orderByValue = OrderBy.Manga(orderBy);

            List<MangaSingle> mangas = _mangaRepository.GetAll(orderByValue, page, perPage)
                .Select(m => new MangaSingle(m)).ToList();


            return mangas;
        }

        public int CountAllMangas()
        {
            int count = _mangaRepository.CountAll();


            return count;
        }

        public List<MangaSingle> SearchMangas(string query)
        {
            List<Manga> mangas = _mangaRepository.GetByNameOrSummary(query);


            return mangas.Select(m => new MangaSingle(m)).ToList();
        }

        public List<MangaSingle> GetByLetter(string letter, string orderBy, int page, int perPage)
        {

            string orderByValue = OrderBy.Manga(orderBy);

            List<MangaSingle> mangas = new List<MangaSingle>();

            if (letter.Equals("all"))
            {
                mangas = _mangaRepository.GetAll(orderByValue, page, perPage)
                    .Select(m => new MangaSingle(m)).ToList();
                return mangas;
            }

            mangas = _mangaRepository.GetByLetter(letter, orderByValue, page, perPage)
                 .Select(m => new MangaSingle(m)).ToList();


            return mangas;
        }

        public async Task<MangaSingle> UploadMangaCover(CreateMangaCoverDTO mangaCoverDTO, UserAuthorize user)
        {

            CheckMangaPolicy(user);


            MangaSingle mangaSingle = GetById(mangaCoverDTO.MangaId);

            String coverPhotoPath = "";

            if (!string.IsNullOrEmpty(mangaCoverDTO.MangaCoverFile))
            {
                coverPhotoPath = await FileHandler.Save(mangaCoverDTO.MangaCoverFile, "manga-covers");
            }

            mangaSingle.CoverPhoto = coverPhotoPath;

            _mangaRepository.UpdateMangaCoverPhoto(coverPhotoPath, mangaCoverDTO.MangaId);


            return mangaSingle;
        }


        public MangaSingle DeleteManga(int id, UserAuthorize user)
        {

            CheckMangaPolicy(user);


            MangaSingle mangaSingle = GetById(id);

            try
            {

                FileHandler.Delete(mangaSingle.CoverPhoto);

                _mangaRepository.DeleteManga(id);

            }
            catch (Exception ex)
            {
                _mangaRepository.DeleteManga(id);
            }


            return mangaSingle;
        }


        public MangaSingle UpdateManga(UpdateMangaDTO updateMangaDTO, UserAuthorize user)
        {

            CheckMangaPolicy(user);


            MangaSingle mangaSingle = GetById(updateMangaDTO.Id);

            _mangaRepository.UpdateManga(updateMangaDTO.ToManga());


            return updateMangaDTO.ToMangaSingle(mangaSingle);
        }

        public List<MangaSingle> GetMostPopular(int page, int perPage)
        {
            List<MangaSingle> mangas = _mangaRepository.GetMostPopular(page, perPage).Select(m => new MangaSingle(m)).ToList();


            return mangas;
        }

        public double RateManga(int mangaId, UserAuthorize user, MangaRatingDTO mangaRatingDTO)
        {

            try
            {

                int userId = Convert.ToInt32(user.UserId);


                return _mangaRepository.RateManga(mangaId, userId, mangaRatingDTO.Rating);
            }
            catch (Exception e)
            {
                throw new BusinessException(e.Message, 500);
            }

        }

        private void CheckMangaPolicy(UserAuthorize user)
        {
            if (!(UserPolicy.isUserAdmin(user) || UserPolicy.isUserAuthor(user)))
            {
                throw new BusinessException("Only stuff members are able to access this api", 401);
            }
        }
    }
}
