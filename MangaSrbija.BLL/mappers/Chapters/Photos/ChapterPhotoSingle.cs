using MangaSrbija.DAL.Entities.Chapter;

namespace MangaSrbija.BLL.mappers.Chapters.Photos
{
    public class ChapterPhotoSingle
    {

        public  ChapterPhotoSingle(ChapterPhoto chapterPhoto)
        { 
            Id = chapterPhoto.Id;
            Path = chapterPhoto.Path;
            PageNumber = chapterPhoto.PageNumber;
            Height = chapterPhoto.Height;
            Width = chapterPhoto.Width;
            CreatedAt = chapterPhoto.CreatedAt; 
            UpdatedAt = chapterPhoto.UpdatedAt;
            ChapterId = chapterPhoto.ChapterId;
        }

        public ChapterPhoto ToChapterPhoto()
        { 

            ChapterPhoto chapterPhoto = new ChapterPhoto();
            chapterPhoto.Id = Id;
            chapterPhoto.Path = Path;
            chapterPhoto.PageNumber = PageNumber;
            chapterPhoto.CreatedAt = CreatedAt;
            chapterPhoto.UpdatedAt = UpdatedAt;
            
            
            return chapterPhoto;

        }

        public int Id { get; set; }
        public string Path { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1;
        public int Width { get; set; }
        public int Height { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int ChapterId { get; set; }

    }
}
