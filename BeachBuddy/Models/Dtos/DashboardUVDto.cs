namespace BeachBuddy.Models.Dtos
{
    public class DashboardUVDto
    {
        public double uv { get; set; }
        
        public string uv_time { get; set; }
        
        public double uv_max { get; set; }
        
        public string uv_max_time { get; set; }
        
        public SafeExposureTimeDto safe_exposure_time { get; set; }
    }
}