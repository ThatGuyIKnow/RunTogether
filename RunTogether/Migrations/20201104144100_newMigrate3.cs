using Microsoft.EntityFrameworkCore.Migrations;

namespace RunTogether.Migrations
{
    public partial class newMigrate3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "organiser",
                column: "ConcurrencyStamp",
                value: "96664bbd-d881-4244-b18e-5514edfe81f5");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "runner",
                column: "ConcurrencyStamp",
                value: "28c8412d-20b4-4b13-b964-092076f3ef1d");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
