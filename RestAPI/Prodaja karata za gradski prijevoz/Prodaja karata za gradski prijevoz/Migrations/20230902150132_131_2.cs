using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prodaja_karata_za_gradski_prijevoz.Migrations
{
    public partial class _131_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RouteId",
                table: "IssuedTickets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_IssuedTickets_RouteId",
                table: "IssuedTickets",
                column: "RouteId");

            migrationBuilder.AddForeignKey(
                name: "FK_IssuedTickets_Routes_RouteId",
                table: "IssuedTickets",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IssuedTickets_Routes_RouteId",
                table: "IssuedTickets");

            migrationBuilder.DropIndex(
                name: "IX_IssuedTickets_RouteId",
                table: "IssuedTickets");

            migrationBuilder.DropColumn(
                name: "RouteId",
                table: "IssuedTickets");
        }
    }
}
