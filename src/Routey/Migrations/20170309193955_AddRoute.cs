using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Routey.Migrations
{
    public partial class AddRoute : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Routes",
                columns: table => new
                {
                    RouteId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    SaveRoute = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => x.RouteId);
                });

            migrationBuilder.AddColumn<string>(
                name: "LocationType",
                table: "Locations",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RouteId",
                table: "Locations",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_RouteId",
                table: "Locations",
                column: "RouteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Routes_RouteId",
                table: "Locations",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "RouteId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Routes_RouteId",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_RouteId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "LocationType",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "RouteId",
                table: "Locations");

            migrationBuilder.DropTable(
                name: "Routes");
        }
    }
}
