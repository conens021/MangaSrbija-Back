using MangaSrbija.DAL.Entities.EManga;
using Microsoft.Data.SqlClient;

namespace MangaSrbija.DAL.Mappers
{
    public static class ToManga
    {

        public static Manga WithAllFieldsJOIN(SqlDataReader reader)
        {
            Manga manga = new Manga();

            manga.Id = Convert.ToInt32(reader["MangaId"]);

            var title = reader["MangaTitle"].ToString();
            manga.Title = title == null ? "" : title;

            var summary = reader["Summary"].ToString();
            manga.Summary = summary == null ? "" : summary;

            var coverPhoto = reader["CoverPhoto"].ToString();
            manga.CoverPhoto = coverPhoto == null ? "" : coverPhoto;

            var type = reader["Type"].ToString();
            manga.Type = type == null ? "" : type;

            manga.Views = Convert.ToInt32(reader["Views"]);

            manga.Rating = Convert.ToDouble(reader["Rating"]);

            var categories = reader["MangaCategories"].ToString();
            manga.Categories = categories == null ? "" : categories;


            return manga;
        }

        public static Manga WithAllFields(SqlDataReader reader)
        {

            Manga manga = new Manga();
            
            manga.Id = Convert.ToInt32(reader["Id"]);

            var title = reader["Title"].ToString();
            manga.Title = title == null ? "" : title;

            var summary = reader["Summary"].ToString();
            manga.Summary = summary == null ? "" : summary;

            var coverPhoto = reader["CoverPhoto"].ToString();
            manga.CoverPhoto = coverPhoto == null ? "" : coverPhoto;

            var type = reader["Type"].ToString();
            manga.Type = type == null ? "" : type;

            manga.Views = Convert.ToInt32(reader["Views"]);

            manga.Rating = Convert.ToDouble(reader["Rating"]);

            var categories = reader["Categories"].ToString();
            manga.Categories = categories == null ? "" : categories;

            manga.CreatedAt = Convert.ToDateTime(reader["CreatedAt"]);
            manga.UpdatedAt = Convert.ToDateTime(reader["UpdatedAt"]);

            manga.LastChapterRd = Convert.ToDateTime(reader["LastChapterRd"]);


            return manga;
        }

        public static int GetId(SqlDataReader reader)
        {
            return Convert.ToInt32(reader["Id"]);
        }
    }
}
