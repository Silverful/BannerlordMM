using BL.API.Core.Domain.Match;
using BL.API.Core.Domain.Player;
using Microsoft.EntityFrameworkCore;

namespace BL.API.DataAccess.Data
{
    //dotnet ef migrations add InitialCreate -s..\BL.API.WebHost\BL.API.WebHost.csproj
    //dotnet ef database update -s..\BL.API.WebHost\BL.API.WebHost.csproj
    public class EFContext : DbContext
    {
        public EFContext(DbContextOptions<EFContext> options) : base(options) { }

        public virtual DbSet<Player> Players { get; protected set; }
        public virtual DbSet<Match> Matches { get; protected set; }
        public virtual DbSet<PlayerMatchRecord> PlayerMatchRecords { get; protected set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
