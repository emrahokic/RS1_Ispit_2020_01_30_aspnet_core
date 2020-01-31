using Microsoft.EntityFrameworkCore.Migrations;

namespace RS1_Ispit_asp.net_core.Migrations
{
    public partial class updateEM : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PredmetID",
                table: "Takmicenje",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Takmicenje_PredmetID",
                table: "Takmicenje",
                column: "PredmetID");

            migrationBuilder.AddForeignKey(
                name: "FK_Takmicenje_Predmet_PredmetID",
                table: "Takmicenje",
                column: "PredmetID",
                principalTable: "Predmet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Takmicenje_Predmet_PredmetID",
                table: "Takmicenje");

            migrationBuilder.DropIndex(
                name: "IX_Takmicenje_PredmetID",
                table: "Takmicenje");

            migrationBuilder.DropColumn(
                name: "PredmetID",
                table: "Takmicenje");
        }
    }
}
