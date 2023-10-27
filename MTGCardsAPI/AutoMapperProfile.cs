using AutoMapper;

namespace MTGCardsAPI
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CardType, CardTypeDTO>();
            CreateMap<CardTypeDTO, CardType>();
        }
    }
}
