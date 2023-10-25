using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MTGCardsAPI.Models;

namespace MTGCardsAPI.Services.CardTypeService
{
    public interface ICardTypeService
    {
        Task<ServiceResponse<List<CardTypeDTO>>> GetAllTypes();
        Task<ServiceResponse<List<CardTypeDTO>>> GetTypesByName(string name);
        Task<ServiceResponse<List<CardTypeDTO>>> CreateCardType(CardTypeDTO request);
        Task<ServiceResponse<CardTypeDTO>> EditCardType(int id, CardTypeDTO request);
        Task<ServiceResponse<List<CardTypeDTO>>> RemoveCardType(int id);

    }
}
