using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MTGCardsAPI.Services.ColourService;

namespace MTGCardsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColourController : ControllerBase
    {
        private readonly IColourService _colourService;

        public ColourController(IColourService colourService)
        {
            _colourService = colourService;
        }

        [HttpGet("search/{page}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<ServiceResponse<List<CardTypeDTO>>>> GetAllColours(int page)
        {
            return Ok(await _colourService.GetAllColours(page));
        }

        [HttpGet("search/{name}/{page}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<ServiceResponse<List<ColourDTO>>>> GetColoursByName(string name, int page)
        {
            return Ok(await _colourService.GetColoursByName(name, page));
        }

        [HttpPost("create")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<List<ColourDTO>>> CreateColour(ColourDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(request);
            }
            return Ok(await _colourService.CreateColour(request));
        }

        [HttpPut("edit/{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<ServiceResponse<ColourDTO>>> EditColour(int id, ColourDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(request);
            }
            return Ok(await _colourService.EditColour(id, request));
        }

        [HttpDelete("delete/{id}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<List<ColourDTO>>> RemoveColour(int id)
        {
            return Ok(await _colourService.RemoveColour(id));
        }
    }
}
