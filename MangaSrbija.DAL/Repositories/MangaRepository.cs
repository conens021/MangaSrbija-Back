using MangaSrbija.DAL.Entities.EManga;
using MangaSrbija.DAL.Mappers;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

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

        public void UpdateMangaChapterRD(Manga manga)
        {
            string SQL = "Update mangas set LastChapterRd = @LastChapterRD where id = @MangaId";

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand command = new SqlCommand(SQL, connection))
                {

                    command.Parameters.AddWithValue("@MangaId", manga.Id);
                    command.Parameters.AddWithValue("@LastChapterRD", manga.LastChapterRd);

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

        public List<Manga> GetMostPopular(int page,int perPage)
        {
            string SQL = "GetMostPopularMangas";

            List<Manga> mangas = new List<Manga>();

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand command = new SqlCommand(SQL, connection))
                {

                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@PerPage", perPage);
                    command.Parameters.AddWithValue("@Page", page);

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

        public double RateManga(int mangaId,int userId, double rating)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    using (SqlCommand command = connection.CreateCommand())
                    {


                        double mangaRating = 0.00;

                        command.Transaction = transaction;
                        command.CommandText = "RateMangaByUser";
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@Rating", rating);
                        command.Parameters.AddWithValue("@UserId", userId);
                        command.Parameters.AddWithValue("@MangaId", mangaId);

                        try
                        {
                            mangaRating = Convert.ToDouble(command.ExecuteScalar());

                            command.CommandText = "Update mangas Set rating = @MangaRating WHERE id = @MangaId";
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@MangaId", mangaId);
                            command.Parameters.AddWithValue("@MangaRating", mangaRating);

                            command.CommandType = CommandType.Text;

                            command.ExecuteNonQuery();

                            transaction.Commit();


                            return mangaRating;
                        }
                        catch (Exception e)
                        {
                            transaction.Rollback();
                            throw new Exception(e.Message);
                        }

                    }
                }
            }
        }

        public int CountAll()
        {
            string SQL = "select count(Mangas.Id) as count from mangas";

            List<Manga> mangas = new List<Manga>();

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand command = new SqlCommand(SQL, connection))
                {

                    connection.Open();

                    int count = Convert.ToInt32(command.ExecuteScalar());


                    return count;
                }
            }
        }

        public List<Manga> GetAll(string orderBy, int page, int perPage)
        {
            string SQL = "GetAll";

            List<Manga> mangas = new List<Manga>();

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand command = new SqlCommand(SQL, connection))
                {

                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@OrderBy", orderBy);
                    command.Parameters.AddWithValue("@PerPage", perPage);
                    command.Parameters.AddWithValue("@Page", page);

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

        public List<Manga> GetByLetter(string letter, string orderBy, int page, int perPage)
        {
            string SQL = "GetByStartingLetter";

            List<Manga> mangas = new List<Manga>();

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand command = new SqlCommand(SQL, connection))
                {

                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@StartingLetter", letter);
                    command.Parameters.AddWithValue("@OrderBy", orderBy);
                    command.Parameters.AddWithValue("@PerPage", perPage);
                    command.Parameters.AddWithValue("@Page", page);

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

        public List<int> GetAllIds()
        {
            string SQL = "select id from Mangas";

            List<int> ids = new List<int>();

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand command = new SqlCommand(SQL, connection))
                {


                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (!reader.HasRows) return ids;

                    while (reader.Read())
                    {
                        int id = ToManga.GetId(reader);
                        ids.Add(id);
                    }


                    return ids;
                }
            }
        }

        public List<Manga> GetByNameOrSummary(string query)
        {
            string SQL = "SearchMangas";

            List<Manga> mangas = new List<Manga>();

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand command = new SqlCommand(SQL, connection))
                {

                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@Query", query);

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
    }
}