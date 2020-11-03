using Microsoft.EntityFrameworkCore.Migrations;

namespace RunTogether.Data.Migrations
{
    public partial class pointAsVector4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EndPoints_Coordinate_CoordinateId",
                table: "EndPoints");

            migrationBuilder.DropForeignKey(
                name: "FK_StartPoints_Coordinate_CoordinateId",
                table: "StartPoints");

            migrationBuilder.DropForeignKey(
                name: "FK_ThroughPoints_Coordinate_CoordinateId",
                table: "ThroughPoints");

            migrationBuilder.DropTable(
                name: "Coordinate");

            migrationBuilder.DropIndex(
                name: "IX_ThroughPoints_CoordinateId",
                table: "ThroughPoints");

            migrationBuilder.DropIndex(
                name: "IX_StartPoints_CoordinateId",
                table: "StartPoints");

            migrationBuilder.DropIndex(
                name: "IX_EndPoints_CoordinateId",
                table: "EndPoints");

            migrationBuilder.DropColumn(
                name: "CoordinateId",
                table: "ThroughPoints");

            migrationBuilder.DropColumn(
                name: "CoordinateId",
                table: "StartPoints");

            migrationBuilder.DropColumn(
                name: "CoordinateId",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CoordinateId",
                table: "ThroughPoints",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CoordinateId",
                table: "StartPoints",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CoordinateId",
                table: "EndPoints",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Coordinate",
                columns: table => new
                {
                    CoordinateId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    X = table.Column<float>(type: "real", nullable: false),
                    Y = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coordinate", x => x.CoordinateId);
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "organiser",
                column: "ConcurrencyStamp",
                value: "819e2cc0-d83b-48a0-b40a-eb76b5caa52d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "runner",
                column: "ConcurrencyStamp",
                value: "3a5a93a7-7c07-49bf-a0f4-8250620fb353");

            migrationBuilder.CreateIndex(
                name: "IX_ThroughPoints_CoordinateId",
                table: "ThroughPoints",
                column: "CoordinateId");

            migrationBuilder.CreateIndex(
                name: "IX_StartPoints_CoordinateId",
                table: "StartPoints",
                column: "CoordinateId");

            migrationBuilder.CreateIndex(
                name: "IX_EndPoints_CoordinateId",
                table: "EndPoints",
                column: "CoordinateId");

            migrationBuilder.AddForeignKey(
                name: "FK_EndPoints_Coordinate_CoordinateId",
                table: "EndPoints",
                column: "CoordinateId",
                principalTable: "Coordinate",
                principalColumn: "CoordinateId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StartPoints_Coordinate_CoordinateId",
                table: "StartPoints",
                column: "CoordinateId",
                principalTable: "Coordinate",
                principalColumn: "CoordinateId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ThroughPoints_Coordinate_CoordinateId",
                table: "ThroughPoints",
                column: "CoordinateId",
                principalTable: "Coordinate",
                principalColumn: "CoordinateId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
