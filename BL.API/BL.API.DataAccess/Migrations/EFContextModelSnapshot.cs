﻿// <auto-generated />
using System;
using BL.API.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BL.API.DataAccess.Migrations
{
    [DbContext(typeof(EFContext))]
    partial class EFContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("dbo")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BL.API.Core.Domain.Logs.NLog", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Callsite")
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<string>("Exception")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Level")
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<DateTime>("Logged")
                        .HasColumnType("datetime2");

                    b.Property<string>("Logger")
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<string>("MachineName")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Properties")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ID");

                    b.ToTable("NLog");
                });

            modelBuilder.Entity("BL.API.Core.Domain.Match.Match", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<DateTime>("MatchDate")
                        .HasColumnType("datetime2");

                    b.Property<byte>("RoundsPlayed")
                        .HasColumnType("tinyint");

                    b.Property<string>("ScreenshotLink")
                        .HasColumnType("varchar(128)");

                    b.Property<Guid?>("SeasonId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte>("TeamWon")
                        .HasColumnType("tinyint");

                    b.HasKey("Id");

                    b.HasIndex("SeasonId");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("BL.API.Core.Domain.Match.PlayerMatchRecord", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<short?>("Assists")
                        .HasColumnType("smallint");

                    b.Property<byte?>("CalibrationIndex")
                        .HasColumnType("tinyint");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<byte?>("Deaths")
                        .HasColumnType("tinyint");

                    b.Property<int?>("Faction")
                        .HasColumnType("int");

                    b.Property<short?>("Kills")
                        .HasColumnType("smallint");

                    b.Property<int?>("MMRChange")
                        .HasColumnType("int");

                    b.Property<byte?>("MVPs")
                        .HasColumnType("tinyint");

                    b.Property<Guid>("MatchId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("PlayerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte>("RoundsWon")
                        .HasColumnType("tinyint");

                    b.Property<int?>("Score")
                        .HasColumnType("int");

                    b.Property<byte>("TeamIndex")
                        .HasColumnType("tinyint");

                    b.HasKey("Id");

                    b.HasIndex("MatchId");

                    b.HasIndex("PlayerId");

                    b.ToTable("PlayerMatchRecords");
                });

            modelBuilder.Entity("BL.API.Core.Domain.Match.Season", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<DateTime?>("Finished")
                        .HasColumnType("datetime2");

                    b.Property<int>("Index")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:IdentityIncrement", 1)
                        .HasAnnotation("SqlServer:IdentitySeed", 1)
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("OnGoing")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("Started")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Seasons");
                });

            modelBuilder.Entity("BL.API.Core.Domain.Player.Player", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Clan")
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("Country")
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<long?>("DiscordId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsIGL")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<int>("MainClass")
                        .HasColumnType("int");

                    b.Property<string>("Nickname")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<Guid>("PlayerMMRId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("SecondaryClass")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PlayerMMRId")
                        .IsUnique();

                    b.ToTable("Players");
                });

            modelBuilder.Entity("BL.API.Core.Domain.Player.PlayerMMR", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.Property<int>("MMR")
                        .HasColumnType("int");

                    b.Property<Guid>("PlayerId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("SeasonId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("SeasonId");

                    b.ToTable("PlayerMMR");
                });

            modelBuilder.Entity("BL.API.Core.Domain.Match.Match", b =>
                {
                    b.HasOne("BL.API.Core.Domain.Match.Season", "Season")
                        .WithMany()
                        .HasForeignKey("SeasonId");

                    b.Navigation("Season");
                });

            modelBuilder.Entity("BL.API.Core.Domain.Match.PlayerMatchRecord", b =>
                {
                    b.HasOne("BL.API.Core.Domain.Match.Match", "Match")
                        .WithMany("PlayerRecords")
                        .HasForeignKey("MatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BL.API.Core.Domain.Player.Player", "Player")
                        .WithMany()
                        .HasForeignKey("PlayerId");

                    b.Navigation("Match");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("BL.API.Core.Domain.Player.Player", b =>
                {
                    b.HasOne("BL.API.Core.Domain.Player.PlayerMMR", "PlayerMMR")
                        .WithOne("Player")
                        .HasForeignKey("BL.API.Core.Domain.Player.Player", "PlayerMMRId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PlayerMMR");
                });

            modelBuilder.Entity("BL.API.Core.Domain.Player.PlayerMMR", b =>
                {
                    b.HasOne("BL.API.Core.Domain.Match.Season", "Season")
                        .WithMany()
                        .HasForeignKey("SeasonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Season");
                });

            modelBuilder.Entity("BL.API.Core.Domain.Match.Match", b =>
                {
                    b.Navigation("PlayerRecords");
                });

            modelBuilder.Entity("BL.API.Core.Domain.Player.PlayerMMR", b =>
                {
                    b.Navigation("Player");
                });
#pragma warning restore 612, 618
        }
    }
}
