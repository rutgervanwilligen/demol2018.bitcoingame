﻿// <auto-generated />
using System;
using DeMol2018.BitcoinGame.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DeMol2018.BitcoinGame.DAL.Migrations
{
    [DbContext(typeof(BitcoinGameDbContext))]
    partial class BitcoinGameDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.1-rtm-30846")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DeMol2018.BitcoinGame.DAL.Entities.GameEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("HasFinished");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("DeMol2018.BitcoinGame.DAL.Entities.PlayerEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("DeMol2018.BitcoinGame.DAL.Entities.RoundEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("GameId");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("GameId");

                    b.ToTable("Rounds");
                });

            modelBuilder.Entity("DeMol2018.BitcoinGame.DAL.Entities.TransactionEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Amount");

                    b.Property<int>("ReceiverId");

                    b.Property<int>("RoundId");

                    b.Property<int>("SenderId");

                    b.HasKey("Id");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("RoundId");

                    b.HasIndex("SenderId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("DeMol2018.BitcoinGame.DAL.Entities.WalletEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<Guid>("PlayerId");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.ToTable("Wallets");
                });

            modelBuilder.Entity("DeMol2018.BitcoinGame.DAL.Entities.RoundEntity", b =>
                {
                    b.HasOne("DeMol2018.BitcoinGame.DAL.Entities.GameEntity", "Game")
                        .WithMany("Rounds")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DeMol2018.BitcoinGame.DAL.Entities.TransactionEntity", b =>
                {
                    b.HasOne("DeMol2018.BitcoinGame.DAL.Entities.WalletEntity", "ReceiverWallet")
                        .WithMany("ReceivedTransactions")
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("DeMol2018.BitcoinGame.DAL.Entities.RoundEntity", "Round")
                        .WithMany("Transactions")
                        .HasForeignKey("RoundId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DeMol2018.BitcoinGame.DAL.Entities.WalletEntity", "SenderWallet")
                        .WithMany("SentTransactions")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("DeMol2018.BitcoinGame.DAL.Entities.WalletEntity", b =>
                {
                    b.HasOne("DeMol2018.BitcoinGame.DAL.Entities.PlayerEntity", "Player")
                        .WithMany("Wallets")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
