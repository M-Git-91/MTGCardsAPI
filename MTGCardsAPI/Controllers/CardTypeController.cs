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
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ServiceResponse<List<CardTypeDTO>>>> GetAllTypes(int page, [Required] float resultsPerPage)
        {
            if (resultsPerPage == 0 || page == 0)
                return BadRequest();

            return Ok(await _typeService.GetAllTypes(page, resultsPerPage));
        }


        [HttpGet("search/{name}/{page}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ServiceResponse<List<CardTypeDTO>>>> GetTypesByName(string name, int page, [Required] float resultsPerPage) 
        {
            if (resultsPerPage == 0 || page == 0)
                return BadRequest();

            return Ok(await _typeService.GetTypesByName(name, page, resultsPerPage));
        }


        [HttpPost("create"), Authorize]
        [ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(401)]
        public async Task<ActionResult<ServiceResponse<CardTypeDTO>>> CreateCardType(CardTypeDTO request) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(request);
            }    
            return Ok(await _typeService.CreateCardType(request));
        }


        [HttpPut("edit/{id}"), Authorize]
        [ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(401)]
        public async Task<ActionResult<ServiceResponse<CardTypeDTO>>> EditCardType(int id, CardTypeDTO request) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(request);
            }
            return Ok(await _typeService.EditCardType(id, request));
        }

        [HttpDelete("delete/{id}"), Authorize]
        [ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(401)]
        public async Task<ActionResult<ServiceResponse<bool>>> RemoveCardType(int id) 
        {   
            return Ok(await _typeService.RemoveCardType(id));
        }
    }
}
