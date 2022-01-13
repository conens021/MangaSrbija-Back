namespace MangaSrbija.BLL.helpers
{
    public class PathRegistry
    {
        private static PathRegistry _instance;

        public string ContentRootPath { get; private set; }

        public string WebRootPath { get; private set; }

        private PathRegistry(string contentRootPath, string webRootPath)
        {
            ContentRootPath = contentRootPath;
            WebRootPath = webRootPath;
        }

        public static PathRegistry GetInstance(string contentRootPath = null, string webRootPath = null) =>
            _instance = _instance ?? new PathRegistry(contentRootPath, webRootPath);

    }
}
