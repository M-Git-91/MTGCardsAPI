namespace MTGCardsAPI.Services.CardService
{
    public interface ICardService
    {
        Task<ServiceResponse<List<CardResponseDTO>>> GetAllCards(float resultsPerPage, int page);
        Task<ServiceResponse<List<CardResponseDTO>>> GetCardsByName(string name, float resultsPerPage, int page);
        Task<ServiceResponse<List<CardResponseDTO>>> GetCardsByColour(string colour, float resultsPerPage,int page);
        Task<ServiceResponse<List<CardResponseDTO>>> GetCardsByAbility(string ability, float resultsPerPage, int page);
        Task<ServiceResponse<List<CardResponseDTO>>> GetCardsByRulesText(string rulesText, float resultsPerPage, int page);
        Task<ServiceResponse<List<CardResponseDTO>>> GetCardsByFlavourText(string flavourText, float resultsPerPage, int page);
        Task<ServiceResponse<List<CardResponseDTO>>> GetCardsByPower(int power, float resultsPerPage,int page);
        Task<ServiceResponse<List<CardResponseDTO>>> GetCardsByToughness(int toughness, float resultsPerPage, int page);
        Task<ServiceResponse<List<CardResponseDTO>>> GetCardsBySet(string setName, float resultsPerPage, int page);
        Task<ServiceResponse<List<CardResponseDTO>>> GetCardsByType(string typeName, float resultsPerPage, int page);
        Task<ServiceResponse<CardResponseDTO>> CreateCard(CardRequestDTO request);
        Task<ServiceResponse<CardResponseDTO>> EditCard(int id, CardRequestDTO request);
        Task<ServiceResponse<bool>> RemoveCard(int id);
    }
}
