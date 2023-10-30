using AutoMapper;

namespace MTGCardsAPI
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CardType, CardTypeDTO>().ReverseMap();
            CreateMap<AbilityDTO, Ability>().ReverseMap();
            CreateMap<ColourDTO, Colour>().ReverseMap();
        }
    }
}
