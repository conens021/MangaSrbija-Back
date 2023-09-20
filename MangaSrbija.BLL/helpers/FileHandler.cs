namespace MangaSrbija.BLL.helpers
{
    public static class FileHandler
    {
        public async static Task<string> Save(String file, String folder)
        {
            try
            {

                if (Base64Decoder.GetFileType(file) != "image")
                    throw new Exception("Unsupported media type. Supported files:'.jpg','.png','.svg','.gif'");

                string fileContent = Base64Decoder.GetContent(file);
                byte[] fileBytes = Convert.FromBase64String(fileContent);

                PathRegistry pathRegistry = PathRegistry.GetInstance();

                string folderPath = Path.Combine(pathRegistry.WebRootPath, folder);

                string uniqueName = GetUniqueName(file);

                string fullPath = Path.GetFullPath(Path.Combine(folderPath, uniqueName));

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                await File.WriteAllBytesAsync(fullPath, fileBytes);


                return Path.Combine(folder, uniqueName).Replace("\\", "/");

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        public static byte[] GetFileBytes(string file)
        {
            try
            {

                if (Base64Decoder.GetFileType(file) != "image")
                    throw new Exception("Unsupported media type. Supported files:'.jpg','.png','.svg','.gif'");

                string fileExtension = Base64Decoder.GetExtension(file);
                string uniqueName = Guid.NewGuid().ToString() + "." + fileExtension;

                string fileContent = Base64Decoder.GetContent(file);

                byte[] fileBytes = Convert.FromBase64String(fileContent);


                return fileBytes;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }


        public static async Task<string> WriteBytes(byte[] fileBytes,string file, string folder)
        {
            try
            {

                PathRegistry pathRegistry = PathRegistry.GetInstance();

                string folderPath = Path.Combine(pathRegistry.WebRootPath, folder);

                string uniqueName = GetUniqueName(file);

                string fullPath = Path.GetFullPath(Path.Combine(folderPath, uniqueName));

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                await File.WriteAllBytesAsync(fullPath, fileBytes);


                return Path.Combine(folder, uniqueName).Replace("\\","/");

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static void Delete(String file)
        {
            PathRegistry pathRegistry = PathRegistry.GetInstance();

            string fullPath = Path.GetFullPath(Path.Combine(pathRegistry.WebRootPath, file));

            if (!File.Exists(fullPath)) throw new Exception("File does not exists");

            File.Delete(fullPath);
        }


        private static string GetUniqueName(string file)
        {

            string fileExtension = Base64Decoder.GetExtension(file);
            
            string uniqueName = Guid.NewGuid().ToString() + "." + fileExtension;


            return uniqueName;
        }
    }
}
