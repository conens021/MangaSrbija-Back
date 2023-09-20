using MangaSrbija.DAL.Entities.User;
using MangaSrbija.DAL.Mappers.users;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace MangaSrbija.DAL.Repositories.users
{
    public class UserRepository : IUserRepository
    {

        private readonly IConfiguration _configuration;

        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<int> GetAllUsersIds()
        {
            string SQL = "select Id from [Users]";

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand command = new SqlCommand(SQL, connection))
                {

                    List<int> ids = new List<int>();        


                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (!reader.HasRows) throw new Exception("User does not exists!");

                    while (reader.Read())
                    {

                        int id = Convert.ToInt32(reader["Id"]);


                        ids.Add(id);
                    }


                    return ids;
                }
            }
        }

        public User GetById(int id)
        {
            string SQL = "GetUserById";

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand command = new SqlCommand(SQL, connection))
                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@UserId", id);

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (!reader.HasRows) throw new Exception("User does not exists!");

                    reader.Read();

                    User user = ToUser.WithAllFields(reader);


                    return user;
                }
            }
        }

        public User GetByUsernameAndPassword(string usernameOrEmail, string password)
        {
            string SQL = "GetUserByUsernameOrEmail";

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand command = new SqlCommand(SQL, connection))
                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Username", usernameOrEmail);
                    command.Parameters.AddWithValue("@Email", usernameOrEmail);

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (!reader.HasRows) throw new Exception("User with given credential not found!");

                    reader.Read();

                    User user = ToUser.WithAllFields(reader);

                    if (!user.Password.Equals(password)) throw new Exception("User with given credential not found!");

                    return user;
                }
            }
        }

        public User GetByUsernameOrEmail(string usernameOrEmail)
        {
            string SQL = "GetUserByUsernameOrEmail";

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand command = new SqlCommand(SQL, connection))
                {

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Username", usernameOrEmail);
                    command.Parameters.AddWithValue("@Email", usernameOrEmail);

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (!reader.HasRows) return new User();

                    reader.Read();

                    User user = ToUser.WithAllFields(reader);

                    return user;
                }
            }
        }

        public int Save(User user)
        {
            string SQL = "AddUser";

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand command = new SqlCommand(SQL, connection))
                {

                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Username", user.Username);
                    command.Parameters.AddWithValue("@Email", user.Email);
                    command.Parameters.AddWithValue("@ProfilePicture", user.ProfilePicture);

                    SqlParameter paramPassword = command.CreateParameter();
                    paramPassword.ParameterName = "@Password";
                    paramPassword.DbType = DbType.AnsiStringFixedLength;
                    paramPassword.Value = user.Password;
                    command.Parameters.Add(paramPassword);

                    connection.Open();

                    int id = Convert.ToInt32(command.ExecuteScalar());


                    return id;
                }
            }
        }

        public User Update(User user)
        {
            throw new NotImplementedException();
        }

        public void UpdateUserPasswod(string password, int userId)
        {
            string SQL = "UpdateUserPassword";

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand command = new SqlCommand(SQL, connection))
                {

                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@UserId", userId);

                    SqlParameter paramPassword = command.CreateParameter();
                    paramPassword.ParameterName = "@Password";
                    paramPassword.DbType = DbType.AnsiStringFixedLength;
                    paramPassword.Value = password;
                    command.Parameters.Add(paramPassword);

                    connection.Open();

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
