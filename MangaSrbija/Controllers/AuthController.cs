using MangaSrbija.BLL.mappers.UserAuth;
using MangaSrbija.BLL.mappers.Users;
using MangaSrbija.BLL.services.UserServices;
using MangaSrbija.Presentation.Attributes;
using MangaSrbija.Presentation.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace MangaSrbija.Presentation.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly UserService _userService;

        private readonly JwtAuthenticationManager _jwtAuthenticationManager;


        public AuthController(UserService userService,
                              JwtAuthenticationManager jwtAuthenticationManager)
        {
            _userService = userService;
            _jwtAuthenticationManager = jwtAuthenticationManager;
        }

        [AllowAnonymous]
        [HttpPost("/auth")]
        public ActionResult UserAuthenticate([FromBody] UserAuthenticate user)
        {

            UserAuthorize userFromDb = _userService.GetByUsernameOrEmailAndPassword(user.UserName, user.Password);


            return Ok(new UserSession(userFromDb, _jwtAuthenticationManager.Authenticate(userFromDb)));
        }

        [HttpPost("forget-password/email")]
        public async Task<ActionResult> ForgetPassword([FromBody] ForgetPasswordDTO forgetPasswordDTO)
        {

            await _userService.ForgetPassword(forgetPasswordDTO);


            return Ok();
        }

        [HttpGet("forget-password/email/{code}")]
        public async Task<ActionResult> PasswordReset(string code)
        {

            UserAuthorize userAuthorize = await _userService.GetForgetedPasswordUser(code);


            return Ok(new UserRecoveryDTO(userAuthorize.UserId, _jwtAuthenticationManager.Authenticate(userAuthorize)));
        }
    }
}
