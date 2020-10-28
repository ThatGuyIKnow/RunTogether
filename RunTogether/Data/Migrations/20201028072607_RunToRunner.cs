using Microsoft.EntityFrameworkCore.Migrations;

namespace RunTogether.Data.Migrations
{
    public partial class RunToRunner : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Runs_RunId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "RunId",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "organiser",
                column: "ConcurrencyStamp",
                value: "3a94d61f-f55c-45f3-9d16-1294f8a4671b");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "runner",
                column: "ConcurrencyStamp",
                value: "cf0cc906-abff-4c71-b14f-c5edb054e203");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Runs_RunId",
                table: "AspNetUsers",
                column: "RunId",
                principalTable: "Runs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Runs_RunId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "RunId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "organiser",
                column: "ConcurrencyStamp",
                value: "ef594953-da07-487a-be28-b6231dc34957");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "runner",
                column: "ConcurrencyStamp",
                value: "04a6b69d-0d61-47ae-b8a7-4a6ff885e830");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Runs_RunId",
                table: "AspNetUsers",
                column: "RunId",
                principalTable: "Runs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
