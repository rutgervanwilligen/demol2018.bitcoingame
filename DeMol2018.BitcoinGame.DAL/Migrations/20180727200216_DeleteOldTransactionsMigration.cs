using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DeMol2018.BitcoinGame.DAL.Migrations
{
    public partial class DeleteOldTransactionsMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Amount = table.Column<int>(nullable: false),
                    GameId = table.Column<Guid>(nullable: false),
                    InvalidReceiverAddress = table.Column<int>(nullable: true),
                    ReceiverId = table.Column<Guid>(nullable: true),
                    ReceiverWalletId = table.Column<Guid>(nullable: true),
                    RoundId = table.Column<Guid>(nullable: false),
                    RoundNumber = table.Column<int>(nullable: false),
                    SenderId = table.Column<Guid>(nullable: false),
                    SenderWalletId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Wallets_ReceiverWalletId",
                        column: x => x.ReceiverWalletId,
                        principalTable: "Wallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Transactions_Rounds_RoundId",
                        column: x => x.RoundId,
                        principalTable: "Rounds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_Wallets_SenderWalletId",
                        column: x => x.SenderWalletId,
                        principalTable: "Wallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ReceiverWalletId",
                table: "Transactions",
                column: "ReceiverWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_RoundId",
                table: "Transactions",
                column: "RoundId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_SenderWalletId",
                table: "Transactions",
                column: "SenderWalletId");
        }
    }
}
