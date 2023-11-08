namespace MTGCardsAPI.Services.SetService
{
    public interface ISetService
    {
        Task<ServiceResponse<List<SetDTO>>> GetAllSets(int page, float resultsPerPage);
        Task<ServiceResponse<List<SetDTO>>> GetSetsByName(string name, int page, float resultsPerPage);
        Task<ServiceResponse<SetDTO>> CreateSet(SetDTO request);
        Task<ServiceResponse<SetDTO>> EditSet(int id, SetDTO request);
        Task<ServiceResponse<bool>> RemoveSet(int id);
    }
}
