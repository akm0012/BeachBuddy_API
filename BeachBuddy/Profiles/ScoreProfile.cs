using AutoMapper;
using BeachBuddy.Models.Dtos.Score;

namespace BeachBuddy.Profiles
{
    public class ScoreProfile : Profile
    {
        public ScoreProfile()
        {
            CreateMap<Entities.Score, ScoreDto>();
            
            CreateMap<UpdateScoreDto, Entities.Score>();
        }
    }
}