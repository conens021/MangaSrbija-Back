using MangaSrbija.DAL.Entities.Category;
using Microsoft.Data.SqlClient;

namespace MangaSrbija.DAL.Mappers.category
{
    public static class ToCategory
    {
        public static Category WithAllFields(SqlDataReader reader)
        {

            Category category = new Category();

            category.Id = Convert.ToInt32(reader["Id"]);

            var name = reader["Name"].ToString();
            category.Name = name == null ? "" : name;


            return category;
        }

        public static Category WithJoin(SqlDataReader reader)
        {
            Category category = new Category();

            category.Id = Convert.ToInt32(reader["CategoryId"]);

            var name = reader["CategoryName"].ToString();
            category.Name = name == null ? "" : name;


            return category;
        }
    }
}
