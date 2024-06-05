using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CamOn_FE.Migrations
{
    public partial class initial2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cameras_Cameras_CameraId",
                table: "Cameras");

            migrationBuilder.DropForeignKey(
                name: "FK_Cameras_CaptureImages_CaptureImageId",
                table: "Cameras");

            migrationBuilder.DropForeignKey(
                name: "FK_Cameras_Packages_PackageId",
                table: "Cameras");

            migrationBuilder.DropIndex(
                name: "IX_Cameras_CameraId",
                table: "Cameras");

            migrationBuilder.DropIndex(
                name: "IX_Cameras_CaptureImageId",
                table: "Cameras");

            migrationBuilder.DropIndex(
                name: "IX_Cameras_PackageId",
                table: "Cameras");

            migrationBuilder.DropColumn(
                name: "CameraId",
                table: "Cameras");

            migrationBuilder.DropColumn(
                name: "CaptureImageId",
                table: "Cameras");

            migrationBuilder.DropColumn(
                name: "PackageId",
                table: "Cameras");

            migrationBuilder.AlterColumn<string>(
                name: "AccountId",
                table: "Cameras",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cameras_AccountId",
                table: "Cameras",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cameras_AspNetUsers_AccountId",
                table: "Cameras",
                column: "AccountId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cameras_AspNetUsers_AccountId",
                table: "Cameras");

            migrationBuilder.DropIndex(
                name: "IX_Cameras_AccountId",
                table: "Cameras");

            migrationBuilder.AlterColumn<string>(
                name: "AccountId",
                table: "Cameras",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CameraId",
                table: "Cameras",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CaptureImageId",
                table: "Cameras",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PackageId",
                table: "Cameras",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cameras_CameraId",
                table: "Cameras",
                column: "CameraId");

            migrationBuilder.CreateIndex(
                name: "IX_Cameras_CaptureImageId",
                table: "Cameras",
                column: "CaptureImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Cameras_PackageId",
                table: "Cameras",
                column: "PackageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cameras_Cameras_CameraId",
                table: "Cameras",
                column: "CameraId",
                principalTable: "Cameras",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cameras_CaptureImages_CaptureImageId",
                table: "Cameras",
                column: "CaptureImageId",
                principalTable: "CaptureImages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Cameras_Packages_PackageId",
                table: "Cameras",
                column: "PackageId",
                principalTable: "Packages",
                principalColumn: "Id");
        }
    }
}
