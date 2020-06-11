using System;

namespace BeachBuddy.Enums
{
    public static class Extensions
    {
        
        public static double GetUVProtectionConstant(this SkinType skinType)
        {
            return skinType switch
            {
                SkinType.One => 2.5,
                SkinType.Two => 3,
                SkinType.Three => 4,
                SkinType.Four => 5,
                SkinType.Five => 8,
                SkinType.Six => 15,
                _ => throw new ArgumentOutOfRangeException(nameof(skinType), skinType, null)
            };
        }

        public static double GetSafeExposureTime(this SkinType skinType, double uvIndex, int spf = 1)
        {
            return spf * (200 * skinType.GetUVProtectionConstant()) / (3 * uvIndex); 
        }
        
    }
}