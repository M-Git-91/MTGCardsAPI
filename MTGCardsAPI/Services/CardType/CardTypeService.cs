using Azure.Core;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using MTGCardsAPI.DTO;

namespace MTGCardsAPI.Services.CardTypeService

{
    public class CardTypeService : ICardTypeService
    {
        private readonly DataContext _context;

        public CardTypeService(DataContext context)
        {
            _context = context;
        }

        public async Task<ServiceResponse<CardType>> EditCardType(int id, CardType request)
        {
            var response = new ServiceResponse<CardType>();
            var searchResult = await FindCardTypeById(id);
            
            if (searchResult.Success == false)
            {
                response.Success = false;
                response.Message = searchResult.Message;
            }
            else
            {
                var toUpdateType = searchResult.Data;

                toUpdateType.Name = request.Name;
                toUpdateType.Cards = request.Cards;
                _context.Update(toUpdateType);
                _context.SaveChanges();

                response.Data = toUpdateType;
            }

            return response;
        }

        public async Task<ServiceResponse<List<CardType>>> GetAllTypes()
        {
            var response = new ServiceResponse<List<CardType>>
            {
                Data = await GetAll(),
            }; 
            return response;
        }

        public async Task<ServiceResponse<List<CardType>>> GetTypesByName(string name)
        {
            var response = new ServiceResponse<List<CardType>>();
            var searchResult = await _context.CardTypes
                        .Where(ct => ct.Name.ToLower()
                        .Contains(name.ToLower()))
                        .ToListAsync();

            if (searchResult.Count > 0)
            {
                response.Data = searchResult;
            }
            else
            {
                response.Data = new List<CardType>();
                response.Success = false;
                response.Message = "No Cardtypes found";
            }

            return response;
        }

        public async Task<ServiceResponse<List<CardType>>> CreateCardType(CardType request)
        {
            var response = new ServiceResponse<List<CardType>>();
            _context.CardTypes.Add(request);
            var saveCount = await _context.SaveChangesAsync();

            if (saveCount > 0)
            {
                response.Data = await GetAll();
            }
            else 
            {
                response.Data = new List<CardType>();
                response.Success = false;
                response.Message = "Cardtype was not created.";
            }

            return response;
        }

        public async Task<ServiceResponse<List<CardType>>> RemoveCardType(int id)
        {
            var response = new ServiceResponse<List<CardType>>();
            var searchResult = await FindCardTypeById(id);

            if (searchResult.Data != null)
            {
                _context.CardTypes.Remove(searchResult.Data);
                await _context.SaveChangesAsync();

                response.Data = await GetAll();
            }
            else
            {
                response.Success = searchResult.Success;
                response.Message = searchResult.Message;
            }

            return response;
        }

        private async Task<List<CardType>> GetAll() 
        {
            return await _context.CardTypes.ToListAsync();
        }

        private async Task<ServiceResponse<CardType>> FindCardTypeById(int id)
        {
            var response = new ServiceResponse<CardType>();
            var card = await _context.CardTypes.FindAsync(id);

            if (card == null)
            {
                response.Success = false;
                response.Message = "Cardtype was not found.";
            }
            else
            {
                response.Data = card;
            }
            return response;
        }
    }
}
