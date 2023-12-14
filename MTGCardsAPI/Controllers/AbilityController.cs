using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MTGCardsAPI.Services.AbilityService;
using System.ComponentModel.DataAnnotations;

namespace MTGCardsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AbilityController : ControllerBase
    {
        private readonly IAbilityService _abilityService;

        public AbilityController(IAbilityService abilityService)
        {
            _abilityService = abilityService;
        }

        [HttpGet("search/{page}")]
        [ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(404)]
        public async Task<ActionResult<ServiceResponse<List<AbilityDTO>>>> GetAllAbilities(int page, [Required]float resultsPerPage)
        {
            if (resultsPerPage == 0 || page == 0)
                return BadRequest();

            var response = await _abilityService.GetAllAbilities(page, resultsPerPage);

            if (response.Data.Count == 0)
                return NotFound(response);

            return Ok(response);
        }

        [HttpGet("search/{name}/{page}")]
        [ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(401)]
        public async Task<ActionResult<ServiceResponse<List<AbilityDTO>>>> GetAbilitiesByName(string name, int page, [Required] float resultsPerPage)
        {
            if (resultsPerPage == 0 || page == 0)
                return BadRequest();

            var response = await _abilityService.GetAbilitiesByName(name, page, resultsPerPage);

            if (response.Data.Count == 0)
                return NotFound(response);

            return Ok(response);
        }

        [HttpPut("edit/{id}"), Authorize]
        [ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(401), ProducesResponseType(404), ProducesResponseType(500)]
        public async Task<ActionResult<ServiceResponse<AbilityDTO>>> EditAbility(int id, AbilityDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(request);

            var response = await _abilityService.EditAbility(id, request);

            if (response.Success == false && response.Data == null) 
                return NotFound(response);

            if (response.Success == false && response.Data != null)
                return StatusCode(500, response);

            return Ok(response);
        }

        [HttpPost("create"), Authorize]
        [ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(401), ProducesResponseType(500)]
        public async Task<ActionResult<ServiceResponse<List<AbilityDTO>>>> CreateAbility(AbilityDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(request);

            var response = await _abilityService.CreateAbility(request);

            if (response.Success == false)
                return StatusCode(500, response);


            return Ok(response);
        }

        [HttpDelete("delete/{id}"), Authorize]
        [ProducesResponseType(200), ProducesResponseType(401), ProducesResponseType(404)]
        public async Task<ActionResult<ServiceResponse<AbilityDTO>>> RemoveAbility(int id)
        {
            var response = await _abilityService.RemoveAbility(id);

            if (response.Success == false)
                return NotFound(response);

            return Ok(response);
        }

    }
}
