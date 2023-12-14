using AutoMapper;

namespace MTGCardsAPI.Services.SetService
{
    public class SetService : ISetService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public SetService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<SetDTO>> CreateSet(SetDTO request)
        {
            var response = new ServiceResponse<SetDTO>();
            Set newSet = new Set
            {
                Name = request.Name,
                IconURL = request.IconURL,
            };

            _context.Sets.Add(newSet);
            var saveCount = await _context.SaveChangesAsync();

            if (saveCount > 0)
            {
                response.Data = _mapper.Map<SetDTO>(newSet);
                response.Message = "Set was successfully created.";
            }
            else
            {
                response.Success = false;
                response.Message = "Set was not created.";
            }

            return response;
        }

        public async Task<ServiceResponse<SetDTO>> EditSet(int id, SetDTO request)
        {
            var response = new ServiceResponse<SetDTO>();
            var searchResult = await FindSetById(id);

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
                var saveCount = _context.SaveChanges();
                if (saveCount > 0) 
                {
                    response.Data = _mapper.Map<SetDTO>(searchResult.Data);
                    response.Message = "Set was successfully updated.";
                }
                else
                {
                    response.Data = _mapper.Map<SetDTO>(searchResult.Data);
                    response.Success = false;
                    response.Message = "Set was not updated.";
                }
            }

            return response;
        }

        public async Task<ServiceResponse<List<SetDTO>>> GetAllSets(int page, float resultsPerPage)
        {
            var allSets = await _context.Sets.ToListAsync();
            
            var pageCount = PageCount(allSets, resultsPerPage);
            var paginateSets = PaginateSets(page, resultsPerPage, allSets);

            var mapDto = paginateSets.Select(pct => _mapper.Map<SetDTO>(pct));

            var response = new ServiceResponse<List<SetDTO>> { Data = new List<SetDTO>() };
            response.Data.AddRange(mapDto);
            response.Pages = (int)pageCount;
            response.CurrentPage = page;

            return response;
        }

        public async Task<ServiceResponse<List<SetDTO>>> GetSetsByName(string name, int page, float resultsPerPage)
        {
            var response = new ServiceResponse<List<SetDTO>> { Data = new List<SetDTO>() };
            var searchResult = await _context.Sets
                        .Where(a => a.Name.ToLower()
                        .Contains(name.ToLower()))
                        .ToListAsync();


            if (searchResult.Count > 0)
            {
                var pageCount = PageCount(searchResult, resultsPerPage);

                var paginatedResults = PaginateSets(page, resultsPerPage, searchResult);

                var mapDto = paginatedResults.Select(sr => _mapper.Map<SetDTO>(sr));
                response.Data.AddRange(mapDto);
                response.Pages = (int)pageCount;
                response.CurrentPage = page;
            }
            else
            {
                response.Data = new List<SetDTO>();
                response.Success = false;
                response.Message = "No sets found";
            }

            return response;
        }

        public async Task<ServiceResponse<SetDTO>> RemoveSet(int id)
        {
            var response = new ServiceResponse<SetDTO>();
            var searchResult = await FindSetById(id);

            if (searchResult.Data != null)
            {
                _context.Sets.Remove(searchResult.Data);
                await _context.SaveChangesAsync();

                response.Message = "Set was successfully deleted.";
            }
            else
            {
                response.Success = searchResult.Success;
                response.Message = searchResult.Message;
            }

            return response;
        }

        private async Task<ServiceResponse<Set>> FindSetById(int id)
        {
            var response = new ServiceResponse<Set>();
            var set = await _context.Sets.FindAsync(id);

            if (set == null)
            {
                response.Success = false;
                response.Message = "Set was not found.";
            }
            else
            {
                response.Data = set;
            }
            return response;
        }

        private List<Set> PaginateSets(int page, float resultsPerPage, List<Set> sets)
        {
            return sets.Skip((page - 1) * (int)resultsPerPage)
                        .Take((int)resultsPerPage)
                        .ToList();
        }

        private double PageCount(List<Set> sets, float resultsPerPage)
        {
            return Math.Ceiling(sets.Count() / resultsPerPage);
        }
    }
}
