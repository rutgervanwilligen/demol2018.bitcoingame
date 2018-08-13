using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DeMol2018.BitcoinGame.DAL.Migrations
{
    public partial class TransactionTimestampsMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Timestamp",
                table: "OutgoingTransactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Timestamp",
                table: "IncomingTransactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "OutgoingTransactions");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "IncomingTransactions");
        }
    }
}
