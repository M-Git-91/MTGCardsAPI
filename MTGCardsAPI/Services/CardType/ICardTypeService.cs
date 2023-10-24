using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MTGCardsAPI.Models;

namespace MTGCardsAPI.Services.CardTypeService
{
    public interface ICardTypeService
    {
        Task<ServiceResponse<List<CardType>>> GetAllTypes();
        Task<ServiceResponse<List<CardType>>> GetTypesByName(string name);
        Task<ServiceResponse<List<CardType>>> CreateCardType(CardType request);
        Task<ServiceResponse<CardType>> EditCardType(int id, CardType request);
        Task<ServiceResponse<List<CardType>>> RemoveCardType(int id);

    }
}
