using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MTGCardsAPI.Services.CardService;
using System.ComponentModel.DataAnnotations;

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

        [HttpGet("searchall/{page}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ServiceResponse<List<CardResponseDTO>>>> GetAllCards([Required] float resultsPerPage, int page)
        {
            if (resultsPerPage == 0 || page == 0)
                return BadRequest();

            return Ok(await _cardService.GetAllCards(resultsPerPage, page));
        }

        [HttpGet("searchname/{name}/{page}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ServiceResponse<List<CardResponseDTO>>>> GetCardsByName(string name, [Required] float resultsPerPage, int page)
        {
            if (resultsPerPage == 0 || page == 0)
                return BadRequest();

            return Ok(await _cardService.GetCardsByName(name, resultsPerPage, page));
        }

        [HttpGet("searchcolour/{colour}/{page}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ServiceResponse<List<CardResponseDTO>>>> GetCardsByColour(string colour, [Required] float resultsPerPage, int page)
        {
            if (resultsPerPage == 0 || page == 0)
                return BadRequest();

            return Ok(await _cardService.GetCardsByColour(colour, resultsPerPage, page));
        }

        [HttpGet("searchability/{ability}/{page}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ServiceResponse<List<CardResponseDTO>>>> GetCardsByAbility(string ability, [Required] float resultsPerPage, int page)
        {
            if (resultsPerPage == 0 || page == 0)
                return BadRequest();

            return Ok(await _cardService.GetCardsByAbility(ability, resultsPerPage, page));
        }

        [HttpGet("searchrules/{rulesText}/{page}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ServiceResponse<List<CardResponseDTO>>>> GetCardsByRulesText(string rulesText, [Required] float resultsPerPage, int page)
        {
            if (resultsPerPage == 0 || page == 0)
                return BadRequest();

            return Ok(await _cardService.GetCardsByRulesText(rulesText, resultsPerPage, page));
        }

        [HttpGet("searchflavour/{flavourText}/{page}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ServiceResponse<List<CardResponseDTO>>>> GetCardsByFlavourText(string flavourText, [Required] float resultsPerPage, int page)
        {
            if (resultsPerPage == 0 || page == 0)
                return BadRequest();

            return Ok(await _cardService.GetCardsByFlavourText(flavourText, resultsPerPage, page));
        }

        [HttpGet("searchpower/{power}/{page}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ServiceResponse<List<CardResponseDTO>>>> GetCardsByPower(int power, [Required] float resultsPerPage, int page)
        {
            if (resultsPerPage == 0 || page == 0)
                return BadRequest();

            return Ok(await _cardService.GetCardsByPower(power, resultsPerPage, page));
        }

        [HttpGet("searchtoughness/{toughness}/{page}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<ServiceResponse<List<CardResponseDTO>>>> GetCardsByToughness(int toughness, [Required] float resultsPerPage, int page)
        {
            if (resultsPerPage == 0 || page == 0)
                return BadRequest();

            return Ok(await _cardService.GetCardsByToughness(toughness, resultsPerPage, page));
        }

        [HttpGet("searchset/{setName}/{page}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ServiceResponse<List<CardResponseDTO>>>> GetCardsBySet(string setName, [Required] float resultsPerPage, int page)
        {
            return Ok(await _cardService.GetCardsBySet(setName, resultsPerPage, page));
        }

        [HttpGet("searchtype/{typeName}/{page}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ServiceResponse<List<CardResponseDTO>>>> GetCardsByType(string typeName, [Required] float resultsPerPage, int page)
        {
            return Ok(await _cardService.GetCardsByType(typeName, resultsPerPage, page));
        }

        [HttpPost("create")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ServiceResponse<CardResponseDTO>>> CreateCard(CardRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(request);
            }
            return Ok(await _cardService.CreateCard(request));
        }

        [HttpPut("update/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ServiceResponse<CardResponseDTO>>> EditCard(int id, CardRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(request);
            }
            return Ok(await _cardService.EditCard(id, request));
        }

        [HttpDelete("delete/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ServiceResponse<bool>>> RemoveCard(int id)
        {     
            return Ok(await _cardService.RemoveCard(id));
        }
    }
}
