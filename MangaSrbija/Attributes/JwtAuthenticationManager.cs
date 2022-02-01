using MangaSrbija.BLL.mappers.UserAuth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MangaSrbija.Presentation.Attributes
{
    public class JwtAuthenticationManager
    {

        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _contextAccesor;

        public JwtAuthenticationManager(IConfiguration configuration, IHttpContextAccessor contextAccesor)
        {
            _configuration = configuration;
            _contextAccesor = contextAccesor;
        }


        public string Authenticate(UserAuthorize user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenKey = Encoding.ASCII.GetBytes(_configuration["AppSettings:EncryptionKey"]);

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[] {
                    new Claim(ClaimTypes.NameIdentifier,user.UserId),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email,user.Email),
                    new Claim(ClaimTypes.Role,user.Role),
                    new Claim(ClaimTypes.IsPersistent,user.Active)
                }),
                Expires = DateTime.UtcNow.AddHours(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);

        }

        public UserAuthorize GetBearerUser()
        {

            var key = Encoding.ASCII.GetBytes(_configuration["AppSettings:EncryptionKey"]);

            var token = GetToken();

            var handler = new JwtSecurityTokenHandler();

            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };

            var claims = handler.ValidateToken(token, validations, out var tokenSecure);

            var claim = claims.Claims.FirstOrDefault();

            UserAuthorize userAuthorize =  ToUser(claims);

            return userAuthorize;
        }


        private string GetToken()
        {
            var jwtTokenValue = _contextAccesor?.HttpContext?.Request.Headers.Authorization.ToString().Replace("Bearer ", "");


            return jwtTokenValue;
        }


        private UserAuthorize ToUser(ClaimsPrincipal claims)
        {

            var id = claims.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier.ToString()).FirstOrDefault()?.Value;
            var username =  claims.Claims.Where(c => c.Type == ClaimTypes.Name.ToString()).FirstOrDefault()?.Value;
            var email  = claims.Claims.Where(c => c.Type == ClaimTypes.Email.ToString()).FirstOrDefault()?.Value;
            var  role =  claims.Claims.Where(c => c.Type == ClaimTypes.Role.ToString()).FirstOrDefault()?.Value;
            var active = claims.Claims.Where(c => c.Type == ClaimTypes.IsPersistent.ToString()).FirstOrDefault()?.Value;

            UserAuthorize userAuthorize = new UserAuthorize();

            userAuthorize.UserId = id == null ? "" : id;
            userAuthorize.Username = username == null ? "" : username;
            userAuthorize.Email = email == null ? "" : email;
            userAuthorize.Role = role == null ? "" : role;
            userAuthorize.Active = active == null ? "INACTIVE" : active;

                    
            return userAuthorize;
        }
    }
}
