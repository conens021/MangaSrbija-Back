using MangaSrbija.BLL.exceptions;
using MangaSrbija.BLL.helpers;
using MangaSrbija.BLL.mappers.Mangas;
using MangaSrbija.BLL.mappers.UserAuth;
using MangaSrbija.BLL.mappers.Users;
using MangaSrbija.BLL.services.MangaServices;
using MangaSrbija.DAL.Entities.EManga;
using MangaSrbija.DAL.Entities.User;
using MangaSrbija.DAL.Repositories.mangaReadLater;
using MangaSrbija.DAL.Repositories.mangaUserFavorites;
using MangaSrbija.DAL.Repositories.users;
using MangaSrbija.Presentation.Helpers;
using Microsoft.Extensions.Caching.Distributed;

namespace MangaSrbija.BLL.services.UserServices
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMangaUserFavorites _mangaUserFavorites;
        private readonly IMangaUserReadLater _mangaUserReadLater;

        private readonly IDistributedCache _cache;

        private readonly MangaService _mangaService;

     
        public UserService(IUserRepository userRepository
                           , IMangaUserFavorites mangaUserFavorites
                           , MangaService mangaService
                           , IMangaUserReadLater mangaUserReadLater,
                            IDistributedCache cache)
        {
            _userRepository = userRepository;
            _mangaUserFavorites = mangaUserFavorites;
            _mangaUserReadLater = mangaUserReadLater;
            _mangaService = mangaService;
            _cache = cache;
        }

        public void CheckUsername(string username)
        {

            User user = _userRepository.GetByUsernameOrEmail(username);

            if (user.Id != 0) { throw new BusinessException("User with given username or email exists!", 409); }

        }

        public List<int> GetAllUsersIds()
        {
            return _userRepository.GetAllUsersIds();
        }

        public UserSingleDTO GetUserById(int id)
        {
            try
            {

                User user = _userRepository.GetById(id);

                UserSingleDTO userDto = new UserSingleDTO(user);


                return userDto;
            }
            catch (Exception e)
            {
                throw new BusinessException("User not found", 404);
            }
        }

        public UserSingleDTO CreateUser(CreateUserDTO createUserDTO)
        {

            User user = createUserDTO.ToUser();

            user.Id = _userRepository.Save(user);


            return new UserSingleDTO(user);
        }

        public async Task ForgetPassword(ForgetPasswordDTO forgetPasswordDTO)
        {
            try
            {

                User user = _userRepository.GetByUsernameOrEmail(forgetPasswordDTO.UsernameOrEmail);

                if (user.Id == 0) { throw new BusinessException("User does not exists!", 400); }

                string code = RandomGenerator.Get6DigitsCode();

                await _cache.SetRecordAsync(code, user.Id, TimeSpan.FromMinutes(5));

                await EmailClient.Send(user.Username, user.Email,code);

            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, 401);
            }
        }

        public async Task<UserAuthorize> GetForgetedPasswordUser(string code)
        {

            int userID = await _cache.GetRecordAsync<int>(code);


            User user = _userRepository.GetById(userID);


            return new UserAuthorize(user);
        }

        public UserSingleDTO ChangeUserPassword(UserAuthorize user, ChangeUserPasswordDTO userPassword)
        {

            UserSingleDTO userSingleDTO = CheckUserPolicy(user);

            try
            {
               
                _userRepository.UpdateUserPasswod(userPassword.Password, userSingleDTO.Id);


                return userSingleDTO;
            }
            catch (Exception e)
            {
                throw new BusinessException(e.Message,500);
            }
        }



        public UserAuthorize GetByUsernameOrEmailAndPassword(string usernameOrEmail, string password)
        {
            try
            {
                User user = _userRepository.GetByUsernameAndPassword(usernameOrEmail, password);


                return new UserAuthorize(user);
            }
            catch (Exception ex)
            {
                throw new BusinessException(ex.Message, 401);
            }

        }



        public MangaSingle AddMangaToUserFavorites(UserAuthorize user, AddMangaToFavoritesDTO addMangaToFavoritesDTO)
        {

            UserSingleDTO userSingleDTO = CheckUserPolicy(user);

            MangaSingle manga = _mangaService.GetById(addMangaToFavoritesDTO.MangaId);

            _mangaUserFavorites.Save(manga.ToManga(), userSingleDTO.Id);


            return manga;
        }

        public void DeleteMangaFromUserFavorites(int mangaId, UserAuthorize user)
        {

            UserSingleDTO userSingleDTO = CheckUserPolicy(user);

            try
            {
                _mangaUserFavorites.Delete(mangaId, userSingleDTO.Id);
            }
            catch (Exception e)
            {
                throw new BusinessException(e.Message, 500);
            }

        }

        public List<MangaSingle> GetUserFavoriteMangas(UserAuthorize user)
        {

            UserSingleDTO userSingleDTO = CheckUserPolicy(user);

            try
            {
                List<Manga> mangas = _mangaUserFavorites.GetAllByUser(userSingleDTO.Id);

                if (mangas.Count() == 0) return new List<MangaSingle>();

                return mangas.Select(m => new MangaSingle(m)).ToList();
            }
            catch (Exception e)
            {
                throw new BusinessException(e.Message, 500);
            }
        }

        public MangaSingle AddMangaToUserReadLater(UserAuthorize user, AddMangaToFavoritesDTO addMangaToFavoritesDTO)
        {

            UserSingleDTO userSingleDTO = CheckUserPolicy(user);

            MangaSingle manga = _mangaService.GetById(addMangaToFavoritesDTO.MangaId);

            _mangaUserReadLater.Save(manga.ToManga(), userSingleDTO.Id);


            return manga;
        }

        public void DeleteMangaFromUserReadLater(int mangaId, UserAuthorize user)
        {

            UserSingleDTO userSingleDTO = CheckUserPolicy(user);

            try
            {
                _mangaUserReadLater.Delete(mangaId, userSingleDTO.Id);
            }
            catch (Exception e)
            {
                throw new BusinessException(e.Message, 500);
            }

        }

        public List<MangaSingle> GetUserReadLaterMangas(UserAuthorize user)
        {

            UserSingleDTO userSingleDTO = CheckUserPolicy(user);


            return _mangaUserReadLater.GetAllByUser(userSingleDTO.Id).Select(m => new MangaSingle(m)).ToList();
        }


        private UserSingleDTO CheckUserPolicy(UserAuthorize user)
        {

            UserPolicy.IsUserActive(user);

            UserSingleDTO userSingleDTO = new UserSingleDTO(user);


            return userSingleDTO;
        }
    }
}
