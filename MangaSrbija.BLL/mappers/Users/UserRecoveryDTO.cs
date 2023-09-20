namespace MangaSrbija.BLL.mappers.Users
{
    public class UserRecoveryDTO
    {

        public UserRecoveryDTO(string userId, string jwt)
        {
            UserId = userId;
            JWT = jwt;
        }

        public string UserId { get; set; }
        public string JWT { get; set; } 
    }
}
