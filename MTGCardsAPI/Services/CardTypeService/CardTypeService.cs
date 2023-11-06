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

        public async Task<ServiceResponse<List<CardTypeDTO>>> GetAllTypes(int page, float resultsPerPage)
        {
            var allCardTypes = await _context.CardTypes.ToListAsync();
            var pageCount = PageCount(allCardTypes, resultsPerPage);
            var paginateCardTypes = PaginateTypes(page, resultsPerPage, allCardTypes);
            
            var mapDto = paginateCardTypes.Select(pct => _mapper.Map<CardTypeDTO>(pct));
            
            var response = new ServiceResponse<List<CardTypeDTO>> { Data = new List<CardTypeDTO>() };
            
            response.Data.AddRange(mapDto);
            response.Pages = (int)pageCount;
            response.CurrentPage = page;

            return response;
        }

        public async Task<ServiceResponse<List<CardTypeDTO>>> GetTypesByName(string name, int page, float resultsPerPage)
        {
            var response = new ServiceResponse<List<CardTypeDTO>> { Data = new List<CardTypeDTO>() };
            var searchResult = await _context.CardTypes
                        .Where(ct => ct.Name.ToLower()
                        .Contains(name.ToLower()))
                        .ToListAsync();


            if (searchResult.Count > 0)
            {
                var pageCount = PageCount(searchResult, resultsPerPage);
                var paginateCardTypes = PaginateTypes(page, resultsPerPage, searchResult);

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
                response.Message = "Cardtype was successfully created.";
            }
            else 
            {
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
                response.Message = "Cardtype was successfully deleted.";
            }
            else
            {
                response.Success = searchResult.Success;
                response.Message = searchResult.Message;
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

        private List<CardType> PaginateTypes(int page, float resultsPerPage, List<CardType> types)
        {
            return types.Skip((page - 1) * (int)resultsPerPage)
                        .Take((int)resultsPerPage)
                        .ToList();
        }

        private double PageCount(List<CardType> types, float resultsPerPage)
        {
            return Math.Ceiling(types.Count() / resultsPerPage);
        }
    }
}
