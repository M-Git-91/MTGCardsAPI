using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MTGCardsAPI.DTO
{
    public class CardRequestDTO
    {
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        public string ImageURL { get; set; } = string.Empty;
        public List<int> Colours { get; set; } = new List<int>();
        public List<int> Abilities { get; set; } = new List<int>();
        [MaxLength(1500)]
        public string RulesText { get; set; } = string.Empty;
        [MaxLength(500)]
        public string FlavourText { get; set; } = string.Empty;
        public int Power { get; set; }
        public int Toughness { get; set; }
        public int Set { get; set; }
        public List<int> Type { get; set; } = new List<int>();
    }
}
