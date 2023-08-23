using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prodaja_karata_za_gradski_prijevoz.Migrations
{
    public partial class _67_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "IssuedTickets");

            migrationBuilder.AddColumn<Guid>(
                name: "TaxId",
                table: "Invoices",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<double>(
                name: "TotalWithoutTax",
                table: "Invoices",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "Taxes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Percentage = table.Column<double>(type: "float(5)", precision: 5, scale: 2, nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Taxes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Taxes",
                columns: new[] { "Id", "Active", "Name", "Percentage" },
                values: new object[] { new Guid("e363863b-ba6d-477f-9afb-15dcbf70616b"), true, "PDV", 0.17000000000000001 });

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_TaxId",
                table: "Invoices",
                column: "TaxId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Taxes_TaxId",
                table: "Invoices",
                column: "TaxId",
                principalTable: "Taxes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Taxes_TaxId",
                table: "Invoices");

            migrationBuilder.DropTable(
                name: "Taxes");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_TaxId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "TaxId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "TotalWithoutTax",
                table: "Invoices");

            migrationBuilder.AddColumn<int>(
                name: "Amount",
                table: "IssuedTickets",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
