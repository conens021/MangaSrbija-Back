
using SixLabors.ImageSharp;
namespace MangaSrbija.BLL.helpers
{
    public static class ImageHandler
    {
        public static int GetHeight(byte[] fileBytes)
        {

            Image image = Image.Load(fileBytes);

            return image.Height;
        }


        public static int GetWidth(byte[] fileBytes)
        {

            Image image = Image.Load(fileBytes);


            return image.Width;
        }
    }
}
