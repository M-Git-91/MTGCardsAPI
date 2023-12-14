using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using MTGCardsAPI.Models;
using MTGCardsAPI.Services.CardTypeService;
using System.ComponentModel.DataAnnotations;

namespace MTGCardsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardTypeController : ControllerBase
    {
        private readonly ICardTypeService _typeService;

        public CardTypeController(ICardTypeService typeService)
        {
            _typeService = typeService;
        }

        [HttpGet("search/{page}")]
        [ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(404)]
        public async Task<ActionResult<ServiceResponse<List<CardTypeDTO>>>> GetAllTypes(int page, [Required] float resultsPerPage)
        {
            if (resultsPerPage == 0 || page == 0)
                return BadRequest();

            var response = await _typeService.GetAllTypes(page, resultsPerPage);

            if (response.Data.Count == 0)
                return NotFound(response);

            return Ok(response);
        }


        [HttpGet("search/{name}/{page}")]
        [ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(404)]
        public async Task<ActionResult<ServiceResponse<List<CardTypeDTO>>>> GetTypesByName(string name, int page, [Required] float resultsPerPage) 
        {
            if (resultsPerPage == 0 || page == 0)
                return BadRequest();

            var response = await _typeService.GetTypesByName(name, page, resultsPerPage);

            if (response.Data.Count == 0)
                return NotFound(response);

            return Ok(response);
        }


        [HttpPost("create"), Authorize]
        [ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(401), ProducesResponseType(500)]
        public async Task<ActionResult<ServiceResponse<CardTypeDTO>>> CreateCardType(CardTypeDTO request) 
        {
            if (!ModelState.IsValid)
                return BadRequest(request);

            var response = await _typeService.CreateCardType(request);

            if (response.Success == false)
                return StatusCode(500, response);

            return Ok(response);
        }


        [HttpPut("edit/{id}"), Authorize]
        [ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(401), ProducesResponseType(404), ProducesResponseType(500)]
        public async Task<ActionResult<ServiceResponse<CardTypeDTO>>> EditCardType(int id, CardTypeDTO request) 
        {
            if (!ModelState.IsValid)
                return BadRequest(request);

            var response = await _typeService.EditCardType(id, request);

            if (response.Success == false && response.Data == null)
                return NotFound(response);

            if (response.Success == false && response.Data != null)
                return StatusCode(500, response);

            return Ok(response);
        }

        [HttpDelete("delete/{id}"), Authorize]
        [ProducesResponseType(200), ProducesResponseType(401)]
        public async Task<ActionResult<ServiceResponse<CardTypeDTO>>> RemoveCardType(int id) 
        {
            var response = await _typeService.RemoveCardType(id);

            if (response.Success == false)
                return NotFound(response);

            return Ok(response);
        }
    }
}
