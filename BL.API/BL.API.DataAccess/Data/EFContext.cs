using BL.API.Core.Domain.Match;
using BL.API.Core.Domain.Player;
using Microsoft.EntityFrameworkCore;

namespace BL.API.DataAccess.Data
{
    public class EFContext : DbContext
    {
        public EFContext(DbContextOptions<EFContext> options) : base(options) { }

        public virtual DbSet<Player> Players { get; protected set; }
        public virtual DbSet<Match> Matches { get; protected set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
