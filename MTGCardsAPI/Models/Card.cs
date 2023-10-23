using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public List<Colour> Colours { get; set; }
        public List<Ability>? Abilities { get; set; }
        [MaxLength(1500)]
        public string RulesText { get; set; } = string.Empty;
        [MaxLength(500)]
        public string? FlavourText { get; set; }
        public int Power { get; set; }
        public int Toughness { get; set; }
        public Set Set { get; set; }
        public List<CardType> Type { get; set; }
    }
}
