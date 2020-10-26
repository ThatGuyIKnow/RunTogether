using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RunTogether.Data.Migrations
{
    public partial class initalMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Runs",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Runs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "RunRoutes",
                columns: table => new
                {
                    RunRouteId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RunId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RunRoutes", x => x.RunRouteId);
                    table.ForeignKey(
                        name: "FK_RunRoutes_Runs_RunId",
                        column: x => x.RunId,
                        principalTable: "Runs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stages",
                columns: table => new
                {
                    StageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    RunRouteId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stages", x => x.StageId);
                    table.ForeignKey(
                        name: "FK_Stages_RunRoutes_RunRouteId",
                        column: x => x.RunRouteId,
                        principalTable: "RunRoutes",
                        principalColumn: "RunRouteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EndPoints",
                columns: table => new
                {
                    EndPointId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StageId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EndPoints", x => x.EndPointId);
                    table.ForeignKey(
                        name: "FK_EndPoints_Stages_StageId",
                        column: x => x.StageId,
                        principalTable: "Stages",
                        principalColumn: "StageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StartPoints",
                columns: table => new
                {
                    StartPointId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StageId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StartPoints", x => x.StartPointId);
                    table.ForeignKey(
                        name: "FK_StartPoints_Stages_StageId",
                        column: x => x.StageId,
                        principalTable: "Stages",
                        principalColumn: "StageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ThroughPoints",
                columns: table => new
                {
                    ThroughPointId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StageId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThroughPoints", x => x.ThroughPointId);
                    table.ForeignKey(
                        name: "FK_ThroughPoints_Stages_StageId",
                        column: x => x.StageId,
                        principalTable: "Stages",
                        principalColumn: "StageId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EndPoints_StageId",
                table: "EndPoints",
                column: "StageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RunRoutes_RunId",
                table: "RunRoutes",
                column: "RunId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stages_RunRouteId",
                table: "Stages",
                column: "RunRouteId");

            migrationBuilder.CreateIndex(
                name: "IX_StartPoints_StageId",
                table: "StartPoints",
                column: "StageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ThroughPoints_StageId",
                table: "ThroughPoints",
                column: "StageId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EndPoints");

            migrationBuilder.DropTable(
                name: "StartPoints");

            migrationBuilder.DropTable(
                name: "ThroughPoints");

            migrationBuilder.DropTable(
                name: "Stages");

            migrationBuilder.DropTable(
                name: "RunRoutes");

            migrationBuilder.DropTable(
                name: "Runs");
        }
    }
}
