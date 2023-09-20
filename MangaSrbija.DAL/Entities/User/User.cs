namespace MangaSrbija.DAL.Entities.User
{
    public class User
    {
        public int Id { get; set; } = 0;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } =string.Empty;
        public string ProfilePicture { get; set; } = string.Empty;
        public bool Active { get; set; } = false;
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
