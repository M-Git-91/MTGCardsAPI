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
            }
            else
            {
                response.Data = new SetDTO();
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

                response.Data = _mapper.Map<SetDTO>(searchResult.Data);

                _context.Update(searchResult.Data);
                _context.SaveChanges();
            }

            return response;
        }

        public async Task<ServiceResponse<List<SetDTO>>> GetAllSets(int page)
        {
            var pageResults = 5f;
            var allSets = await GetAll();
            var pageCount = Math.Ceiling(allSets.Count() / pageResults);


            var paginateSets = await _context.Sets
                                .Skip((page - 1) * (int)pageResults)
                                .Take((int)pageResults)
                                .ToListAsync();

            var mapDto = paginateSets.Select(pct => _mapper.Map<SetDTO>(pct));

            var response = new ServiceResponse<List<SetDTO>> { Data = new List<SetDTO>() };
            response.Data.AddRange(mapDto);
            response.Pages = (int)pageCount;
            response.CurrentPage = page;

            return response;
        }

        public async Task<ServiceResponse<List<SetDTO>>> GetSetsByName(string name, int page)
        {
            var response = new ServiceResponse<List<SetDTO>> { Data = new List<SetDTO>() };
            var searchResult = await _context.Sets
                        .Where(a => a.Name.ToLower()
                        .Contains(name.ToLower()))
                        .ToListAsync();


            if (searchResult.Count > 0)
            {
                var pageResults = 5f;
                var pageCount = Math.Ceiling(searchResult.Count() / pageResults);

                var paginatedResults = searchResult
                                .Skip((page - 1) * (int)pageResults)
                                .Take((int)pageResults);

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

        public async Task<ServiceResponse<List<SetDTO>>> RemoveSet(int id)
        {
            var response = new ServiceResponse<List<SetDTO>>();
            var searchResult = await FindSetById(id);

            if (searchResult.Data != null)
            {
                _context.Sets.Remove(searchResult.Data);
                await _context.SaveChangesAsync();

                response.Data = new List<SetDTO>();
            }
            else
            {
                response.Success = searchResult.Success;
                response.Message = searchResult.Message;
            }

            return response;
        }

        private async Task<List<SetDTO>> GetAll()
        {
            var searchResult = await _context.Sets.ToListAsync();

            var response = new List<SetDTO>();

            foreach (var set in searchResult)
            {
                SetDTO dto = new SetDTO
                {
                    Id = set.Id,
                    Name = set.Name,
                    IconURL = set.IconURL,
                };
                response.Add(dto);
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
    }
}
