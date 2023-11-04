namespace MTGCardsAPI.Services.CardService
{
    public interface ICardService
    {
        Task<ServiceResponse<List<CardResponseDTO>>> GetAllCards(int page);
        Task<ServiceResponse<List<CardResponseDTO>>> GetCardsByName(string name, int page);
        Task<ServiceResponse<List<Card>>> GetCardsByColour(string colour, int page);
        Task<ServiceResponse<List<CardResponseDTO>>> GetCardsByAbility(string ability, int page);
        Task<ServiceResponse<List<Card>>> GetCardsByRulesText(string rulesText, int page);
        Task<ServiceResponse<List<Card>>> GetCardsByFlavourText(string flavourText, int page);
        Task<ServiceResponse<List<Card>>> GetCardsByPower(int power, int page);
        Task<ServiceResponse<List<Card>>> GetCardsByToughness(int toughness, int page);
        Task<ServiceResponse<List<Card>>> GetCardsBySet(string setName, int page);
        Task<ServiceResponse<List<Card>>> GetCardsByType(string typeName, int page);
        Task<ServiceResponse<CardResponseDTO>> CreateCard(CardRequestDTO request);
        Task<ServiceResponse<Card>> EditCard(int id, CardRequestDTO request);
        Task<ServiceResponse<List<Card>>> RemoveCard(int id);
    }
}
