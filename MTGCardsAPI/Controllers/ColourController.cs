using Azure;
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
        [ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(404)]
        public async Task<ActionResult<ServiceResponse<List<ColourDTO>>>> GetAllColours(int page, [Required] float resultsPerPage)
        {
            if (resultsPerPage == 0 || page == 0)
                return BadRequest();

            var response = await _colourService.GetAllColours(page, resultsPerPage);

            if (response.Data.Count == 0)
                return NotFound(response);

            return Ok(response);
        }

        [HttpGet("search/{name}/{page}")]
        [ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(404)]
        public async Task<ActionResult<ServiceResponse<List<ColourDTO>>>> GetColoursByName(string name, int page, [Required] float resultsPerPage)
        {
            if (resultsPerPage == 0 || page == 0)
                return BadRequest();

            var response = await _colourService.GetColoursByName(name, page, resultsPerPage);

            if (response.Data.Count == 0)
                return NotFound(response);

            return Ok(response);
        }

        [HttpPost("create"), Authorize]
        [ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(401), ProducesResponseType(404), ProducesResponseType(500)]
        public async Task<ActionResult<ServiceResponse<ColourDTO>>> CreateColour(ColourDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(request);

            var response = await _colourService.CreateColour(request);

            if (response.Success == false)
                return StatusCode(500, response);

            return Ok(response);
        }

        [HttpPut("edit/{id}"), Authorize]
        [ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(401), ProducesResponseType(404), ProducesResponseType(500)]
        public async Task<ActionResult<ServiceResponse<ColourDTO>>> EditColour(int id, ColourDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(request);

            var response = await _colourService.EditColour(id, request);

            if (response.Success == false && response.Data == null)
                return NotFound(response);

            if (response.Success == false && response.Data != null)
                return StatusCode(500, response);


            return Ok(response);
        }

        [HttpDelete("delete/{id}"), Authorize]
        [ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(401), ProducesResponseType(404)]
        public async Task<ActionResult<ServiceResponse<ColourDTO>>> RemoveColour(int id)
        {
            var response = await _colourService.RemoveColour(id);

            if (response.Success == false)
                return NotFound(response);

            return Ok(response);
        }
    }
}
