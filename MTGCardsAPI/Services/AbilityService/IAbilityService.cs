namespace MTGCardsAPI.Services.AbilityService
{
    public interface IAbilityService
    {
        Task<ServiceResponse<List<AbilityDTO>>> GetAllAbilities(int page, float resultsPerPage);
        Task<ServiceResponse<List<AbilityDTO>>> GetAbilitiesByName(string name, int page, float resultsPerPage);
        Task<ServiceResponse<AbilityDTO>> CreateAbility(AbilityDTO request);
        Task<ServiceResponse<AbilityDTO>> EditAbility(int id, AbilityDTO request);
        Task<ServiceResponse<bool>> RemoveAbility(int id);
    }
}
