using Microsoft.AspNetCore.Http;

namespace MangaSrbija.BLL.mappers.Chapters.Photos
{
    public class UploadChapterPhotosDTO
    {
        public List<string> Photos { get; set; } = new List<string>();
    }
}
