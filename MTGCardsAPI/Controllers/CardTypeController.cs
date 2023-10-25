﻿using Microsoft.AspNetCore.Http;
using MTGCardsAPI.Services.CardTypeService;

namespace MTGCardsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CardTypeController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly ICardTypeService _typeService;

        public CardTypeController(DataContext context, ICardTypeService typeService)
        {
            _context = context;
            _typeService = typeService;
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<ServiceResponse<List<CardTypeDTO>>>> GetAllTypes()
        {
            return Ok(await _typeService.GetAllTypes());
        }


        [HttpGet("{name}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<ServiceResponse<List<CardTypeDTO>>>> GetTypesByName(string name) 
        {             
            return Ok(await _typeService.GetTypesByName(name));
        }


        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<List<CardTypeDTO>>> CreateCardType(CardTypeDTO request) 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(request);
            }    
            return Ok(await _typeService.CreateCardType(request));
        }


        [HttpPut]
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

        [HttpDelete]
        [ProducesResponseType(200)]
        public async Task<ActionResult<List<CardTypeDTO>>> RemoveCardType(int id) 
        {   
            return Ok(await _typeService.RemoveCardType(id));
        }
    }
}
