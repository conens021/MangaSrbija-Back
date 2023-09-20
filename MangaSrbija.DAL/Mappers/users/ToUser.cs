
using MangaSrbija.DAL.Entities.User;
using Microsoft.Data.SqlClient;

namespace MangaSrbija.DAL.Mappers.users
{
    public class ToUser
    {
        public static User WithAllFields(SqlDataReader reader)
        {

            User user = new User();

            user.Id = Convert.ToInt32(reader["Id"]);

            var username = reader["Username"].ToString();
            user.Username = username == null ? "" : username;

            var email = reader["Email"].ToString();
            user.Email = email == null ? "" : email;

            var profilePicture = reader["ProfilePicture"].ToString();
            user.ProfilePicture = profilePicture == null ? "" : profilePicture;

            var password = reader["Password"].ToString();
            user.Password = password == null ? "" : password;

            user.Active = Convert.ToBoolean(reader["Active"]);

            user.RoleId = Convert.ToInt32(reader["RoleId"]);

            var roleName = reader["Role"].ToString();
            user.RoleName = roleName == null ? "" : roleName;

            user.CreatedAt = Convert.ToDateTime(reader["CreatedAt"]);
            user.UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"]);


            return user;
        }

    }
}
