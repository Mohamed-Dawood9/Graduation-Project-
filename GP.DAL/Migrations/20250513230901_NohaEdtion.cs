using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GP.DAL.Migrations
{
    public partial class NohaEdtion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Analyses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    DoctorId = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OriginalPhotoPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcessedPhotoPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HDI_S = table.Column<float>(type: "real", nullable: false),
                    HDI_A = table.Column<float>(type: "real", nullable: false),
                    HDI_T = table.Column<float>(type: "real", nullable: false),
                    FAI_C7 = table.Column<float>(type: "real", nullable: false),
                    FAI_A = table.Column<float>(type: "real", nullable: false),
                    FAI_T = table.Column<float>(type: "real", nullable: false),
                    TotalHDI = table.Column<float>(type: "real", nullable: false),
                    TotalFAI = table.Column<float>(type: "real", nullable: false),
                    POTSI = table.Column<float>(type: "real", nullable: false),
                    LeftSideDistance = table.Column<float>(type: "real", nullable: false),
                    RightSideDistance = table.Column<float>(type: "real", nullable: false),
                    LeftUnderArmDistance = table.Column<float>(type: "real", nullable: false),
                    RightUnderArmDistance = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Analyses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Analyses_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KeyPoints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnalysisId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    X = table.Column<float>(type: "real", nullable: false),
                    Y = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeyPoints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KeyPoints_Analyses_AnalysisId",
                        column: x => x.AnalysisId,
                        principalTable: "Analyses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Analyses_PatientId",
                table: "Analyses",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_KeyPoints_AnalysisId",
                table: "KeyPoints",
                column: "AnalysisId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KeyPoints");

            migrationBuilder.DropTable(
                name: "Analyses");
        }
    }
}
