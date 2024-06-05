using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CamOn_FE.Migrations
{
    public partial class initial3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PostCountValue",
                table: "Packages",
                newName: "MonthValue");

            migrationBuilder.AlterColumn<int>(
                name: "CameraId",
                table: "CaptureImages",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CaptureImages_CameraId",
                table: "CaptureImages",
                column: "CameraId");

            migrationBuilder.AddForeignKey(
                name: "FK_CaptureImages_Cameras_CameraId",
                table: "CaptureImages",
                column: "CameraId",
                principalTable: "Cameras",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CaptureImages_Cameras_CameraId",
                table: "CaptureImages");

            migrationBuilder.DropIndex(
                name: "IX_CaptureImages_CameraId",
                table: "CaptureImages");

            migrationBuilder.RenameColumn(
                name: "MonthValue",
                table: "Packages",
                newName: "PostCountValue");

            migrationBuilder.AlterColumn<string>(
                name: "CameraId",
                table: "CaptureImages",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
