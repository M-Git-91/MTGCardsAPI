using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Services;
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
                searchResult.Data.Name = request.Name;
                response.Data = _mapper.Map<CardTypeDTO>(searchResult.Data);

                _context.Update(searchResult.Data);
                _context.SaveChanges();
            }

            return response;
        }

        public async Task<ServiceResponse<List<CardTypeDTO>>> GetAllTypes(int page)
        {   
            var pageResults = 5f;
            var allCardTypes = await GetAll();
            var pageCount = Math.Ceiling(allCardTypes.Count() / pageResults);
            

            var paginateCardTypes = await _context.CardTypes
                                .Skip((page - 1) * (int)pageResults)
                                .Take((int)pageResults)
                                .ToListAsync();
            
            var mapDto = paginateCardTypes.Select(pct => _mapper.Map<CardTypeDTO>(pct));
            
            var response = new ServiceResponse<List<CardTypeDTO>> { Data = new List<CardTypeDTO>() };
            response.Data.AddRange(mapDto);
            response.Pages = (int)pageCount;
            response.CurrentPage = page;

            return response;
        }

        public async Task<ServiceResponse<List<CardTypeDTO>>> GetTypesByName(string name, int page)
        {
            var response = new ServiceResponse<List<CardTypeDTO>> { Data = new List<CardTypeDTO>() };
            var searchResult = await _context.CardTypes
                        .Where(ct => ct.Name.ToLower()
                        .Contains(name.ToLower()))
                        .ToListAsync();


            if (searchResult.Count > 0)
            {
                var pageResults = 5f;
                var pageCount = Math.Ceiling(searchResult.Count() / pageResults);

                var paginateCardTypes = searchResult
                                .Skip((page - 1) * (int)pageResults)
                                .Take((int)pageResults);

                var mapDto = paginateCardTypes.Select(sr => _mapper.Map<CardTypeDTO>(sr));
                response.Data.AddRange(mapDto);
                response.Pages = (int)pageCount;
                response.CurrentPage = page;
            }
            else
            {
                response.Data = new List<CardTypeDTO>();
                response.Success = false;
                response.Message = "No Cardtypes found";
            }

            return response;
        }

        public async Task<ServiceResponse<CardTypeDTO>> CreateCardType(CardTypeDTO request)
        {
            var response = new ServiceResponse<CardTypeDTO>();
            CardType newCardType = new CardType { Name = request.Name };

            _context.CardTypes.Add(newCardType);
            var saveCount = await _context.SaveChangesAsync();

            if (saveCount > 0)
            {
                response.Data = _mapper.Map<CardTypeDTO>(newCardType);
            }
            else 
            {
                response.Data = new CardTypeDTO();
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

                response.Data = new List<CardTypeDTO>();
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
