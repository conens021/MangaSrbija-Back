
using MangaSrbija.BLL.mappers.UserAuth;
using MangaSrbija.DAL.Entities.User;

namespace MangaSrbija.BLL.mappers.Users
{
    public class UserSingleDTO
    {

        public UserSingleDTO() { }

        public UserSingleDTO(UserAuthorize userAuthorize)
        {
            Id = Convert.ToInt32(userAuthorize.UserId);
            UserName = userAuthorize.Username;
            Email = userAuthorize.Email;
            isActive = userAuthorize.Active.Equals("ACTIVE") ? true : false;
            UserRole = userAuthorize.Role;

        }
        public UserSingleDTO (User user)
        {
            Id = user.Id;
            UserName = user.Username;
            Password = user.Password;
            Email = user.Email;
            ProfilePicture = user.ProfilePicture;
            isActive = user.Active;
        }

        public int Id { get; set; } 
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ProfilePicture { get; set; } = string.Empty;
        public bool isActive { get; set; }
        public string UserRole { get; set; } = "User";
    }
}
