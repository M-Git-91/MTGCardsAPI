using AutoMapper;

namespace MTGCardsAPI.Services.ColourService
{
    public class ColourService : IColourService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ColourService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<ColourDTO>> CreateColour(ColourDTO request)
        {
            var response = new ServiceResponse<ColourDTO>();
            Colour newColour = new Colour { 
                Name = request.Name, 
                IconURL = request.IconURL,
            };

            _context.Colours.Add(newColour);
            var saveCount = await _context.SaveChangesAsync();

            if (saveCount > 0)
            {
                response.Data = _mapper.Map<ColourDTO>(newColour);
                response.Message = "Colour was successfully created.";
            }
            else
            {
                response.Data = new ColourDTO();
                response.Success = false;
                response.Message = "Cardtype was not created.";
            }

            return response;
        }

        public async Task<ServiceResponse<ColourDTO>> EditColour(int id, ColourDTO request)
        {
            var response = new ServiceResponse<ColourDTO>();
            var searchResult = await FindColourById(id);

            if (searchResult.Success == false)
            {
                response.Success = false;
                response.Message = searchResult.Message;
            }
            else
            {
                searchResult.Data.Name = request.Name;
                searchResult.Data.IconURL = request.IconURL;

                _context.Update(searchResult.Data);
                _context.SaveChanges();

                response.Data = _mapper.Map<ColourDTO>(searchResult.Data);
                response.Message = "Colour was successfully updated.";
            }

            return response;
        }

        public async Task<ServiceResponse<List<ColourDTO>>> GetAllColours(int page, float resultsPerPage)
        {
            var allColours = await _context.Colours.ToListAsync();
            var pageCount = PageCount(allColours, resultsPerPage);


            var paginateColours = await _context.Colours
                                .Skip((page - 1) * (int)resultsPerPage)
                                .Take((int)resultsPerPage)
                                .ToListAsync();

            var mapDto = paginateColours.Select(pct => _mapper.Map<ColourDTO>(pct));

            var response = new ServiceResponse<List<ColourDTO>> { Data = new List<ColourDTO>() };
            
            response.Data.AddRange(mapDto);
            response.Pages = (int)pageCount;
            response.CurrentPage = page;

            return response;
        }

        public async Task<ServiceResponse<List<ColourDTO>>> GetColoursByName(string name, int page, float resultsPerPage)
        {
            var response = new ServiceResponse<List<ColourDTO>> { Data = new List<ColourDTO>() };
            var searchResult = await _context.Colours
                        .Where(c => c.Name.ToLower()
                        .Contains(name.ToLower()))
                        .ToListAsync();


            if (searchResult.Count > 0)
            {
                var pageCount = PageCount(searchResult, resultsPerPage);

                var paginateColours = PaginateColours(page, resultsPerPage, searchResult);

                var mapDto = paginateColours.Select(sr => _mapper.Map<ColourDTO>(sr));
                response.Data.AddRange(mapDto);
                response.Message = "Colour(s) found.";
                response.Pages = (int)pageCount;
                response.CurrentPage = page;
            }
            else
            {
                response.Data = new List<ColourDTO>();
                response.Success = false;
                response.Message = "No Colour found";
            }

            return response;
        }

        public async Task<ServiceResponse<List<ColourDTO>>> RemoveColour(int id)
        {
            var response = new ServiceResponse<List<ColourDTO>>();
            var searchResult = await FindColourById(id);

            if (searchResult.Data != null)
            {
                _context.Colours.Remove(searchResult.Data);
                await _context.SaveChangesAsync();

                response.Data = new List<ColourDTO>();
                response.Message = "Colour was successfully deleted.";
            }
            else
            {
                response.Success = searchResult.Success;
                response.Message = searchResult.Message;
            }

            return response;
        }

        private async Task<ServiceResponse<Colour>> FindColourById(int id)
        {
            var response = new ServiceResponse<Colour>();
            var colour = await _context.Colours.FindAsync(id);

            if (colour == null)
            {
                response.Success = false;
                response.Message = "Colour was not found.";
            }
            else
            {
                response.Data = colour;
            }
            return response;
        }

        private List<Colour> PaginateColours(int page, float resultsPerPage, List<Colour> colours)
        {
            return colours.Skip((page - 1) * (int)resultsPerPage)
                        .Take((int)resultsPerPage)
                        .ToList();
        }

        private double PageCount(List<Colour> colours, float resultsPerPage)
        {
            return Math.Ceiling(colours.Count() / resultsPerPage);
        }
    }
}
