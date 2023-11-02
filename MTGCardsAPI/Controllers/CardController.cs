using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MTGCardsAPI.Services.CardService;

namespace MTGCardsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardController : ControllerBase
    {
        private readonly ICardService _cardService;

        public CardController(ICardService cardService)
        {
            _cardService = cardService;
        }

        [HttpGet("search/{page}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<ServiceResponse<List<CardResponseDTO>>>> GetAllCards(int page)
        {
            return Ok(await _cardService.GetAllCards(page));
        }

        [HttpPost("create")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<ServiceResponse<List<CardResponseDTO>>>> CreateCard(CardRequestDTO request)
        {
            return Ok(await _cardService.CreateCard(request));
        }
    }
}
