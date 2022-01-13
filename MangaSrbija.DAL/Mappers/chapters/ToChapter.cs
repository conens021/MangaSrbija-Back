using MangaSrbija.DAL.Entities.Chapter;
using System.Data.SqlClient;

namespace MangaSrbija.DAL.Mappers.chapters
{
    public class ToChapter
    {
        public static Chapter WithAllFields(SqlDataReader reader)
        { 
        
            Chapter chapter = new Chapter();

            chapter.Id = Convert.ToInt32(reader["Id"]);

            var name = reader["Name"].ToString();
            chapter.Name = name == null ? "" : name;

            chapter.MangaId = Convert.ToInt32(reader["MangaId"]);

            chapter.CreatedAt = Convert.ToDateTime(reader["CreatedAt"]);
            chapter.UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"]);

            return chapter;

        }
    }
}
