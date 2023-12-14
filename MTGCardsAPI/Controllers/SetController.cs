using Azure;
using Microsoft.AspNetCore.Authorization;
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
        [ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(404)]
        public async Task<ActionResult<ServiceResponse<List<SetDTO>>>> GetAllSets(int page, [Required] float resultsPerPage)
        {
            if (resultsPerPage == 0 || page == 0)
                return BadRequest();

            var response = await _setService.GetAllSets(page, resultsPerPage);

            if (response.Data.Count == 0)
                return NotFound(response);

            return Ok(response);
        }

        [HttpGet("search/{name}/{page}")]
        [ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(404)]
        public async Task<ActionResult<ServiceResponse<List<SetDTO>>>> GetSetsByName(string name, int page, [Required] float resultsPerPage)
        {
            if (resultsPerPage == 0 || page == 0)
                return BadRequest();

            var response = await _setService.GetSetsByName(name, page, resultsPerPage);

            if (response.Data.Count == 0)
                return NotFound(response);

            return Ok(response);
        }

        [HttpPost("create"), Authorize]
        [ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(401), ProducesResponseType(500)]
        public async Task<ActionResult<ServiceResponse<List<Set>>>> CreateSet(SetDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(request);

            var response = await _setService.CreateSet(request);

            if (response.Success == false)
                return StatusCode(500, response);

            return Ok(response);
        }

        [HttpPut("edit/{id}"), Authorize]
        [ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(401), ProducesResponseType(404), ProducesResponseType(500)]
        public async Task<ActionResult<ServiceResponse<SetDTO>>> EditSet(int id, SetDTO request)
        {
            if (!ModelState.IsValid)
                return BadRequest(request);

            var response = await _setService.EditSet(id, request);

            if (response.Success == false && response.Data == null)
                return NotFound(response);

            if (response.Success == false && response.Data != null)
                return StatusCode(500, response);

            return Ok(response);
        }

        [HttpDelete("delete/{id}"), Authorize]
        [ProducesResponseType(200), ProducesResponseType(400), ProducesResponseType(401), ProducesResponseType(404)]
        public async Task<ActionResult<ServiceResponse<SetDTO>>> RemoveSet(int id)
        {
            var response = await _setService.RemoveSet(id);

            if (response.Success == false)
                return NotFound(response);

            return Ok(response);
        }

    }
}
