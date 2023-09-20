namespace MangaSrbija.BLL.helpers
{
    public static class OrderBy
    {
        public static string Manga(string order)
        {
            switch (order.ToUpper())
            {
                case "AZ":
                    return "byMangaTitleAsc";
                case "ZA":
                    return "byMangaTitleDesc";
                case "BR":
                    return "byMangaRating";
                case "BV":
                    return "byMangaViews";
                case "RDA":
                    return "byMangaRelaeseDateAsc";
                case "RDD":
                    return "byMangaRelaeseDateDesc";

                default: return "";
            }
        }
    }
}
