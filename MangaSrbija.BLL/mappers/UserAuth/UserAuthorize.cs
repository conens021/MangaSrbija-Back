using MangaSrbija.DAL.Entities.User;

namespace MangaSrbija.BLL.mappers.UserAuth
{
    public class UserAuthorize
    {

        public UserAuthorize() { }

        public UserAuthorize(User user)
        {
            UserId = user.Id.ToString();
            Username = user.Username;
            Email = user.Email;
            Active = user.Active ? "ACTIVE" : "INACTIVE";
            Role = user.RoleName;
        }

        public string UserId { get; set; } = String.Empty;
        public string Username { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string Role { get; set; } = String.Empty;
        public string Active { get; set; } = String.Empty;
    }
}
