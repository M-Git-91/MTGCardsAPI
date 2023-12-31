﻿using AutoMapper;
using Azure;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic.FileIO;
using MTGCardsAPI.Models;
using System.Drawing;
using System.Linq;

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
                if (findColour == null)
                {
                    response.Success = false;
                    response.Message = $"Colour with Id:{colour} was not found";
                    return response;
                }
                newCard.Colours.Add(findColour);
            };

            foreach(var ability in request.Abilities) 
            {
                var findAbility = await _context.Abilities.FirstOrDefaultAsync(a => a.Id == ability);
                if (findAbility == null)
                {
                    response.Success = false;
                    response.Message = $"Ability with Id:{ability} was not found";
                    return response;
                }
                newCard.Abilities.Add(findAbility);
            }

            var findSet = await _context.Sets.FirstOrDefaultAsync(s => s.Id == request.Set);
            if (findSet == null)
            {
                response.Success = false;
                response.Message = $"Set with Id:{request.Set} was not found";
                return response;
            }
            newCard.Set = findSet;

            foreach (var type in request.Type)
            {
                var findType = await _context.CardTypes.FirstOrDefaultAsync(a => a.Id == type);
                if (findType == null)
                {
                    response.Success = false;
                    response.Message = $"Type with Id:{type} was not found";
                    return response;
                }
                newCard.Type.Add(findType);
            }


            _context.Cards.Add(newCard);
            var saveCount = await _context.SaveChangesAsync();

            var cardResponseDTO = new CardResponseDTO
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

            if (saveCount > 0)
            {
                response.Data = cardResponseDTO;
                response.Success = true;
                response.Message = "Card was successfully created.";

                return response;
            }
            else
            {
                response.Data = cardResponseDTO;
                response.Success = false;
                response.Message = "Card was not created.";
            }

            return response;
        }

        public async Task<ServiceResponse<CardResponseDTO>> EditCard(int id, CardRequestDTO request)
        {
            var response = new ServiceResponse<CardResponseDTO>();
            var searchResult = await FindCardById(id);
            if (searchResult.Success == false)
            {
                response.Success = false;
                response.Message = searchResult.Message;
                return response;
            }
            else
            {
                searchResult.Data.Name = request.Name;
                searchResult.Data.ImageURL = request.ImageURL;

                searchResult.Data.Colours.Clear();
                foreach (var colour in request.Colours)
                {
                    var findColour = await _context.Colours.FirstOrDefaultAsync(c => c.Id == colour);
                    if (findColour == null)
                    {
                        response.Success = false;
                        response.Message = $"Colour with Id:{colour} was not found";
                        return response;
                    }
                    searchResult.Data.Colours.Add(findColour);
                };

                searchResult.Data.Abilities.Clear();
                foreach (var ability in request.Abilities)
                {
                    var findAbility = await _context.Abilities.FirstOrDefaultAsync(c => c.Id == ability);
                    if (findAbility == null)
                    {
                        response.Success = false;
                        response.Message = $"Ability with Id:{ability} was not found";
                        return response;
                    }
                    searchResult.Data.Abilities.Add(findAbility);
                };

                searchResult.Data.RulesText = request.RulesText;
                searchResult.Data.FlavourText = request.FlavourText;
                searchResult.Data.Power = request.Power;
                searchResult.Data.Toughness = request.Toughness;

                var findSet = await _context.Sets.FirstOrDefaultAsync(s => s.Id == request.Set);
                if (findSet == null)
                {
                    response.Success = false;
                    response.Message = $"Set with Id:{request.Set} was not found";
                    return response;
                }
                searchResult.Data.Set = findSet;

                searchResult.Data.Type.Clear();
                foreach (var type in request.Type)
                {
                    var findType = await _context.CardTypes.FirstOrDefaultAsync(a => a.Id == type);
                    if (findType == null)
                    {
                        response.Success = false;
                        response.Message = $"Type with Id:{type} was not found";
                        return response;
                    }

                    searchResult.Data.Type.Add(findType);
                };

                _context.Cards.Update(searchResult.Data);
                var saveCount = await _context.SaveChangesAsync();

                if (saveCount > 0)
                {
                    response.Data = MapCardToResponseDTO(searchResult.Data);
                    response.Success = true;
                    response.Message = "Card was successfully updated.";
                    return response;
                }
                response.Data = MapCardToResponseDTO(searchResult.Data);
                response.Success = false;
                response.Message = "Card was not updated.";
                return response;     
            }
        }

        public async Task<ServiceResponse<List<CardResponseDTO>>> GetAllCards(float resultsPerPage, int page)
        {           
            var allCards = await GetAll();
            var pageCount = PageCount(allCards, resultsPerPage);
            var paginatedCards = PaginateCards(page, resultsPerPage, allCards);

            return GETResponse(allCards, paginatedCards, pageCount, page);
        }

        public async Task<ServiceResponse<List<CardResponseDTO>>> GetCardsByAbility(string ability, float resultsPerPage, int page)
        {
            var allCards = await GetAll();
            var findAbility = _context.Abilities.Where(a => a.Name.ToLower() == ability.ToLower())
                            .FirstOrDefault();

            if (findAbility == null)
            {
                var response = new ServiceResponse<List<CardResponseDTO>>
                {
                    Data = new List<CardResponseDTO>(),
                    Success = false,
                    Message = "Ability not found."
                };
                return response;
            }

            var cardsByAbility = _context.Cards
                                .Where(c => c.Abilities.Contains(findAbility))
                                .ToList();

            var paginatedCards = PaginateCards(page, resultsPerPage, cardsByAbility);
            var pageCount = PageCount(cardsByAbility, resultsPerPage);

            return GETResponse(cardsByAbility, paginatedCards, pageCount, page);
        }

        public async Task<ServiceResponse<List<CardResponseDTO>>> GetCardsByColour(string colour, float resultsPerPage,int page)
        {
            var allCards = await GetAll();
            var findColour = _context.Colours
                            .Where(a => a.Name.ToLower() == colour.ToLower())
                            .FirstOrDefault();
            if (findColour == null)
            {
                var response = new ServiceResponse<List<CardResponseDTO>>
                {
                    Data = new List<CardResponseDTO>(),
                    Success = false,
                    Message = "Colour not found."
                };
                return response;
            }

            var cardsByColour = _context.Cards
                            .Where(c => c.Colours.Contains(findColour))
                            .ToList();

            var paginatedCards = PaginateCards(page, resultsPerPage, cardsByColour);
            var pageCount = PageCount(cardsByColour, resultsPerPage);

            return GETResponse(cardsByColour, paginatedCards, pageCount, page);
        }

        public async Task<ServiceResponse<List<CardResponseDTO>>> GetCardsByFlavourText(string flavourText, float resultsPerPage, int page)
        {
            var allCards = await GetAll();
            var cardsByFlavourText = allCards
                            .Where(c => c.FlavourText.ToLower().Contains(flavourText.ToLower()))
                            .ToList();

            var pageCount = PageCount(cardsByFlavourText, resultsPerPage);
            var paginatedCards = PaginateCards(page, resultsPerPage, cardsByFlavourText);

            return GETResponse(cardsByFlavourText, paginatedCards, pageCount, page);
        }

        public async Task<ServiceResponse<List<CardResponseDTO>>> GetCardsByName(string name, float resultsPerPage,int page)
        {
            var allCards = await GetAll();
            var cardsByName = allCards
                            .Where(c  => c.Name.ToLower().Contains(name.ToLower()))
                            .ToList();

            var pageCount = PageCount(cardsByName, resultsPerPage);
            var paginatedCards = PaginateCards(page, resultsPerPage, cardsByName);

            return GETResponse(cardsByName, paginatedCards, pageCount, page);
        }

        public async Task<ServiceResponse<List<CardResponseDTO>>> GetCardsByPower(int power, float resultsPerPage, int page)
        {
            var allCards = await GetAll();
            var cardsByPower = allCards
                            .Where(c => c.Power == power)
                            .ToList();

            var pageCount = PageCount(cardsByPower, resultsPerPage);
            var paginatedCards = PaginateCards(page, resultsPerPage, cardsByPower);

            return GETResponse(cardsByPower, paginatedCards, pageCount, page);
        }

        public async Task<ServiceResponse<List<CardResponseDTO>>> GetCardsByRulesText(string rulesText, float resultsPerPage, int page)
        {
            var allCards = await GetAll();
            var cardsByRulesText = allCards
                            .Where(c => c.RulesText.ToLower().Contains(rulesText.ToLower()))
                            .ToList();

            var pageCount = PageCount(cardsByRulesText, resultsPerPage);
            var paginatedCards = PaginateCards(page, resultsPerPage, cardsByRulesText);

            return GETResponse(cardsByRulesText, paginatedCards, pageCount, page);
        }

        public async Task<ServiceResponse<List<CardResponseDTO>>> GetCardsBySet(string setName, float resultsPerPage, int page)
        {
            var allCards = await GetAll();
            var findSet = _context.Sets
                            .Where(a => a.Name.ToLower().Contains(setName.ToLower()))
                            .FirstOrDefault();

            if (findSet == null)
            {
                var response = new ServiceResponse<List<CardResponseDTO>>
                {
                    Data = new List<CardResponseDTO>(),
                    Success = false,
                    Message = "Set not found."
                };
                return response;
            }

            var cardsBySet = _context.Cards
                            .Where(c => c.Set.Name.Contains(findSet.Name))
                            .ToList();

            var paginatedCards = PaginateCards(page, resultsPerPage, cardsBySet);
            var pageCount = PageCount(cardsBySet, resultsPerPage);

            return GETResponse(cardsBySet, paginatedCards, pageCount, page);
        }

        public async Task<ServiceResponse<List<CardResponseDTO>>> GetCardsByToughness(int toughness, float resultsPerPage, int page)
        {
            var allCards = await GetAll();
            var cardsByToughness = allCards
                            .Where(c => c.Toughness == toughness)
                            .ToList();

            var pageCount = PageCount(cardsByToughness, resultsPerPage);
            var paginatedCards = PaginateCards(page, resultsPerPage, cardsByToughness);

            return GETResponse(cardsByToughness, paginatedCards, pageCount, page);
        }

        public async Task<ServiceResponse<List<CardResponseDTO>>> GetCardsByType(string typeName, float resultsPerPage, int page)
        {
            var allCards = await GetAll();
            var findType = _context.CardTypes
                            .Where(a => a.Name.ToLower() == typeName.ToLower())
                            .FirstOrDefault();

            if (findType == null)
            {
                var response = new ServiceResponse<List<CardResponseDTO>>
                {
                    Data = new List<CardResponseDTO>(),
                    Success = false,
                    Message = "Card type not found."
                };
                return response;
            }

            var cardsByType = _context.Cards
                            .Where(c => c.Type.Contains(findType))
                            .ToList();

            var paginatedCards = PaginateCards(page, resultsPerPage, cardsByType);
            var pageCount = PageCount(cardsByType, resultsPerPage);

            return GETResponse(cardsByType, paginatedCards, pageCount, page);
        }

        public async Task<ServiceResponse<CardResponseDTO>> RemoveCard(int id)
        {
            var response = new ServiceResponse<CardResponseDTO>();
            var searchResult = await FindCardById(id);
            if (searchResult.Success == false)
            {
                response.Success = false;
                response.Message = searchResult.Message;
                return response;
            }

            _context.Cards.Remove(searchResult.Data);
            await _context.SaveChangesAsync();

            response.Success = true;
            response.Message = "Card was successfully deleted.";
            return response;
            
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
            var card = await _context.Cards
                .Include(c => c.Colours)
                .Include(c => c.Abilities)
                .Include(c => c.Set)
                .Include(c => c.Type)
                .FirstOrDefaultAsync(c => c.Id == id);

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

        private List<Card> PaginateCards(int page,float resultsPerPage, List<Card> cards)
        {
            return cards.Skip((page - 1) * (int)resultsPerPage)
                        .Take((int)resultsPerPage)
                        .ToList();
        }

        private double PageCount(List<Card> cards, float resultsPerPage)
        {
            return Math.Ceiling(cards.Count() / resultsPerPage);
        }

        private ServiceResponse<List<CardResponseDTO>> GETResponse(List<Card> cardsByX, List<Card> paginatedCards, double pageCount, int page)
        {
            var response = new ServiceResponse<List<CardResponseDTO>> { Data = new List<CardResponseDTO>() };

            if (cardsByX.Count > 0)
            {
                foreach (var card in paginatedCards)
                {
                    response.Data.Add(MapCardToResponseDTO(card));
                };
            }
            else
            {
                response.Data = new List<CardResponseDTO>();
                response.Success = false;
                response.Message = "No cards found";
            }
            response.Pages = (int)pageCount;
            response.CurrentPage = page;

            return response;
        }
    }
}
