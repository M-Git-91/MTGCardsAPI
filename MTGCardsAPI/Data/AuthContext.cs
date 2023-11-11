using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace MTGCardsAPI.Data
{
    public class AuthContext : IdentityDbContext
    {
        public AuthContext(DbContextOptions<AuthContext> options) : base(options)
        {
            
        }
    }
}
