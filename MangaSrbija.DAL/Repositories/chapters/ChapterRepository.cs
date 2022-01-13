﻿using MangaSrbija.DAL.Entities.Chapter;
using MangaSrbija.DAL.Mappers.chapters;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

namespace MangaSrbija.DAL.Repositories.chapters
{
    public class ChapterRepository : IChapterRepository
    {

        private readonly IConfiguration _configuration;

        public ChapterRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Chapter GetById(int id)
        {
            string SQL = "Select * from Chapters where Id = @ChapterId";

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand command = new SqlCommand(SQL, connection))
                {
                    command.Parameters.AddWithValue("@ChapterId", id);

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (!reader.HasRows) return new Chapter();

                    reader.Read();

                    Chapter category = ToChapter.WithAllFields(reader);


                    return category;
                }
            }
        }

        public void Delete(int id)
        {
            string SQL = "Delete from Chapters  Where id = @Id";

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

        public List<Chapter> GetAllByMangaId(int mangaId)
        {
            string SQL = "Select  * from Chapters Where MangaId = @MangaId ORDER BY Name DESC";

            List<Chapter> chapters = new List<Chapter>();

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand command = new SqlCommand(SQL, connection))
                {

                    command.Parameters.AddWithValue("@MangaId", mangaId);

                    connection.Open();

                    SqlDataReader reader = command.ExecuteReader();

                    if (!reader.HasRows) return chapters;

                    while (reader.Read())
                    {
                        Chapter category = ToChapter.WithAllFields(reader);

                        chapters.Add(category);
                    }


                    return chapters;
                }
            }
        }

        public int Save(Chapter chapter)
        {
            string SQL = "Insert into Chapters values(@ChapterName,@MangaId,CURRENT_TIMESTAMP,CURRENT_TIMESTAMP);SELECT SCOPE_IDENTITY()";

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand command = new SqlCommand(SQL, connection))
                {
                    command.Parameters.AddWithValue("@ChapterName", chapter.Name);
                    command.Parameters.AddWithValue("@MangaId", chapter.MangaId);

                    connection.Open();

                    int id = Convert.ToInt32(command.ExecuteScalar());


                    return id;
                }
            }
        }

        public Chapter Update(Chapter chapter)
        {
            string SQL = "Update Chapters set Name = @Name, MangaId = @MangaId,UpdatedAt = CURRENT_TIMESTAMP Where id = @Id";

            using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                using (SqlCommand command = new SqlCommand(SQL, connection))
                {
                    command.Parameters.AddWithValue("@Name", chapter.Name);
                    command.Parameters.AddWithValue("@MangaId", chapter.MangaId);
                    command.Parameters.AddWithValue("@Id", chapter.Id);

                    connection.Open();

                    command.ExecuteNonQuery();

                    chapter.UpdatedAt = DateTime.Now;


                    return chapter;
                }
            }
        }
    }
}
