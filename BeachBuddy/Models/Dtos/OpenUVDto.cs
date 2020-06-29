using Microsoft.VisualBasic.CompilerServices;

namespace BeachBuddy.Models.Dtos
{
    public class OpenUVDto
    {
        public Result result { get; set; }
    }
    
    public class Result
    {
        public double uv { get; set; }
        
        public string uv_time { get; set; }
        
        public double uv_max { get; set; }
        
        public string uv_max_time { get; set; }
        
        public SafeExposureTimeDto safe_exposure_time { get; set; }
    }

    public class SafeExposureTimeDto
    {
        public IntegerType st1 { get; set; }
        public IntegerType st2 { get; set; }
        public IntegerType st3 { get; set; }
        public IntegerType st4 { get; set; }
        public IntegerType st5 { get; set; }
        public IntegerType st6 { get; set; }
    }
}