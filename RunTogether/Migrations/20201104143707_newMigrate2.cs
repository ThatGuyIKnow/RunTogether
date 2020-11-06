using Microsoft.EntityFrameworkCore.Migrations;

namespace RunTogether.Migrations
{
    public partial class newMigrate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "organiser",
                column: "ConcurrencyStamp",
                value: "5790124a-d127-40ed-9221-644bb254fe66");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "runner",
                column: "ConcurrencyStamp",
                value: "f1c6de80-d725-4da8-b84c-6bfc63de7472");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "organiser",
                column: "ConcurrencyStamp",
                value: "62cf713b-8521-46bc-9402-82374a0e556e");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "runner",
                column: "ConcurrencyStamp",
                value: "e78687cd-82b1-433e-811e-7f2c662369ed");
        }
    }
}
