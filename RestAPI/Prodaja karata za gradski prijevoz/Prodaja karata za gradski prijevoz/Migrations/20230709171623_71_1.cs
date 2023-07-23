using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prodaja_karata_za_gradski_prijevoz.Migrations
{
    public partial class _71_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "Id", "Active", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("5dd86adc-50be-4db6-98c5-46c4a582b61a"), true, "Povratna", 3.2000000000000002 },
                    { new Guid("929cb30e-ae11-4653-8f20-41c3b39102bd"), true, "Jednosmjerna", 1.8 },
                    { new Guid("b8eec999-55ff-47b5-9ce0-cfedcabadba6"), true, "Dnevna", 7.0999999999999996 },
                    { new Guid("fb272ac2-6c72-40fc-a425-96da10a0077c"), true, "Dječija", 0.59999999999999998 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: new Guid("5dd86adc-50be-4db6-98c5-46c4a582b61a"));

            migrationBuilder.DeleteData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: new Guid("929cb30e-ae11-4653-8f20-41c3b39102bd"));

            migrationBuilder.DeleteData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: new Guid("b8eec999-55ff-47b5-9ce0-cfedcabadba6"));

            migrationBuilder.DeleteData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: new Guid("fb272ac2-6c72-40fc-a425-96da10a0077c"));
        }
    }
}
