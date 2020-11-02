using Microsoft.EntityFrameworkCore.Migrations;

namespace RunTogether.Data.Migrations
{
    public partial class pointAsVector5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "X",
                table: "ThroughPoints",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Y",
                table: "ThroughPoints",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "X",
                table: "StartPoints",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Y",
                table: "StartPoints",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "X",
                table: "EndPoints",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Y",
                table: "EndPoints",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "organiser",
                column: "ConcurrencyStamp",
                value: "199f994c-dcfb-4385-b24e-5e3fe2b8829a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "runner",
                column: "ConcurrencyStamp",
                value: "115777b3-b8a2-4bfe-a236-24d4dd665ba4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "X",
                table: "ThroughPoints");

            migrationBuilder.DropColumn(
                name: "Y",
                table: "ThroughPoints");

            migrationBuilder.DropColumn(
                name: "X",
                table: "StartPoints");

            migrationBuilder.DropColumn(
                name: "Y",
                table: "StartPoints");

            migrationBuilder.DropColumn(
                name: "X",
                table: "EndPoints");

            migrationBuilder.DropColumn(
                name: "Y",
                table: "EndPoints");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "organiser",
                column: "ConcurrencyStamp",
                value: "fb4eb839-92f4-4194-a2c7-ea01bbf462b2");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "runner",
                column: "ConcurrencyStamp",
                value: "5e3ebecb-dd2d-421b-a3c2-16e42adbd555");
        }
    }
}
