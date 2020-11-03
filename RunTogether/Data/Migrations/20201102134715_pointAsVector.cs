using Microsoft.EntityFrameworkCore.Migrations;

namespace RunTogether.Data.Migrations
{
    public partial class pointAsVector : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Coordinates",
                table: "ThroughPoints",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Coordinates",
                table: "StartPoints",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Coordinates",
                table: "EndPoints",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "organiser",
                column: "ConcurrencyStamp",
                value: "ea5460c4-70dc-4149-a541-ae94eaddd2c8");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "runner",
                column: "ConcurrencyStamp",
                value: "5705ba43-e468-40a2-9a77-ff9a32f0566e");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Coordinates",
                table: "ThroughPoints");

            migrationBuilder.DropColumn(
                name: "Coordinates",
                table: "StartPoints");

            migrationBuilder.DropColumn(
                name: "Coordinates",
                table: "EndPoints");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "organiser",
                column: "ConcurrencyStamp",
                value: "e04a036a-f636-4c53-b033-9a67538c044a");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "runner",
                column: "ConcurrencyStamp",
                value: "f3612e97-82fc-4f15-9b30-ca35cfe91b01");
        }
    }
}
