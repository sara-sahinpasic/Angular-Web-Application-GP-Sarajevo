using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prodaja_karata_za_gradski_prijevoz.Migrations
{
    public partial class _111_1: Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Tickets_TicketId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Users_RoleId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_TicketId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "TicketId",
                table: "Invoices");

            migrationBuilder.RenameColumn(
                name: "PurchaseDate",
                table: "Invoices",
                newName: "InvoicingDate");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Invoices",
                newName: "Total");

            migrationBuilder.CreateTable(
                name: "IssuedTickets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TicketId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IssuedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InvoiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IssuedTickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IssuedTickets_Invoices_InvoiceId",
                        column: x => x.InvoiceId,
                        principalTable: "Invoices",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_IssuedTickets_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IssuedTickets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_IssuedTickets_InvoiceId",
                table: "IssuedTickets",
                column: "InvoiceId");

            migrationBuilder.CreateIndex(
                name: "IX_IssuedTickets_TicketId",
                table: "IssuedTickets",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_IssuedTickets_UserId",
                table: "IssuedTickets",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IssuedTickets");

            migrationBuilder.DropIndex(
                name: "IX_Users_RoleId",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "Total",
                table: "Invoices",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "InvoicingDate",
                table: "Invoices",
                newName: "PurchaseDate");

            migrationBuilder.AddColumn<int>(
                name: "Amount",
                table: "Invoices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "TicketId",
                table: "Invoices",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_TicketId",
                table: "Invoices",
                column: "TicketId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Tickets_TicketId",
                table: "Invoices",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
