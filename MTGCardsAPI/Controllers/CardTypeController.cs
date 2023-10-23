using Microsoft.AspNetCore.Http;
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
        public async Task<ActionResult<ServiceResponse<List<CardType>>>> GetAllTypes()
        {
            return Ok(await _typeService.GetAllTypes());
        }


        [HttpGet("{name}")]
        public async Task<ActionResult<ServiceResponse<List<CardType>>>> GetTypesByName(string name) 
        {          
            return Ok(await _typeService.GetTypesByName(name));
        }


        [HttpPost]
        public async Task<ActionResult<List<CardType>>> CreateCardType(CardType request) 
        {
            _context.CardTypes.Add(request);
            _context.SaveChanges();
            
            return Ok(await _typeService.GetAllTypes());
        }


        [HttpPut]
        public async Task<ActionResult<ServiceResponse<CardType>>> EditCardType(int id, CardType request) 
        {         
            return Ok(await _typeService.EditCardType(id, request));
        }

        [HttpDelete]
        public async Task<ActionResult<List<CardType>>> RemoveCardType(int id) 
        {   
            return Ok(await _typeService.RemoveCardType(id));
        }
    }
}
