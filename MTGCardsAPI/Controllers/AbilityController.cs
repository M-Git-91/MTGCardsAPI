using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MTGCardsAPI.Services.Ability;

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
        [ProducesResponseType(200)]
        public async Task<ActionResult<ServiceResponse<List<AbilityDTO>>>> GetAllAbilities(int page)
        {
            return Ok(await _abilityService.GetAllAbilities(page));
        }

        [HttpGet("search/{name}/{page}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<ServiceResponse<List<AbilityDTO>>>> GetAbilitiesByName(string name, int page)
        {
            return Ok(await _abilityService.GetAbilitiesByName(name, page));
        }

        [HttpPut("edit/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ServiceResponse<AbilityDTO>>> EditAbility(int id, AbilityDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(request);
            }
            return Ok(await _abilityService.EditAbility(id, request));
        }

        [HttpPost("create")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<List<Ability>>> CreateAbility(AbilityDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(request);
            }
            return Ok(await _abilityService.CreateAbility(request));
        }

        [HttpDelete("delete/{id}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<List<AbilityDTO>>> RemoveAbility(int id)
        {
            return Ok(await _abilityService.RemoveAbility(id));
        }

    }
}
