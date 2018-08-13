using Microsoft.EntityFrameworkCore.Migrations;

namespace DeMol2018.BitcoinGame.DAL.Migrations
{
    public partial class IsCurrentGameMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Games_StartTime_HasFinished",
                table: "Games");

            migrationBuilder.AddColumn<bool>(
                name: "IsCurrentGame",
                table: "Games",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCurrentGame",
                table: "Games");

            migrationBuilder.CreateIndex(
                name: "IX_Games_StartTime_HasFinished",
                table: "Games",
                columns: new[] { "StartTime", "HasFinished" });
        }
    }
}
