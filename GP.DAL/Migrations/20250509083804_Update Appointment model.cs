using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GP.DAL.Migrations
{
    public partial class UpdateAppointmentmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProccessedPhotoPath",
                table: "Appointments",
                newName: "ProcessedPhotoPath3");

            migrationBuilder.AddColumn<string>(
                name: "ProcessedPhotoPath1",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProcessedPhotoPath2",
                table: "Appointments",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProcessedPhotoPath1",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "ProcessedPhotoPath2",
                table: "Appointments");

            migrationBuilder.RenameColumn(
                name: "ProcessedPhotoPath3",
                table: "Appointments",
                newName: "ProccessedPhotoPath");
        }
    }
}
