﻿// <auto-generated />
using System;
using BL.API.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BL.API.DataAccess.Migrations
{
    [DbContext(typeof(EFContext))]
    [Migration("20211213195446_MatchesUpdated")]
    partial class MatchesUpdated
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
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
                        .HasDefaultValue(new DateTime(2021, 12, 13, 19, 54, 46, 367, DateTimeKind.Utc).AddTicks(6878));

                    b.Property<DateTime>("MatchDate")
                        .HasColumnType("datetime2");

                    b.Property<byte>("RoundsPlayed")
                        .HasColumnType("tinyint");

                    b.Property<string>("ScreenshotLink")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte>("TeamWon")
                        .HasColumnType("tinyint");

                    b.HasKey("Id");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("BL.API.Core.Domain.Match.PlayerMatchRecord", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte?>("Assists")
                        .HasColumnType("tinyint");

                    b.Property<DateTime>("Created")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2021, 12, 13, 19, 54, 46, 373, DateTimeKind.Utc).AddTicks(436));

                    b.Property<byte?>("Deaths")
                        .HasColumnType("tinyint");

                    b.Property<int>("Faction")
                        .HasColumnType("int");

                    b.Property<byte?>("Kills")
                        .HasColumnType("tinyint");

                    b.Property<int>("MMRChange")
                        .HasColumnType("int");

                    b.Property<byte?>("MVPs")
                        .HasColumnType("tinyint");

                    b.Property<Guid>("MatchId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PlayerId")
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
                        .HasDefaultValue(new DateTime(2021, 12, 13, 19, 54, 46, 372, DateTimeKind.Utc).AddTicks(9325));

                    b.Property<int>("DiscordId")
                        .HasColumnType("int");

                    b.Property<int>("MainClass")
                        .HasColumnType("int");

                    b.Property<string>("Nickname")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<int>("PlayerMMR")
                        .HasColumnType("int");

                    b.Property<int>("SecondaryClass")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Players");
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
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Match");

                    b.Navigation("Player");
                });

            modelBuilder.Entity("BL.API.Core.Domain.Match.Match", b =>
                {
                    b.Navigation("PlayerRecords");
                });
#pragma warning restore 612, 618
        }
    }
}
