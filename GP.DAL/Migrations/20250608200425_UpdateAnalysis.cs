using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GP.DAL.Migrations
{
    public partial class UpdateAnalysis : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeftSideDistance",
                table: "Analyses");

            migrationBuilder.DropColumn(
                name: "LeftUnderArmDistance",
                table: "Analyses");

            migrationBuilder.DropColumn(
                name: "POTSI",
                table: "Analyses");

            migrationBuilder.DropColumn(
                name: "RightSideDistance",
                table: "Analyses");

            migrationBuilder.DropColumn(
                name: "RightUnderArmDistance",
                table: "Analyses");

            migrationBuilder.DropColumn(
                name: "TotalFAI",
                table: "Analyses");

            migrationBuilder.DropColumn(
                name: "TotalHDI",
                table: "Analyses");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "LeftSideDistance",
                table: "Analyses",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "LeftUnderArmDistance",
                table: "Analyses",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "POTSI",
                table: "Analyses",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "RightSideDistance",
                table: "Analyses",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "RightUnderArmDistance",
                table: "Analyses",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "TotalFAI",
                table: "Analyses",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "TotalHDI",
                table: "Analyses",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
