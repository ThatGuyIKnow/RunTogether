using Microsoft.EntityFrameworkCore.Migrations;

namespace RunTogether.Data.Migrations
{
    public partial class WorkingMap : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "organiser",
                column: "ConcurrencyStamp",
                value: "2d4f6f1a-0ff7-4673-8a58-f889fefce996");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "runner",
                column: "ConcurrencyStamp",
                value: "71fc51bb-b9f9-484b-adfa-bbb5427d2af7");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
