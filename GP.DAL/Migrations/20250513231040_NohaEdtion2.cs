using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GP.DAL.Migrations
{
    public partial class NohaEdtion2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnalysisId",
                table: "Notes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notes_AnalysisId",
                table: "Notes",
                column: "AnalysisId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Analyses_AnalysisId",
                table: "Notes",
                column: "AnalysisId",
                principalTable: "Analyses",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Analyses_AnalysisId",
                table: "Notes");

            migrationBuilder.DropIndex(
                name: "IX_Notes_AnalysisId",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "AnalysisId",
                table: "Notes");
        }
    }
}
