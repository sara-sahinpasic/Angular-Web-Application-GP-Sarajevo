using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prodaja_karata_za_gradski_prijevoz.Migrations
{
    public partial class _105_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PaymentOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentOptions", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "PaymentOptions",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("46536a11-f5b3-4505-a13a-e7d44dda9ae9"), "Mail" });

            migrationBuilder.InsertData(
                table: "PaymentOptions",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("8e5264f5-0eea-4fae-9945-80d835583ba1"), "Card" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentOptions");
        }
    }
}
