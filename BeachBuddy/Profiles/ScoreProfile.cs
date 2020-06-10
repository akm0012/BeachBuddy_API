using AutoMapper;

namespace BeachBuddy.Profiles
{
    public class ScoreProfile : Profile
    {
        public ScoreProfile()
        {
            CreateMap<Entities.Score, Models.ScoreDto>();
            
            CreateMap<Models.UpdateScoreDto, Entities.Score>();
        }
    }
}