using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MTGCardsAPI.DTO
{
    public class CardResponseDTO
    {
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        public string ImageURL { get; set; } = string.Empty;
        public List<string> Colours { get; set; } = new List<string>();
        public List<string> Abilities { get; set; } = new List<string>();
        [MaxLength(1500)]
        public string RulesText { get; set; } = string.Empty;
        [MaxLength(500)]
        public string FlavourText { get; set; } = string.Empty;
        public int? Power { get; set; }
        public int? Toughness { get; set; }
        public string Set { get; set; } = string.Empty;
        public List<string> Type { get; set; } = new List<string>();
    }
}
