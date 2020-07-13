# BeachBuddy_API
A simple API that helps you have fun in the sun by monitoring the weather, manages a Beach List, and keeps track of a leader board. 

This is was my first .NET Core app used to help me learn: 
- Backend Development (obviously) 
- Working with Databases
- Push Notifications w/ Firebase 
- Twilio Integration 
- Hitting external APIs
- HTML scrapping (not proud of it) 
- Caching
- Docker

Note: In order to compile, you will need to create a class called, `APIKeys.cs` at the root of the project. Should look like this: 

```
namespace BeachBuddy
{
    public static partial class APIKeys
    {
        public const string AppSecretHeader = "<SOME_SECRET_HEADER>";
    
        public const string OpenWeatherApiKey = "<OPEN_WEATHER_API_KEY>";
        
        public const string OpenUVIndexApiKey = "<OPEN_UV_INDEX_KEY>";
 
        public const string TwilioAccoutSid = "<TWILIO_ACCOUNT_SID>";
        public const string TwilioAuthToken = "<TWILIO_AUTH_TOKEN>";
    }
}
```
