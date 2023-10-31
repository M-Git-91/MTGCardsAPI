using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MTGCardsAPI.Services.SetService;

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
        [ProducesResponseType(200)]
        public async Task<ActionResult<ServiceResponse<List<SetDTO>>>> GetAllSets(int page)
        {
            return Ok(await _setService.GetAllSets(page));
        }

        [HttpGet("search/{name}/{page}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<ServiceResponse<List<SetDTO>>>> GetSetsByName(string name, int page)
        {
            return Ok(await _setService.GetSetsByName(name, page));
        }

        [HttpPost("create")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<List<Set>>> CreateSet(SetDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(request);
            }
            return Ok(await _setService.CreateSet(request));
        }

        [HttpPut("edit/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ServiceResponse<SetDTO>>> EditSet(int id, SetDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(request);
            }
            return Ok(await _setService.EditSet(id, request));
        }

        [HttpDelete("delete/{id}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<List<SetDTO>>> RemoveSet(int id)
        {
            return Ok(await _setService.RemoveSet(id));
        }

    }
}
