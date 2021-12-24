using BL.API.Core.Domain.Logs;
using BL.API.Core.Domain.Match;
using BL.API.Core.Domain.Player;
using Microsoft.EntityFrameworkCore;
using System;

namespace BL.API.DataAccess.Data
{
    //dotnet ef migrations add InitialCreate -s..\BL.API.WebHost\BL.API.WebHost.csproj
    //dotnet ef database update -s..\BL.API.WebHost\BL.API.WebHost.csproj
    public class EFContext : DbContext
    {
        public EFContext(DbContextOptions options) : base(options) 
        {
        }

        public virtual DbSet<Player> Players { get; protected set; }
        public virtual DbSet<Match> Matches { get; protected set; }
        public virtual DbSet<PlayerMatchRecord> PlayerMatchRecords { get; protected set; }
        public virtual DbSet<Season> Seasons { get; protected set; }
        public virtual DbSet<PlayerMMR> PlayerMMR { get; protected set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");

            modelBuilder.Entity<NLog>()
                .Property(l => l.ID)
                .UseIdentityColumn(1, 1);

            modelBuilder.Entity<Season>()
                .Property(l => l.Index)
                .UseIdentityColumn(1, 1);

            modelBuilder.Entity<PlayerMMR>()
                .Property(l => l.Created)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Season>()
                .Property(l => l.Created)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Match>()
                .Property(p => p.Created)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Player>()
                .Property(p => p.Created)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Player>()
                .Property(p => p.IsIGL)
                .HasDefaultValue(false);

            modelBuilder.Entity<PlayerMatchRecord>()
                .Property(p => p.Created)
                .HasDefaultValueSql("getdate()");

            modelBuilder.Entity<Player>()
                .Navigation(p => p.PlayerMMR)
                .AutoInclude();

            modelBuilder.Entity<PlayerMatchRecord>()
                .Navigation(p => p.Player)
                .AutoInclude();

            modelBuilder.Entity<PlayerMatchRecord>()
                .Navigation(p => p.Match)
                .AutoInclude();

            modelBuilder.Entity<Match>()
                .Navigation(p => p.PlayerRecords)
                .AutoInclude();

            modelBuilder.Entity<Match>()
                .Navigation(p => p.Season)
                .AutoInclude();
        }
    }
}
