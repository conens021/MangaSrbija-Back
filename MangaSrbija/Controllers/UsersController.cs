using MangaSrbija.BLL.mappers.Mangas;
using MangaSrbija.BLL.mappers.Users;
using MangaSrbija.BLL.services.UserServices;
using MangaSrbija.Presentation.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MangaSrbija.Presentation.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {

        private readonly UserService _userService;
        private readonly JwtAuthenticationManager _jwtAuthenticationManager;

        public UsersController(UserService userService, JwtAuthenticationManager jwtAuthenticationManager)
        {
            _userService = userService;
            _jwtAuthenticationManager = jwtAuthenticationManager;
        }

        [AllowAnonymous]
        [HttpGet("ids")]
        public ActionResult GetAllUsersIds()
        {
            List<int> ids = _userService.GetAllUsersIds();


            return Ok(ids);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public ActionResult GetAllUsersIds(int id)
        {

            UserSingleDTO userDto = _userService.GetUserById(id);


            return Ok(userDto);
        }

        [AllowAnonymous]
        [HttpGet("available-username/{username}")]
        public ActionResult CheckUsername(string username)
        {

           _userService.CheckUsername(username);


            return Ok();
        }


        [AllowAnonymous]
        [HttpPost]
        public ActionResult Post([FromBody] CreateUserDTO createUserDTO)
        {

            UserSingleDTO userSingleDTO = _userService.CreateUser(createUserDTO);


            return Ok(userSingleDTO);
        }

        [HttpPatch]
        public ActionResult ChangeUserPassword([FromBody] ChangeUserPasswordDTO changeUserPasswordDTO)
        {
            UserSingleDTO userDto = _userService.ChangeUserPassword(_jwtAuthenticationManager.GetBearerUser(), changeUserPasswordDTO);

            return Ok(userDto);
        }

        [HttpPost("favorites")]
        public ActionResult AddMangaToUserFavorite([FromBody] AddMangaToFavoritesDTO addMangaToFavoritesDTO)
        {

            MangaSingle mangaSingle = _userService.AddMangaToUserFavorites(_jwtAuthenticationManager.GetBearerUser(), addMangaToFavoritesDTO);


            return Ok(mangaSingle);
        }

        [HttpDelete("favorites/{mangaId}")]
        public ActionResult DeleteMangaFromUserFavorites(int mangaId)
        {

            _userService.DeleteMangaFromUserFavorites(mangaId, _jwtAuthenticationManager.GetBearerUser());


            return Ok();
        }


        [HttpGet("favorites")]
        public ActionResult GetUserFavoriteMangas(int userId)
        {

            List<MangaSingle> mangaSingle = _userService.GetUserFavoriteMangas(_jwtAuthenticationManager.GetBearerUser());


            return Ok(mangaSingle);
        }

        [HttpPost("readlater")]
        public ActionResult AddMangaToUserReadLater([FromBody] AddMangaToFavoritesDTO addMangaToFavoritesDTO)
        {

            MangaSingle mangaSingle = _userService.AddMangaToUserReadLater(_jwtAuthenticationManager.GetBearerUser(), addMangaToFavoritesDTO);


            return Ok(mangaSingle);
        }

        [HttpDelete("readlater/{mangaId}")]
        public ActionResult DeleteMangaFromUserReadLater(int mangaId)
        {

            _userService.DeleteMangaFromUserReadLater(mangaId, _jwtAuthenticationManager.GetBearerUser());


            return Ok();
        }

        [HttpGet("readlater")]
        public ActionResult GetUserReadLaterMangas()
        {

            List<MangaSingle> mangaSingle = _userService.GetUserReadLaterMangas(_jwtAuthenticationManager.GetBearerUser());


            return Ok(mangaSingle);
        }
    }
}
