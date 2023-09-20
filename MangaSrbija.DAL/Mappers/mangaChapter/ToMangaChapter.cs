
using MangaSrbija.DAL.Entities.Chapter;
using MangaSrbija.DAL.Entities.EManga;
using MangaSrbija.DAL.Entities.MangaChapter;
using Microsoft.Data.SqlClient;

namespace MangaSrbija.DAL.Mappers.mangaChapter
{
    public class ToMangaChapter
    {
        public static MangaChapter WithAllFieldsJOIN(SqlDataReader reader)
        {

            MangaChapter mangaChapter = new MangaChapter();

            Manga manga = new Manga();

            Chapter chapter = new Chapter();

            manga.Id = Convert.ToInt32(reader["MangaId"]);

            var title = reader["MangaTitle"].ToString();
            manga.Title = title == null ? "" : title;

            var coverPhoto = reader["CoverPhoto"].ToString();
            manga.CoverPhoto = coverPhoto == null ? "" : coverPhoto;

            chapter.Id = Convert.ToInt32(reader["ChapterId"]);

            var name = reader["ChapterName"].ToString();
            chapter.Name = name == null ? "" : name;

            mangaChapter.Chapter = chapter;
            mangaChapter.Manga = manga;


            return mangaChapter;
        }
    }
}
