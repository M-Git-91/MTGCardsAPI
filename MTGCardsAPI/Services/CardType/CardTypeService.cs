using Azure.Core;
using Microsoft.EntityFrameworkCore;

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
            var findType = await FindCardTypeById(id);
            
            if (findType.Success == false)
            {
                response.Success = false;
                response.Message = findType.Message;
            }
            else
            {
                var updateType = findType.Data;

                updateType.Name = request.Name;
                updateType.Cards = request.Cards;

                response.Data = updateType;
            }

            return response;
        }

        public async Task<ServiceResponse<CardType>> FindCardTypeById(int id)
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
            var response = new ServiceResponse<List<CardType>>
            {
                Data = await _context.CardTypes
                        .Where(ct => ct.Name.ToLower()
                        .Contains(name.ToLower()))
                        .ToListAsync()
            };
            return response;
        }

        public async Task<ServiceResponse<List<CardType>>> RemoveCardType(int id)
        {
            var response = new ServiceResponse<List<CardType>>();
            var searchResult = FindById(id).Result;

            _context.CardTypes.Remove(searchResult);
            await _context.SaveChangesAsync();

            response.Data = await GetAll();
            return response;
        }

        private async Task<List<CardType>> GetAll() 
        {
            return await _context.CardTypes.ToListAsync();
        }
        /*
        private async Task<CardType> FindById(int id)
        {
            return await _context.CardTypes.FindAsync(id);
        }*/


    }
}
