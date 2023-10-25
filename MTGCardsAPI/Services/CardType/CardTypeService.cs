using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using MTGCardsAPI.DTO;

namespace MTGCardsAPI.Services.CardTypeService

{
    public class CardTypeService : ICardTypeService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CardTypeService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<CardTypeDTO>> EditCardType(int id, CardTypeDTO request)
        {
            var response = new ServiceResponse<CardTypeDTO>();
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
                _context.Update(toUpdateType);
                _context.SaveChanges();

                response.Data.Name = toUpdateType.Name;
            }

            return response;
        }

        public async Task<ServiceResponse<List<CardTypeDTO>>> GetAllTypes()
        {
            var response = new ServiceResponse<List<CardTypeDTO>>
            {
                Data = await GetAll()
            }; 
            return response;
        }

        public async Task<ServiceResponse<List<CardTypeDTO>>> GetTypesByName(string name)
        {
            var response = new ServiceResponse<List<CardTypeDTO>>();
            var searchResult = await _context.CardTypes
                        .Where(ct => ct.Name.ToLower()
                        .Contains(name.ToLower()))
                        .ToListAsync();

            if (searchResult.Count > 0)
            {
                searchResult.Select(sr => _mapper.Map<CardTypeDTO>(sr));
                response.Data = new List<CardTypeDTO>();
                response.Data.AddRange(searchResult.Select(sr => _mapper.Map<CardTypeDTO>(sr)));
            }
            else
            {
                response.Data = new List<CardTypeDTO>();
                response.Success = false;
                response.Message = "No Cardtypes found";
            }

            return response;
        }

        public async Task<ServiceResponse<List<CardTypeDTO>>> CreateCardType(CardTypeDTO request)
        {
            var response = new ServiceResponse<List<CardTypeDTO>>();
            CardType newCardType = new CardType { Name = request.Name };

            _context.CardTypes.Add(newCardType);
            var saveCount = await _context.SaveChangesAsync();

            if (saveCount > 0)
            {
                response.Data = await GetAll();
            }
            else 
            {
                response.Data = new List<CardTypeDTO>();
                response.Success = false;
                response.Message = "Cardtype was not created.";
            }

            return response;
        }

        public async Task<ServiceResponse<List<CardTypeDTO>>> RemoveCardType(int id)
        {
            var response = new ServiceResponse<List<CardTypeDTO>>();
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

        private async Task<List<CardTypeDTO>> GetAll() 
        {
            var searchResult = await _context.CardTypes.ToListAsync();

            var response = new List<CardTypeDTO>();

            foreach (var cardType in searchResult)
            {
                CardTypeDTO dto = new CardTypeDTO
                {
                    Id = cardType.Id,
                    Name = cardType.Name,
                };
                response.Add(dto);
            }

            return response;
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
