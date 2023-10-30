namespace MTGCardsAPI.Services.Ability
{
    public interface IAbilityService
    {
        Task<ServiceResponse<List<AbilityDTO>>> GetAllAbilities(int page);
        Task<ServiceResponse<List<AbilityDTO>>> GetAbilitiesByName(string name, int page);
        Task<ServiceResponse<AbilityDTO>> CreateAbility(AbilityDTO request);
        Task<ServiceResponse<AbilityDTO>> EditAbility(int id, AbilityDTO request);
        Task<ServiceResponse<List<AbilityDTO>>> RemoveAbility(int id);
    }
}
