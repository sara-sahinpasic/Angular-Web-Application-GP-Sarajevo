using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prodaja_karata_za_gradski_prijevoz.Migrations
{
    public partial class _241_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: new Guid("eb11af9e-f0c9-49b5-b3b3-149a9b4c7ebd"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "Id", "Discount", "Name" },
                values: new object[] { new Guid("eb11af9e-f0c9-49b5-b3b3-149a9b4c7ebd"), 0.0, "Default" });
        }
    }
}
