﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MTGCardsAPI.Services.SetService;
using System.ComponentModel.DataAnnotations;

namespace MTGCardsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SetController : ControllerBase
    {
        private readonly ISetService _setService;

        public SetController(ISetService setService)
        {
            _setService = setService;
        }

        [HttpGet("search/{page}")]
        [ProducesResponseType(200), ProducesResponseType(400)]
        public async Task<ActionResult<ServiceResponse<List<SetDTO>>>> GetAllSets(int page, [Required] float resultsPerPage)
        {
            if (resultsPerPage == 0 || page == 0)
                return BadRequest();

            return Ok(await _setService.GetAllSets(page, resultsPerPage));
        }

        [HttpGet("search/{name}/{page}")]
        [ProducesResponseType(200), ProducesResponseType(400)]
        public async Task<ActionResult<ServiceResponse<List<SetDTO>>>> GetSetsByName(string name, int page, [Required] float resultsPerPage)
        {
            if (resultsPerPage == 0 || page == 0)
                return BadRequest();

            return Ok(await _setService.GetSetsByName(name, page, resultsPerPage));
        }

        [HttpPost("create"), Authorize]
        [ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(401)]
        public async Task<ActionResult<ServiceResponse<List<Set>>>> CreateSet(SetDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(request);
            }
            return Ok(await _setService.CreateSet(request));
        }

        [HttpPut("edit/{id}"), Authorize]
        [ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(401)]
        public async Task<ActionResult<ServiceResponse<SetDTO>>> EditSet(int id, SetDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(request);
            }
            return Ok(await _setService.EditSet(id, request));
        }

        [HttpDelete("delete/{id}"), Authorize]
        [ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(401)]
        public async Task<ActionResult<ServiceResponse<bool>>> RemoveSet(int id)
        {
            return Ok(await _setService.RemoveSet(id));
        }

    }
}
