using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MTGCardsAPI.DTO
{
    public class AbilityDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(200)]
        public string Description { get; set; } = string.Empty;
    }
}
