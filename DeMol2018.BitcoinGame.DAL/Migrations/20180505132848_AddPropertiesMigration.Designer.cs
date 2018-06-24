﻿// <auto-generated />
using DeMol2018.BitcoinGame.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace DeMol2018.BitcoinGame.DAL.Migrations
{
    [DbContext(typeof(BitcoinGameDbContext))]
    [Migration("20180505132848_AddPropertiesMigration")]
    partial class AddPropertiesMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Rounds");
                });

            modelBuilder.Entity("DeMol2018.BitcoinGame.DAL.Entities.TransactionEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ReceiverId");

                    b.Property<int>("RoundId");

                    b.Property<int>("SenderId");

                    b.Property<int>("Value");

                    b.HasKey("Id");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("RoundId");

                    b.HasIndex("SenderId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("DeMol2018.BitcoinGame.DAL.Entities.WalletEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("PlayerId");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.ToTable("Wallets");
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
