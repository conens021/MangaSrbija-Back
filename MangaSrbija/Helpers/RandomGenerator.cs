using System.Text;

namespace MangaSrbija.Presentation.Helpers
{
    public static class RandomGenerator
    {
        public static string Get6DigitsCode()
        { 
            Random random = new Random();
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < 6; i++)
            {
                sb.Append(random.Next(1,10).ToString());
            }

            return sb.ToString();
        }
    }
}
