using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace MTGCardsAPI.Models
{
    public class Card
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        public string ImageURL { get; set; } = string.Empty;
        public List<Colour> Colours { get; set; } = new List<Colour>();
        //public List<char> ManaCost { get; set; } = new List<char> { ' ' };
        public List<Ability> Abilities { get; set; } = new List<Ability>();
        [MaxLength(1500)]
        public string RulesText { get; set; } = string.Empty;
        [MaxLength(500)]
        public string? FlavourText { get; set; }
        public int? Power { get; set; }
        public int? Toughness { get; set; }
        public Set Set { get; set; } = new Set();
        public List<CardType> Type { get; set; } = new List<CardType>();
    }
}
