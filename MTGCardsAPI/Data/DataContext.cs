using Microsoft.EntityFrameworkCore;
using MTGCardsAPI.Models;

namespace MTGCardsAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext>options) : base(options)
        {
            
        }

        public DbSet<Ability> Abilities { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Colour> Colours { get; set; }
        public DbSet<Set> Sets { get; set; }
        public DbSet<CardType> CardTypes { get; set; }
    }
}
