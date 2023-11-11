using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MTGCardsAPI.Services.ColourService;
using System.ComponentModel.DataAnnotations;

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
        [ProducesResponseType(200), ProducesResponseType(400)]
        public async Task<ActionResult<ServiceResponse<List<ColourDTO>>>> GetAllColours(int page, [Required] float resultsPerPage)
        {
            if (resultsPerPage == 0 || page == 0)
                return BadRequest();

            return Ok(await _colourService.GetAllColours(page, resultsPerPage));
        }

        [HttpGet("search/{name}/{page}")]
        [ProducesResponseType(200), ProducesResponseType(400)]
        public async Task<ActionResult<ServiceResponse<List<ColourDTO>>>> GetColoursByName(string name, int page, [Required] float resultsPerPage)
        {
            if (resultsPerPage == 0 || page == 0)
                return BadRequest();

            return Ok(await _colourService.GetColoursByName(name, page, resultsPerPage));
        }

        [HttpPost("create"), Authorize]
        [ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(401)]
        public async Task<ActionResult<ServiceResponse<ColourDTO>>> CreateColour(ColourDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(request);
            }
            return Ok(await _colourService.CreateColour(request));
        }

        [HttpPut("edit/{id}"), Authorize]
        [ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(401)]
        public async Task<ActionResult<ServiceResponse<ColourDTO>>> EditColour(int id, ColourDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(request);
            }
            return Ok(await _colourService.EditColour(id, request));
        }

        [HttpDelete("delete/{id}"), Authorize]
        [ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(401)]
        public async Task<ActionResult<ServiceResponse<bool>>> RemoveColour(int id)
        {
            return Ok(await _colourService.RemoveColour(id));
        }
    }
}
