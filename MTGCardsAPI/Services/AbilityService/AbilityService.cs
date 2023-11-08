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
                response.Message = "Ability was successfully created.";
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
                response.Message = "Ability was successfully updated.";

                _context.Update(searchResult.Data);
                _context.SaveChanges();
            }

            return response;
        }

        public async Task<ServiceResponse<List<AbilityDTO>>> GetAbilitiesByName(string name, int page, float resultsPerPage)
        {
            var response = new ServiceResponse<List<AbilityDTO>> { Data = new List<AbilityDTO>() };
            var searchResult = await _context.Abilities
                        .Where(a => a.Name.ToLower()
                        .Contains(name.ToLower()))
                        .ToListAsync();


            if (searchResult.Count > 0)
            {
                var pageCount = PageCount(searchResult, resultsPerPage);

                var paginatedResults = PaginateAbilities(page, resultsPerPage, searchResult);

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

        public async Task<ServiceResponse<List<AbilityDTO>>> GetAllAbilities(int page, float resultsPerPage)
        {
            var allAbilities = await _context.Abilities.ToListAsync();
            var pageCount = PageCount(allAbilities, resultsPerPage);
            var paginatedAbilities = PaginateAbilities(page, resultsPerPage, allAbilities);

            var mapDto = paginatedAbilities.Select(pct => _mapper.Map<AbilityDTO>(pct));

            var response = new ServiceResponse<List<AbilityDTO>> { Data = new List<AbilityDTO>() };
            response.Data.AddRange(mapDto);
            response.Pages = (int)pageCount;
            response.CurrentPage = page;

            return response;
        }

        public async Task<ServiceResponse<bool>> RemoveAbility(int id)
        {
            var response = new ServiceResponse<bool>();
            var searchResult = await FindAbilityById(id);

            if (searchResult.Data != null)
            {
                _context.Abilities.Remove(searchResult.Data);
                await _context.SaveChangesAsync();

                response.Data = false;
                response.Success = true;
                response.Message = "Ability was successfully deleted.";
            }
            else
            {
                response.Data = false;
                response.Success = searchResult.Success;
                response.Message = searchResult.Message;
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

        private List<Ability> PaginateAbilities(int page, float resultsPerPage, List<Ability> abilities)
        {
            return abilities.Skip((page - 1) * (int)resultsPerPage)
                        .Take((int)resultsPerPage)
                        .ToList();
        }

        private double PageCount(List<Ability> abilities, float resultsPerPage)
        {
            return Math.Ceiling(abilities.Count() / resultsPerPage);
        }
    }
}
