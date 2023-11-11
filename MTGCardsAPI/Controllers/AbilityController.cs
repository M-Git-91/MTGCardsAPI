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
        [ProducesResponseType(200), ProducesResponseType(400)]
        public async Task<ActionResult<ServiceResponse<List<AbilityDTO>>>> GetAllAbilities(int page, [Required]float resultsPerPage)
        {
            if (resultsPerPage == 0 || page == 0)
                return BadRequest();

            return Ok(await _abilityService.GetAllAbilities(page, resultsPerPage));
        }

        [HttpGet("search/{name}/{page}")]
        [ProducesResponseType(200), ProducesResponseType(400)]
        public async Task<ActionResult<ServiceResponse<List<AbilityDTO>>>> GetAbilitiesByName(string name, int page, [Required] float resultsPerPage)
        {
            if (resultsPerPage == 0 || page == 0)
                return BadRequest();

            return Ok(await _abilityService.GetAbilitiesByName(name, page, resultsPerPage));
        }

        [HttpPut("edit/{id}"), Authorize]
        [ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(401)]
        public async Task<ActionResult<ServiceResponse<AbilityDTO>>> EditAbility(int id, AbilityDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(request);
            }
            return Ok(await _abilityService.EditAbility(id, request));
        }

        [HttpPost("create"), Authorize]
        [ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(401)]
        public async Task<ActionResult<ServiceResponse<List<AbilityDTO>>>> CreateAbility(AbilityDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(request);
            }
            return Ok(await _abilityService.CreateAbility(request));
        }

        [HttpDelete("delete/{id}"), Authorize]
        [ProducesResponseType(200), ProducesResponseType(401)]
        public async Task<ActionResult<ServiceResponse<bool>>> RemoveAbility(int id)
        {
            return Ok(await _abilityService.RemoveAbility(id));
        }

    }
}
