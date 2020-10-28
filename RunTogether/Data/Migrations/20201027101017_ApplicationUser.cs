using Microsoft.EntityFrameworkCore.Migrations;

namespace RunTogether.Data.Migrations
{
    public partial class ApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "54490d0e-b340-40cc-baf2-56f2c8c18456");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8318451e-78ae-49f0-94a8-1d76921e7788");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ffc1791c-be68-4e2d-b7e3-fe17dd2eaf16", "dd842761-e1a1-4f8d-a906-bfa3d40481c7", "Runner", "RUNNER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "3463be73-0ab5-4d7f-abcf-257f695d49e6", "376bab79-3539-49a1-b9b8-e72e1f59226a", "Organiser", "ORGANISER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3463be73-0ab5-4d7f-abcf-257f695d49e6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ffc1791c-be68-4e2d-b7e3-fe17dd2eaf16");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "54490d0e-b340-40cc-baf2-56f2c8c18456", "eb3efbdd-abd6-47ce-a688-c15736aa2ef6", "Runner", "RUNNER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "8318451e-78ae-49f0-94a8-1d76921e7788", "48f5efe4-9904-4cea-b2ec-c2a980fd3f35", "Organiser", "ORGANISER" });
        }
    }
}
