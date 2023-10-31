using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace MTGCardsAPI.Services.AbilityService
{
    public class AbilityService : IAbilityService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public AbilityService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<AbilityDTO>> CreateAbility(AbilityDTO request)
        {
            var response = new ServiceResponse<AbilityDTO>();
            Ability newAbility = new Ability { 
                Name = request.Name,
                Description = request.Description,
            };

            _context.Abilities.Add(newAbility);
            var saveCount = await _context.SaveChangesAsync();

            if (saveCount > 0)
            {
                response.Data = _mapper.Map<AbilityDTO>(newAbility);
            }
            else
            {
                response.Data = new AbilityDTO();
                response.Success = false;
                response.Message = "Ability was not created.";
            }

            return response;
        }

        public async Task<ServiceResponse<AbilityDTO>> EditAbility(int id, AbilityDTO request)
        {
            var response = new ServiceResponse<AbilityDTO>();
            var searchResult = await FindAbilityById(id);

            if (searchResult.Success == false)
            {
                response.Success = false;
                response.Message = searchResult.Message;
            }
            else
            {
                searchResult.Data.Name = request.Name;
                searchResult.Data.Description = request.Description;
                
                response.Data = _mapper.Map<AbilityDTO>(searchResult.Data);

                _context.Update(searchResult.Data);
                _context.SaveChanges();
            }

            return response;
        }

        public async Task<ServiceResponse<List<AbilityDTO>>> GetAbilitiesByName(string name, int page)
        {
            var response = new ServiceResponse<List<AbilityDTO>> { Data = new List<AbilityDTO>() };
            var searchResult = await _context.Abilities
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

                var mapDto = paginatedResults.Select(sr => _mapper.Map<AbilityDTO>(sr));
                response.Data.AddRange(mapDto);
                response.Pages = (int)pageCount;
                response.CurrentPage = page;
            }
            else
            {
                response.Data = new List<AbilityDTO>();
                response.Success = false;
                response.Message = "No abilities found";
            }

            return response;
        }

        public async Task<ServiceResponse<List<AbilityDTO>>> GetAllAbilities(int page)
        {
            var pageResults = 5f;
            var allAbilities = await GetAll();
            var pageCount = Math.Ceiling(allAbilities.Count() / pageResults);


            var paginateAbilities = await _context.Abilities
                                .Skip((page - 1) * (int)pageResults)
                                .Take((int)pageResults)
                                .ToListAsync();

            var mapDto = paginateAbilities.Select(pct => _mapper.Map<AbilityDTO>(pct));

            var response = new ServiceResponse<List<AbilityDTO>> { Data = new List<AbilityDTO>() };
            response.Data.AddRange(mapDto);
            response.Pages = (int)pageCount;
            response.CurrentPage = page;

            return response;
        }

        public async Task<ServiceResponse<List<AbilityDTO>>> RemoveAbility(int id)
        {
            var response = new ServiceResponse<List<AbilityDTO>>();
            var searchResult = await FindAbilityById(id);

            if (searchResult.Data != null)
            {
                _context.Abilities.Remove(searchResult.Data);
                await _context.SaveChangesAsync();

                response.Data = new List<AbilityDTO>();
            }
            else
            {
                response.Success = searchResult.Success;
                response.Message = searchResult.Message;
            }

            return response;
        }

        private async Task<List<AbilityDTO>> GetAll()
        {
            var searchResult = await _context.Abilities.ToListAsync();

            var response = new List<AbilityDTO>();

            foreach (var ability in searchResult)
            {
                AbilityDTO dto = new AbilityDTO
                {
                    Id = ability.Id,
                    Name = ability.Name,
                };
                response.Add(dto);
            }

            return response;
        }

        private async Task<ServiceResponse<Ability>> FindAbilityById(int id)
        {
            var response = new ServiceResponse<Ability>();
            var ability = await _context.Abilities.FindAsync(id);

            if (ability == null)
            {
                response.Success = false;
                response.Message = "Ability was not found.";
            }
            else
            {
                response.Data = ability;
            }
            return response;
        }
    }
}
