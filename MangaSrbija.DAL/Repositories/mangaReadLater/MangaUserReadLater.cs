using MangaSrbija.DAL.Entities.EManga;
using MangaSrbija.DAL.Mappers;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace MangaSrbija.DAL.Repositories.mangaReadLater
{
    public class MangaUserReadLater : IMangaUserReadLater
    {

        private readonly IConfiguration _configuration;

        public MangaUserReadLater(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public Manga GetById(int mangaId, int userId)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id, int userId)
        {
            string SQL = "Delete from UserReadLaterMangas WHERE UserId = @UserId and MangaId = @MangaId";

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand command = new SqlCommand(SQL, connection))
                {

                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@MangaId", id);

                    connection.Open();
                    try
                    {
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
        }

        public List<Manga> GetAllByUser(int userId)
        {
            string SQL = "GetUserReadLaterMangas";

            List<Manga> mangas = new List<Manga>();   

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand command = new SqlCommand(SQL, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (!reader.HasRows)  throw new Exception("User does not have read later mangas yet!");

                    while (reader.Read())
                    {
                        Manga manga = ToManga.WithAllFieldsJOIN(reader);
                        mangas.Add(manga);
                    }
                   

                    return mangas;
                }
            }
        }

        public int Save(Manga manga, int userId)
        {
            string SQL = "Insert into UserReadLaterMangas values(@UserId,@MangaId);SELECT SCOPE_IDENTITY()";

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand command = new SqlCommand(SQL, connection))
                {

                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@MangaId", manga.Id);


                    connection.Open();

                    int id = Convert.ToInt32(command.ExecuteScalar());


                    return id;
                }
            }
        }
    }
}
