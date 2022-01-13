namespace MangaSrbija.BLL.helpers
{
    public static class FileHandler
    {
        public async static Task<string> Save(String file, String folder)
        {

            if (Base64Decoder.GetFileType(file) != "image")
                throw new Exception("Unsupported media type. Supported files:'.jpg','.png','.svg','.gif'");

            string fileExtension = Base64Decoder.GetExtension(file);
            string uniqueName = Guid.NewGuid().ToString() + "." + fileExtension;

            string fileContent = Base64Decoder.GetContent(file);
            byte[] fileBytes = Convert.FromBase64String(fileContent);

            PathRegistry pathRegistry = PathRegistry.GetInstance();

            string folderPath = Path.Combine(pathRegistry.WebRootPath, folder);

            string fullPath = Path.Combine(folderPath, uniqueName);

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            await File.WriteAllBytesAsync(fullPath, fileBytes);


            return Path.Combine(folder, uniqueName);
        }

        public static void Delete(String file)
        {
            PathRegistry pathRegistry = PathRegistry.GetInstance();

            string fullPath = Path.Combine(pathRegistry.WebRootPath, file);

            if (!File.Exists(fullPath)) throw new Exception("File does not exists");

            File.Delete(fullPath);
        }
    }
}
