﻿using AutoMapper;
using MTGCardsAPI.Models;

namespace MTGCardsAPI.Services.CardService
{
    public class CardService : ICardService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CardService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<CardResponseDTO>> CreateCard(CardRequestDTO request)
        {
            var response = new ServiceResponse<CardResponseDTO>();

            Card newCard = new Card
            {
                Name = request.Name,
                ImageURL = request.ImageURL,
                Colours = new List<Colour>(),
                Abilities = new List<Ability>(),
                RulesText = request.RulesText,
                FlavourText = request.FlavourText,
                Power = request.Power,
                Toughness = request.Toughness,
                Type = new List<CardType>()
            };

            foreach (var colour in request.Colours)
            {
                var findColour = await _context.Colours.FirstOrDefaultAsync(c => c.Id == colour);
                newCard.Colours.Add(findColour);
            };

            foreach(var ability in request.Abilities) 
            {
                var findAbility = await _context.Abilities.FirstOrDefaultAsync(a => a.Id == ability);
                newCard.Abilities.Add(findAbility);
            }

            var findSet = await _context.Sets.FirstOrDefaultAsync(s => s.Id == request.Set);
            newCard.Set = findSet;

            foreach (var type in request.Type)
            {
                var findType = await _context.CardTypes.FirstOrDefaultAsync(a => a.Id == type);
                newCard.Type.Add(findType);
            }


            _context.Cards.Add(newCard);
            var saveCount = await _context.SaveChangesAsync();

            if (saveCount > 0)
            {
                response.Data = new CardResponseDTO
                {
                    Id = newCard.Id,
                    Name = newCard.Name,
                    ImageURL = newCard.ImageURL,
                    Colours = newCard.Colours.Select(c => c.Name).ToList(),
                    Abilities = newCard.Abilities.Select(c => c.Name).ToList(),
                    RulesText = newCard.RulesText,
                    FlavourText = newCard.FlavourText,
                    Power = newCard.Power,
                    Toughness = newCard.Toughness,
                    Set = newCard.Set.Name,
                    Type = newCard.Type.Select(c => c.Name).ToList()
                };
                response.Success = true;
                response.Message = "Card was successfully created.";

                return response;
            }
            else
            {
                response.Data = new CardResponseDTO();
                response.Success = false;
                response.Message = "Card was not created.";
            }

            return response;
        }

        public Task<ServiceResponse<Card>> EditCard(int id, CardRequestDTO request)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<List<CardResponseDTO>>> GetAllCards(int page)
        {
            //Search and pagination
            var pageResults = 5f;
            var allCards = await GetAll();
            var pageCount = Math.Ceiling(allCards.Count() / pageResults);
            var paginateCards = allCards
                                .Skip((page - 1) * (int)pageResults)
                                .Take((int)pageResults)
                                .ToList();

            //Response
            var response = new ServiceResponse<List<CardResponseDTO>> { Data = new List<CardResponseDTO>() };
            foreach (var card in paginateCards)
            {
                response.Data.Add(MapCardToResponseDTO(card));
            };              
            response.Pages = (int)pageCount;
            response.CurrentPage = page;

            return response;
        }

        public async Task<ServiceResponse<List<CardResponseDTO>>> GetCardsByAbility(string ability, int page)
        {
            //Search and pagination
            var pageResults = 5f;

            var allCards = await GetAll();
            var findAbility = _context.Abilities.Where(a => a.Name.ToLower() == ability.ToLower()).FirstOrDefault();
            var findCardsByAbility = _context.Cards
                                    .Where(c => c.Abilities.Contains(findAbility))
                                    .ToList();

            var pageCount = Math.Ceiling(findCardsByAbility.Count() / pageResults);
            var paginateCards = findCardsByAbility
                                .Skip((page - 1) * (int)pageResults)
                                .Take((int)pageResults)
                                .ToList();

            //Response
            var response = new ServiceResponse<List<CardResponseDTO>> { Data = new List<CardResponseDTO>() };

            if (findCardsByAbility.Count > 0)
            {
                foreach (var card in paginateCards)
                {
                    response.Data.Add(MapCardToResponseDTO(card));
                };
            }
            else
            {
                response.Data = new List<CardResponseDTO>();
                response.Message = "No cards found";
            }
            response.Pages = (int)pageCount;
            response.CurrentPage = page;

            return response;
        }

        public Task<ServiceResponse<List<Card>>> GetCardsByColour(string colour, int page)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<List<Card>>> GetCardsByFlavourText(string flavourText, int page)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse<List<CardResponseDTO>>> GetCardsByName(string name, int page)
        {
            //Search and pagination
            var pageResults = 5f;
            var findCards = await _context.Cards
                            .Include(c => c.Colours)
                            .Include(c => c.Abilities)
                            .Include(c => c.Set)
                            .Include(c => c.Type)
                            .Where(c => c.Name.ToLower()
                            .Contains(name.ToLower())).ToListAsync();

            var pageCount = Math.Ceiling(findCards.Count() / pageResults);
            var paginateCards = findCards
                                .Skip((page - 1) * (int)pageResults)
                                .Take((int)pageResults)
                                .ToList();

            //Response
            var response = new ServiceResponse<List<CardResponseDTO>> { Data = new List<CardResponseDTO>() };

            if (findCards.Count > 0)
            {
                foreach (var card in paginateCards)
                {    
                    response.Data.Add(MapCardToResponseDTO(card));
                };
            }
            else
            {
                response.Data = new List<CardResponseDTO>();
                response.Message = "No cards found";
            }
            response.Pages = (int)pageCount;
            response.CurrentPage = page;

            return response;
        }

        public Task<ServiceResponse<List<Card>>> GetCardsByPower(int power, int page)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<List<Card>>> GetCardsByRulesText(string rulesText, int page)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<List<Card>>> GetCardsBySet(string setName, int page)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<List<Card>>> GetCardsByToughness(int toughness, int page)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<List<Card>>> GetCardsByType(string typeName, int page)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResponse<List<Card>>> RemoveCard(int id)
        {
            throw new NotImplementedException();
        }

        private async Task<List<Card>> GetAll()
        {
            return await _context.Cards
                .Include(c => c.Colours)
                .Include(c => c.Abilities)
                .Include(c => c.Set)
                .Include(c => c.Type)
                .ToListAsync();
        }

        private async Task<ServiceResponse<Card>> FindCardById(int id)
        {
            var response = new ServiceResponse<Card>();
            var card = await _context.Cards.FindAsync(id);

            if (card == null)
            {
                response.Success = false;
                response.Message = "Card was not found.";
            }
            else
            {
                response.Data = card;
            }
            return response;
        }

        private CardResponseDTO MapCardToResponseDTO(Card card)
        {
            CardResponseDTO dto = new CardResponseDTO
            {
                Id = card.Id,
                Name = card.Name,
                ImageURL = card.ImageURL,
                Colours = card.Colours.Select(c => c.Name).ToList(),
                Abilities = card.Abilities.Select(c => c.Name).ToList(),
                RulesText = card.RulesText,
                FlavourText = card.FlavourText,
                Power = card.Power,
                Toughness = card.Toughness,
                Set = card.Set.Name,
                Type = card.Type.Select(c => c.Name).ToList(),
            };
            return dto;
        }
    }
}
