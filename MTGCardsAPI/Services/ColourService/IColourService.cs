namespace MTGCardsAPI.Services.ColourService
{
    public interface IColourService
    {
        Task<ServiceResponse<List<ColourDTO>>> GetAllColours(int page);
        Task<ServiceResponse<List<ColourDTO>>> GetColoursByName(string name, int page);
        Task<ServiceResponse<ColourDTO>> CreateColour(ColourDTO request);
        Task<ServiceResponse<ColourDTO>> EditColour(int id, ColourDTO request);
        Task<ServiceResponse<List<ColourDTO>>> RemoveColour(int id);
    }
}
