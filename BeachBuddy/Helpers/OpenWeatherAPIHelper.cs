namespace BeachBuddy.Helpers
{
    public class OpenWeatherAPIHelper
    {
        public static string GetIconUrl(string iconCode)
        {
            return $"http://openweathermap.org/img/wn/{iconCode}@2x.png";
        }
    }
}