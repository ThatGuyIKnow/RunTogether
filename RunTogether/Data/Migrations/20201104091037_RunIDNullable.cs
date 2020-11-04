using Microsoft.EntityFrameworkCore.Migrations;

namespace RunTogether.Data.Migrations
{
    public partial class RunIDNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RunRoutes_Runs_RunId",
                table: "RunRoutes");

            migrationBuilder.DropIndex(
                name: "IX_RunRoutes_RunId",
                table: "RunRoutes");

            migrationBuilder.AlterColumn<int>(
                name: "RunId",
                table: "RunRoutes",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "organiser",
                column: "ConcurrencyStamp",
                value: "98df4dd7-26a1-4c3f-b416-6b08b6b52acd");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "runner",
                column: "ConcurrencyStamp",
                value: "93fdb73e-dbf4-45a9-a0b6-13056b38d6ce");

            migrationBuilder.CreateIndex(
                name: "IX_RunRoutes_RunId",
                table: "RunRoutes",
                column: "RunId",
                unique: true,
                filter: "[RunId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_RunRoutes_Runs_RunId",
                table: "RunRoutes",
                column: "RunId",
                principalTable: "Runs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RunRoutes_Runs_RunId",
                table: "RunRoutes");

            migrationBuilder.DropIndex(
                name: "IX_RunRoutes_RunId",
                table: "RunRoutes");

            migrationBuilder.AlterColumn<int>(
                name: "RunId",
                table: "RunRoutes",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_RunRoutes_RunId",
                table: "RunRoutes",
                column: "RunId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RunRoutes_Runs_RunId",
                table: "RunRoutes",
                column: "RunId",
                principalTable: "Runs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
