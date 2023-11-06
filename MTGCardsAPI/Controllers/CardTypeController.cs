﻿using Microsoft.AspNetCore.Http;
using MTGCardsAPI.Models;
using MTGCardsAPI.Services.CardTypeService;

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
        public async Task<ActionResult<ServiceResponse<List<CardTypeDTO>>>> GetAllTypes(int page, float resultsPerPage)
        {
            return Ok(await _typeService.GetAllTypes(page, resultsPerPage));
        }


        [HttpGet("search/{name}/{page}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<ServiceResponse<List<CardTypeDTO>>>> GetTypesByName(string name, int page, float resultsPerPage) 
        {             
            return Ok(await _typeService.GetTypesByName(name, page, resultsPerPage));
        }


        [HttpPost("create")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ServiceResponse<CardTypeDTO>>> CreateCardType(CardTypeDTO request) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(request);
            }    
            return Ok(await _typeService.CreateCardType(request));
        }


        [HttpPut("edit/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ServiceResponse<CardTypeDTO>>> EditCardType(int id, CardTypeDTO request) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(request);
            }
            return Ok(await _typeService.EditCardType(id, request));
        }

        [HttpDelete("delete/{id}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<ServiceResponse<List<CardTypeDTO>>>> RemoveCardType(int id) 
        {   
            return Ok(await _typeService.RemoveCardType(id));
        }
    }
}
