using MangaSrbija.BLL.exceptions;
using MangaSrbija.BLL.mappers.UserAuth;

namespace MangaSrbija.BLL.helpers
{
    public class UserPolicy
    {

        public static void IsUserActive(UserAuthorize user)
        {
            if (!user.Active.Equals("ACTIVE")) throw new BusinessException("User profile is not active yet!", 401);
        }

        public static bool isUserAuthor(UserAuthorize user)
        {
            return user.Role.Equals("AUTHOR");
        }

        public static bool isUserAdmin(UserAuthorize user)
        {
            return user.Role.Equals("ADMIN");
        }
    }
}
