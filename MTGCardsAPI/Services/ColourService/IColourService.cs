namespace MTGCardsAPI.Services.ColourService
{
    public interface IColourService
    {
        Task<ServiceResponse<List<ColourDTO>>> GetAllColours(int page, float resultsPerPage);
        Task<ServiceResponse<List<ColourDTO>>> GetColoursByName(string name, int page, float resultsPerPage);
        Task<ServiceResponse<ColourDTO>> CreateColour(ColourDTO request);
        Task<ServiceResponse<ColourDTO>> EditColour(int id, ColourDTO request);
        Task<ServiceResponse<ColourDTO>> RemoveColour(int id);
    }
}
