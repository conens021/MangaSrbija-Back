using MangaSrbija.DAL.Entities.Category;
using MangaSrbija.DAL.Entities.EManga;
using MangaSrbija.DAL.Mappers;
using MangaSrbija.DAL.Mappers.category;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MangaSrbija.DAL.Repositories.mangasCategories
{
    public class MangasCategoriesRepository : IMangasCategoriesRepository
    {


        private readonly IConfiguration _configuration;

        public MangasCategoriesRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<Category> GetMangaCategories(int mangaId)
        {
            string SQL = "getMangaCategories";

            List<Category> categories = new List<Category>();

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand command = new SqlCommand(SQL, connection))
                {

                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@MangaId", mangaId);

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (!reader.HasRows) return categories;

                    while (reader.Read())
                    {
                        Category category = ToCategory.WithJoin(reader);
                        categories.Add(category);
                    }


                    return categories;
                }
            }
        }

        public void SetMangaCategories(List<Category> categories, int mangaId)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.Transaction = transaction;
                        command.CommandText = "INSERT INTO MangasCategories VALUES (@mangaId,@categoryId);";
                        command.Parameters.Add(new SqlParameter("@categoryId", SqlDbType.Int));
                        command.Parameters.Add(new SqlParameter("@mangaId", SqlDbType.Int));

                        try
                        {
                            foreach (var category in categories)
                            {
                                command.Parameters[0].Value = category.Id;
                                command.Parameters[1].Value = mangaId;
                                if (command.ExecuteNonQuery() != 1)
                                {

                                    throw new InvalidProgramException();
                                }
                            }

                            transaction.Commit();
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

        public void DeleteMangaCategorie(int categoryId, int mangaId)
        {
            string SQL = "Delete from MangasCategories where MangaId = @MangaId and CategoryId = @CategoryId";

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand command = new SqlCommand(SQL, connection))
                {

                    command.Parameters.AddWithValue("@MangaId", mangaId);
                    command.Parameters.AddWithValue("@CategoryId", categoryId);

                    connection.Open();

                    command.ExecuteNonQuery();
                }
            }
        }

        public IEnumerable<Manga> GetAllContatainingCategory(string categoryName)
        {
            string SQL = "Select * from Mangas WHERE Categories like @CategoryName";

            List<Manga> mangas = new List<Manga>();

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand command = new SqlCommand(SQL, connection))
                {
                    command.Parameters.AddWithValue("@CategoryName", "%" + categoryName + "%");

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
