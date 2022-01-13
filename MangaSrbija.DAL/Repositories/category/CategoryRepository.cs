using MangaSrbija.DAL.Entities.Category;
using MangaSrbija.DAL.Entities.EManga;
using MangaSrbija.DAL.Mappers;
using MangaSrbija.DAL.Mappers.category;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace MangaSrbija.DAL.Repositories.category
{
    public class CategoryRepository : ICategoryRepository
    {

        private readonly IConfiguration _configuration;

        public CategoryRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Category GetById(int id)
        {
            string SQL = "Select * from Category where Id = @CategoryId";

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand command = new SqlCommand(SQL, connection))
                {
                    command.Parameters.AddWithValue("@CategoryId", id);

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (!reader.HasRows) return new Category();

                    reader.Read();

                    Category category = ToCategory.WithAllFields(reader);


                    return category;
                }
            }
        }

        public Category GetByName(string name)
        {
            string SQL = "Select * from Category where Name = @CategoryName";

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand command = new SqlCommand(SQL, connection))
                {
                    command.Parameters.AddWithValue("@CategoryName", name);

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (!reader.HasRows) return new Category();

                    reader.Read();

                    Category category = ToCategory.WithAllFields(reader);


                    return category;
                }
            }
        }

        public void DeleteCategory(int id)
        {
            string SQL = "Delete from Category  Where id = @Id";

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand command = new SqlCommand(SQL, connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    connection.Open();

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Category> GetAll()
        {
            string SQL = "Select  * from Category ORDER BY Name ASC";

            List<Category> categories = new List<Category>();

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand command = new SqlCommand(SQL, connection))
                {

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (!reader.HasRows) return categories;

                    while (reader.Read())
                    {
                        Category category = ToCategory.WithAllFields(reader);

                        categories.Add(category);
                    }


                    return categories;
                }
            }
        }

        public int SaveCategory(Category category)
        {
            string SQL = "Insert into Category values(@CategoryName);SELECT SCOPE_IDENTITY()";

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand command = new SqlCommand(SQL, connection))
                {
                    command.Parameters.AddWithValue("@CategoryName", category.Name);

                    connection.Open();

                    int id = Convert.ToInt32(command.ExecuteScalar());


                    return id;
                }
            }
        }

        public void UpdateCategory(Category category)
        {
            string SQL = "Update Category set Name = @Name Where id = @Id";

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand command = new SqlCommand(SQL, connection))
                {
                    command.Parameters.AddWithValue("@Name", category.Name);
                    command.Parameters.AddWithValue("@Id", category.Id);

                    connection.Open();

                    command.ExecuteNonQuery();
                }
            }
        }

        public Dictionary<Category, List<Manga>> GetMangasByCategoryName(string name, int page, int size)
        {
            Dictionary<Category, List<Manga>> dict = new();

            string SQL = "getMangasByCategoryName";

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand command = new SqlCommand(SQL, connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@CategoryName", name);

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (!reader.HasRows) return dict;

                    int row = 1;

                    Category category = new Category();
                    List<Manga> mangaList = new List<Manga>();

                    while (reader.Read())
                    {
                        if (row == 1)
                        {
                            category = ToCategory.WithJoin(reader);
                            
                        }

                        Manga manga = ToManga.WithAllFieldsJOIN(reader);
                        mangaList.Add(manga);

                        row++;

                    }

                    dict.Add(category, mangaList);


                    return dict;
                }
            }

        }
    }
}
