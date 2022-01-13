using MangaSrbija.DAL.Entities.Category;
using MangaSrbija.DAL.Entities.EManga;
using MangaSrbija.DAL.Mappers;
using MangaSrbija.DAL.Mappers.category;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MangaSrbija.DAL.Repositories
{
    public class MangaRepository : IMangaRepository
    {

        private readonly IConfiguration _configuration;

        public MangaRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Manga GetById(int id)
        {
            string SQL = "Select * from Mangas where Id = @MangaId";

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand command = new SqlCommand(SQL, connection))
                {

                    command.Parameters.AddWithValue("@MangaId", id);

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (!reader.HasRows) return new Manga();

                    reader.Read();

                    Manga manga = ToManga.WithAllFields(reader);


                    return manga;
                }
            }
        }

        public void DeleteManga(int id)
        {
            string SQL = "Delete from Mangas where Id = @MangaId";

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand command = new SqlCommand(SQL, connection))
                {

                    command.Parameters.AddWithValue("@MangaId", id);

                    connection.Open();

                    command.ExecuteNonQuery();
                }
            }
        }

        public int SaveManga(Manga manga)
        {
            string SQL = "createManga";

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand command = new SqlCommand(SQL, connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Title", manga.Title);
                    command.Parameters.AddWithValue("@Summary", manga.Summary);
                    command.Parameters.AddWithValue("@CoverPhoto", manga.CoverPhoto);
                    command.Parameters.AddWithValue("@Type", manga.Type);

                    connection.Open();

                    int id = Convert.ToInt32(command.ExecuteScalar());


                    return id;
                }
            }
        }

        public void UpdateManga(Manga manga)
        {
            string SQL = "updateManga";

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand command = new SqlCommand(SQL, connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Title", manga.Title);
                    command.Parameters.AddWithValue("@Summary", manga.Summary);
                    command.Parameters.AddWithValue("@Type", manga.Type);
                    command.Parameters.AddWithValue("@MangaId", manga.Id);
                    command.Parameters.AddWithValue("@MangaCategories", manga.Categories);
                    
                    connection.Open();

                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateMangaCoverPhoto(string coverPhoto, int id)
        {
            string SQL = "Update mangas set CoverPhoto = @CoverPhoto Where Id = @MangaId";

            List<Manga> mangas = new List<Manga>();

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand command = new SqlCommand(SQL, connection))
                {
                    command.Parameters.AddWithValue("@CoverPhoto", coverPhoto);
                    command.Parameters.AddWithValue("@MangaId", id);

                    connection.Open();

                    command.ExecuteNonQuery();

                }
            }
        }

        public List<Manga> GetRecentlyUpdated(int perPage)
        {
            string SQL = "Select TOP(@PerPage) * from Mangas ORDER BY UpdatedAt DESC";

            List<Manga> mangas = new List<Manga>();

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand command = new SqlCommand(SQL, connection))
                {
                    command.Parameters.AddWithValue("@PerPage", perPage);

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (!reader.HasRows) return mangas;

                    while (reader.Read())
                    {
                        Manga manga = ToManga.WithAllFields(reader);
                        mangas.Add(manga);
                    }


                    return mangas;
                }
            }
        }

        public IEnumerable<Manga> UpdateAll(IEnumerable<Manga> mangas)
        {

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    using (SqlCommand command = connection.CreateCommand())
                    {

                        command.Transaction = transaction;
                        command.CommandText = "updateManga";
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@MangaId", SqlDbType.Int));
                        command.Parameters.Add(new SqlParameter("@Title", SqlDbType.NVarChar));
                        command.Parameters.Add(new SqlParameter("@Summary", SqlDbType.NVarChar));
                        command.Parameters.Add(new SqlParameter("@Type", SqlDbType.NVarChar));
                        command.Parameters.Add(new SqlParameter("@MangaCategories", SqlDbType.NVarChar));

                        try
                        {
                            foreach (var manga in mangas)
                            {
                                command.Parameters[0].Value = manga.Id;
                                command.Parameters[1].Value = manga.Title;
                                command.Parameters[2].Value = manga.Summary;
                                command.Parameters[3].Value = manga.Type;
                                command.Parameters[4].Value = manga.Categories;

                                if (command.ExecuteNonQuery() != 1)
                                {

                                    throw new InvalidProgramException();
                                }
                            }

                            transaction.Commit();

                            return mangas;
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
        }
    }
}