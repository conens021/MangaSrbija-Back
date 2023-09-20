namespace MangaSrbija.BLL.mappers.UserAuth
{
    public class UserSession
    {

        public UserSession(UserAuthorize user,string jwt)
        {
            User = user;
            Jwt = jwt;
        }

        public UserAuthorize User { get; set; }
        public string Jwt { get; set; }

    }
}
