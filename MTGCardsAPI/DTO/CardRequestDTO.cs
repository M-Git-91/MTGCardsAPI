using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MTGCardsAPI.DTO
{
    public class CardRequestDTO
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        public string ImageURL { get; set; } = string.Empty;
        [Required]
        [MinLength(1)]
        public List<int> Colours { get; set; }
        public List<int> Abilities { get; set; } = new List<int>();
        [MaxLength(1500)]
        public string RulesText { get; set; } = string.Empty;
        [MaxLength(500)]
        public string FlavourText { get; set; } = string.Empty;
        public int? Power { get; set; }
        public int? Toughness { get; set; }
        [Required]
        public int Set { get; set; }
        [Required]
        [MinLength(1)]
        public List<int> Type { get; set; } = new List<int>();
    }
}
