using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MTGCardsAPI.Models;

namespace MTGCardsAPI.Services.CardTypeService
{
    public interface ICardTypeService
    {
        Task<ServiceResponse<List<CardTypeDTO>>> GetAllTypes(int page, float resultsPerPage);
        Task<ServiceResponse<List<CardTypeDTO>>> GetTypesByName(string name, int page, float resultsPerPage);
        Task<ServiceResponse<CardTypeDTO>> CreateCardType(CardTypeDTO request);
        Task<ServiceResponse<CardTypeDTO>> EditCardType(int id, CardTypeDTO request);
        Task<ServiceResponse<CardTypeDTO>> RemoveCardType(int id);

    }
}
