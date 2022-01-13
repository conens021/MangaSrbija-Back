using MangaSrbija.DAL.Entities.Chapter;

namespace MangaSrbija.BLL.mappers.Chapters.Photos
{
    public class ChapterPhotoPath
    {

        public ChapterPhotoPath(ChapterPhoto cps)
        { 
            Path = cps.Path;
        }

        public string Path { get; set; } = string.Empty;

    }
}
