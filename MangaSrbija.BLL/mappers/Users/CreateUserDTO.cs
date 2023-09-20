using MangaSrbija.DAL.Entities.User;

namespace MangaSrbija.BLL.mappers.Users
{
    public class CreateUserDTO
    {

        public User ToUser()
        {

            User user = new User();
            user.Username = UserName;
            user.Password = Password;
            user.Email = Email;
            user.ProfilePicture = ProfilePicture;
            user.UpdatedAt = DateTime.Now;
            user.CreatedAt = DateTime.Now;
            user.Active = false;

            return user;
        }

        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string ProfilePicture { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string lastName { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public DateOnly DateOfBirth{get;set;}
    }
}
