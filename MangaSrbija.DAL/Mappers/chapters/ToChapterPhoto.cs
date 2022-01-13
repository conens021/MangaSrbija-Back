using MangaSrbija.DAL.Entities.Chapter;
using System.Data.SqlClient;

namespace MangaSrbija.DAL.Mappers.chapters
{
    public class ToChapterPhoto
    {
        public static ChapterPhoto WithAllFields(SqlDataReader reader)
        {

            ChapterPhoto chapterPhoto = new ChapterPhoto();

            chapterPhoto.Id = Convert.ToInt32(reader["Id"]);

            var path = reader["Path"].ToString();
            chapterPhoto.Path = path == null ? "" : path;

            chapterPhoto.PageNumber = Convert.ToInt32(reader["PageNumber"]);

            chapterPhoto.CreatedAt = Convert.ToDateTime(reader["CreatedAt"]);
            chapterPhoto.UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"]);

            chapterPhoto.ChapterId = Convert.ToInt32(reader["ChapterId"]);


            return chapterPhoto;
        }
    }
}
