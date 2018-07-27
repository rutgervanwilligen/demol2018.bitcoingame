using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DeMol2018.BitcoinGame.DAL.Migrations
{
    public partial class AddIncomingOutgoingTransactionsMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Wallets_ReceiverId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Wallets_SenderId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_ReceiverId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_SenderId",
                table: "Transactions");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Transactions",
                nullable: false,
                oldClrType: typeof(Guid),
                oldDefaultValueSql: "NEWID()");

            migrationBuilder.AddColumn<Guid>(
                name: "GameId",
                table: "Transactions",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ReceiverWalletId",
                table: "Transactions",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SenderWalletId",
                table: "Transactions",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "IncomingTransactionEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ReceiverWalletId = table.Column<Guid>(nullable: false),
                    SenderWalletId = table.Column<Guid>(nullable: false),
                    Amount = table.Column<int>(nullable: false),
                    GameId = table.Column<Guid>(nullable: false),
                    RoundId = table.Column<Guid>(nullable: false),
                    RoundNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomingTransactionEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncomingTransactionEntity_Wallets_ReceiverWalletId",
                        column: x => x.ReceiverWalletId,
                        principalTable: "Wallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OutgoingTransactionEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    SenderWalletId = table.Column<Guid>(nullable: false),
                    ReceiverWalletId = table.Column<Guid>(nullable: true),
                    Amount = table.Column<int>(nullable: false),
                    GameId = table.Column<Guid>(nullable: false),
                    RoundId = table.Column<Guid>(nullable: false),
                    RoundNumber = table.Column<int>(nullable: false),
                    InvalidReceiverAddress = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutgoingTransactionEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutgoingTransactionEntity_Wallets_SenderWalletId",
                        column: x => x.SenderWalletId,
                        principalTable: "Wallets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ReceiverWalletId",
                table: "Transactions",
                column: "ReceiverWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_SenderWalletId",
                table: "Transactions",
                column: "SenderWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_IncomingTransactionEntity_ReceiverWalletId",
                table: "IncomingTransactionEntity",
                column: "ReceiverWalletId");

            migrationBuilder.CreateIndex(
                name: "IX_OutgoingTransactionEntity_SenderWalletId",
                table: "OutgoingTransactionEntity",
                column: "SenderWalletId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Wallets_ReceiverWalletId",
                table: "Transactions",
                column: "ReceiverWalletId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Wallets_SenderWalletId",
                table: "Transactions",
                column: "SenderWalletId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Wallets_ReceiverWalletId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Wallets_SenderWalletId",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "IncomingTransactionEntity");

            migrationBuilder.DropTable(
                name: "OutgoingTransactionEntity");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_ReceiverWalletId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_SenderWalletId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "ReceiverWalletId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "SenderWalletId",
                table: "Transactions");

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Transactions",
                nullable: false,
                defaultValueSql: "NEWID()",
                oldClrType: typeof(Guid));

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ReceiverId",
                table: "Transactions",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_SenderId",
                table: "Transactions",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Wallets_ReceiverId",
                table: "Transactions",
                column: "ReceiverId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Wallets_SenderId",
                table: "Transactions",
                column: "SenderId",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
