using MangaSrbija.DAL.Entities.Chapter;
using MangaSrbija.DAL.Mappers.chapters;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MangaSrbija.DAL.Repositories.chapters
{
    public class ChapterPhotoRepository : IChapterPhotoRepository
    {

        private readonly IConfiguration _configuration;

        public ChapterPhotoRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<ChapterPhoto> GetAllByChapterId(int mangaId)
        {
            throw new NotImplementedException();
        }

        public int Save(ChapterPhoto chapterPhotos)
        {
            throw new NotImplementedException();
        }


        public IEnumerable<ChapterPhoto> GetPhotosByChapter(int chapterId)
        {

            List<ChapterPhoto> chapterPhotos = new List<ChapterPhoto>();

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {

                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {

                    command.CommandText = "select * from ChapterPhotos where ChapterId = @ChapterId";
                    command.Parameters.AddWithValue("@ChapterId", chapterId);

                    SqlDataReader reader = command.ExecuteReader();

                    if (!reader.HasRows) return chapterPhotos;

                    while (reader.Read())
                    {
                        chapterPhotos.Add(ToChapterPhoto.WithAllFields(reader));
                    }

                    return chapterPhotos;
                }
            }
        }

        public IEnumerable<ChapterPhoto> SaveAll(IEnumerable<ChapterPhoto> chapterPhotos)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {

                connection.Open();

                using SqlTransaction transaction = connection.BeginTransaction();

                using (SqlCommand command = connection.CreateCommand())
                {

                    command.Transaction = transaction;
                    command.CommandText = "Insert into ChapterPhotos values (@Path,@PageNumber,CURRENT_TIMESTAMP,CURRENT_TIMESTAMP,@ChapterId)";

                    command.Parameters.Add(new SqlParameter("@Path", SqlDbType.NVarChar));
                    command.Parameters.Add(new SqlParameter("@PageNumber", SqlDbType.Int));
                    command.Parameters.Add(new SqlParameter("@ChapterId", SqlDbType.Int));

                    try
                    {
                        foreach (var chapterPhoto in chapterPhotos)
                        {
                            command.Parameters[0].Value = chapterPhoto.Path;
                            command.Parameters[1].Value = chapterPhoto.PageNumber;
                            command.Parameters[2].Value = chapterPhoto.ChapterId;

                            if (command.ExecuteNonQuery() != 1)
                            {

                                throw new InvalidProgramException();
                            }

                            chapterPhoto.CreatedAt = DateTime.Now;
                            chapterPhoto.UpdatedAt = DateTime.Now;
                            chapterPhotos.Append(chapterPhoto);
                        }

                        transaction.Commit();

                        return chapterPhotos;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void DeleteAll(List<int> chapterPhotosId)
        {
            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {

                connection.Open();

                using SqlTransaction transaction = connection.BeginTransaction();

                using (SqlCommand command = connection.CreateCommand())
                {

                    command.Transaction = transaction;
                    command.CommandText = "Delete from ChapterPhotos where Id = @ChapterPhotoId";

                    command.Parameters.Add(new SqlParameter("@ChapterPhotoId", SqlDbType.Int));

                    try
                    {
                        foreach (int id in chapterPhotosId)
                        {
                            command.Parameters[0].Value = id;

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
}
