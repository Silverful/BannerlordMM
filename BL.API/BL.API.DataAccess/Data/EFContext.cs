using BL.API.Core.Domain.Logs;
using BL.API.Core.Domain.Match;
using BL.API.Core.Domain.Player;
using BL.API.Core.Domain.Settings;
using BL.API.Core.Domain.User;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BL.API.DataAccess.Data
{
    //dotnet ef migrations add InitialCreate -s..\BL.API.WebHost\BL.API.WebHost.csproj
    //dotnet ef database update -s..\BL.API.WebHost\BL.API.WebHost.csproj
    public class EFContext : IdentityDbContext<User>
    {
        public EFContext(DbContextOptions options) : base(options) {}
        public virtual DbSet<Player> Players { get; protected set; }
        public virtual DbSet<Match> Matches { get; protected set; }
        public virtual DbSet<PlayerMatchRecord> PlayerMatchRecords { get; protected set; }
        public virtual DbSet<Season> Seasons { get; protected set; }
        public virtual DbSet<PlayerMMR> PlayerMMR { get; protected set; }
        public virtual DbSet<Configuration> Configurations { get; protected set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
#if DEBUG
            optionsBuilder.EnableSensitiveDataLogging();
#endif
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");

            modelBuilder.Entity<Configuration>()
                .Property(l => l.Created)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Configuration>()
                .Navigation(c => c.Region)
                .AutoInclude();

            modelBuilder.Entity<NLog>()
                .Property(l => l.ID)
                .UseIdentityColumn(1, 1);

            modelBuilder.Entity<Region>()
                .Property(r => r.Created)
                .HasDefaultValueSql("getdate()");


            modelBuilder.Entity<Season>()
                .Property(l => l.Index)
                .UseIdentityColumn(1, 1);

            modelBuilder.Entity<Season>()
                .Property(l => l.Index)
                .Metadata
                .SetAfterSaveBehavior(Microsoft.EntityFrameworkCore.Metadata.PropertySaveBehavior.Ignore);

            modelBuilder.Entity<Season>()
                .Property(l => l.Created)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Season>()
                .Property(l => l.IsTestingSeason)
                .HasDefaultValueSql("0");

            modelBuilder.Entity<Season>()
                .Navigation(l => l.Region)
                .AutoInclude();


            modelBuilder.Entity<PlayerMMR>()
                .Property(l => l.Created)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<PlayerMMR>()
                .Navigation(l => l.Season)
                .AutoInclude();

            modelBuilder.Entity<PlayerMMR>()
                .Navigation(l => l.Region)
                .AutoInclude();


            modelBuilder.Entity<Player>()
                .Property(p => p.Created)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Player>()
                .Property(p => p.IsIGL)
                .HasDefaultValue(false);

            modelBuilder.Entity<Player>()
                .Navigation(p => p.PlayerMMRs)
                .AutoInclude();


            modelBuilder.Entity<Match>()
                .Property(p => p.Created)
                .HasDefaultValueSql("getdate()");

            //modelBuilder.Entity<Match>()
            //    .Navigation(p => p.PlayerRecords)
            //    .AutoInclude();

            modelBuilder.Entity<Match>()
                .Navigation(p => p.Season)
                .AutoInclude();

            modelBuilder.Entity<Match>()
                .Navigation(p => p.Region)
                .AutoInclude();

            modelBuilder.Entity<Match>()
                .HasIndex(p => p.ScreenshotLink)
                .IsUnique();


            modelBuilder.Entity<PlayerMatchRecord>()
                .Property(p => p.Created)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<PlayerMatchRecord>()
                .Navigation(p => p.Player)
                .AutoInclude();
        }
    }
}
