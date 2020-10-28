using Microsoft.EntityFrameworkCore.Migrations;

namespace RunTogether.Data.Migrations
{
    public partial class RunNextRunnerId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bb84aef6-18db-4ea7-8c5f-543de6d7947b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fe04126a-4943-4af4-87ab-188ebac56715");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a03e22c2-61ac-4d34-9cec-b5c81a48c56c", "fec3bb7c-8cec-4d21-94c4-a275badf9b6a", "Runner", "RUNNER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "6c8536d5-a4a5-4caf-a21d-d0ba775bcbf8", "6d991241-c53f-4cff-b27e-688cc2b9bdd0", "Organiser", "ORGANISER" });
        }
    }
}
