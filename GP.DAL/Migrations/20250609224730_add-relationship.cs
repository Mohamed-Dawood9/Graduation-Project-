using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GP.DAL.Migrations
{
    public partial class addrelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppointmentId",
                table: "Analyses",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Analyses_AppointmentId",
                table: "Analyses",
                column: "AppointmentId",
                unique: true,
                filter: "[AppointmentId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Analyses_Appointments_AppointmentId",
                table: "Analyses",
                column: "AppointmentId",
                principalTable: "Appointments",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Analyses_Appointments_AppointmentId",
                table: "Analyses");

            migrationBuilder.DropIndex(
                name: "IX_Analyses_AppointmentId",
                table: "Analyses");

            migrationBuilder.DropColumn(
                name: "AppointmentId",
                table: "Analyses");
        }
    }
}
