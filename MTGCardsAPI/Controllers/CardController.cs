using Azure;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
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
        [ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(404)]
        public async Task<ActionResult<ServiceResponse<List<CardResponseDTO>>>> GetAllCards([Required] float resultsPerPage, int page)
        {
            if (resultsPerPage == 0 || page == 0)
                return BadRequest();

            var response = await _cardService.GetAllCards(resultsPerPage, page);

            if (response.Data.Count == 0)
                return NotFound(response);

            return Ok(response);
        }

        [HttpGet("searchname/{name}/{page}")]
        [ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(404)]
        public async Task<ActionResult<ServiceResponse<List<CardResponseDTO>>>> GetCardsByName(string name, [Required] float resultsPerPage, int page)
        {
            if (resultsPerPage == 0 || page == 0)
                return BadRequest();

            var response = await _cardService.GetCardsByName(name, resultsPerPage, page);

            if (response.Data.Count == 0)
                return NotFound(response);

            return Ok(response);
        }

        [HttpGet("searchcolour/{colour}/{page}")]
        [ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(404)]
        public async Task<ActionResult<ServiceResponse<List<CardResponseDTO>>>> GetCardsByColour(string colour, [Required] float resultsPerPage, int page)
        {
            if (resultsPerPage == 0 || page == 0)
                return BadRequest();

            var response = await _cardService.GetCardsByColour(colour, resultsPerPage, page);

            if (response.Data.Count == 0)
                return NotFound(response);

            return Ok(response);
        }

        [HttpGet("searchability/{ability}/{page}")]
        [ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(404)]
        public async Task<ActionResult<ServiceResponse<List<CardResponseDTO>>>> GetCardsByAbility(string ability, [Required] float resultsPerPage, int page)
        {
            if (resultsPerPage == 0 || page == 0)
                return BadRequest();

            var response = await _cardService.GetCardsByAbility(ability, resultsPerPage, page);
            
            if (response.Data.Count == 0)
                return NotFound(response);

            return Ok(response);
        }

        [HttpGet("searchrules/{rulesText}/{page}")]
        [ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(404)]
        public async Task<ActionResult<ServiceResponse<List<CardResponseDTO>>>> GetCardsByRulesText(string rulesText, [Required] float resultsPerPage, int page)
        {
            if (resultsPerPage == 0 || page == 0)
                return BadRequest();

            var response = await _cardService.GetCardsByRulesText(rulesText, resultsPerPage, page);

            if (response.Data.Count == 0)
                return NotFound(response);

            return Ok(response);
        }

        [HttpGet("searchflavour/{flavourText}/{page}")]
        [ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(404)]
        public async Task<ActionResult<ServiceResponse<List<CardResponseDTO>>>> GetCardsByFlavourText(string flavourText, [Required] float resultsPerPage, int page)
        {
            if (resultsPerPage == 0 || page == 0)
                return BadRequest();

            var response = await _cardService.GetCardsByFlavourText(flavourText, resultsPerPage, page);

            if (response.Data.Count == 0)
                return NotFound(response);

            return Ok(response);
        }

        [HttpGet("searchpower/{power}/{page}")]
        [ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(404)]
        public async Task<ActionResult<ServiceResponse<List<CardResponseDTO>>>> GetCardsByPower(int power, [Required] float resultsPerPage, int page)
        {
            if (resultsPerPage == 0 || page == 0)
                return BadRequest();

            var response = await _cardService.GetCardsByPower(power, resultsPerPage, page);

            if (response.Data.Count == 0)
                return NotFound(response);

            return Ok(response);
        }

        [HttpGet("searchtoughness/{toughness}/{page}")]
        [ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(404)]
        public async Task<ActionResult<ServiceResponse<List<CardResponseDTO>>>> GetCardsByToughness(int toughness, [Required] float resultsPerPage, int page)
        {
            if (resultsPerPage == 0 || page == 0)
                return BadRequest();

            var response = await _cardService.GetCardsByToughness(toughness, resultsPerPage, page);

            if (response.Data.Count == 0)
                return NotFound(response);

            return Ok(response);
        }

        [HttpGet("searchset/{setName}/{page}")]
        [ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(404)]
        public async Task<ActionResult<ServiceResponse<List<CardResponseDTO>>>> GetCardsBySet(string setName, [Required] float resultsPerPage, int page)
        {
            if (resultsPerPage == 0 || page == 0)
                return BadRequest();

            var response = await _cardService.GetCardsBySet(setName, resultsPerPage, page);

            if (response.Data.Count == 0)
                return NotFound(response);

            return Ok(response);
        }

        [HttpGet("searchtype/{typeName}/{page}")]
        [ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(404)]
        public async Task<ActionResult<ServiceResponse<List<CardResponseDTO>>>> GetCardsByType(string typeName, [Required] float resultsPerPage, int page)
        {
            if (resultsPerPage == 0 || page == 0)
                return BadRequest();

            var response = await _cardService.GetCardsByType(typeName, resultsPerPage, page);

            if (response.Data.Count == 0)
                return NotFound(response);

            return Ok(response);
        }

        [HttpPost("create"), Authorize]
        [ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(401), ProducesResponseType(404)]
        public async Task<ActionResult<ServiceResponse<CardResponseDTO>>> CreateCard(CardRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(request);

            var response = await _cardService.CreateCard(request);

            if (response.Success == false && response.Data != null)
                return StatusCode(500, response);

            if (response.Success == false)
                return NotFound(response);

            return Ok(response);
        }

        [HttpPut("update/{id}"), Authorize]
        [ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(401), ProducesResponseType(404), ProducesResponseType(500)]
        public async Task<ActionResult<ServiceResponse<CardResponseDTO>>> EditCard(int id, CardRequestDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(request);

            var response = await _cardService.EditCard(id, request);

            if (response.Success == false && response.Data != null)
                return StatusCode(500, response);

            if (response.Success == false)
                return NotFound(response);

            return Ok(response);
        }

        [HttpDelete("delete/{id}"), Authorize]
        [ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(401), ProducesResponseType(404)]
        public async Task<ActionResult<ServiceResponse<CardResponseDTO>>> RemoveCard(int id)
        {
            var response = await _cardService.RemoveCard(id);

            if (response.Success == false)
                return NotFound(response);

            return Ok(response);
        }
    }
}
