using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prodaja_karata_za_gradski_prijevoz.Migrations
{
    public partial class _110_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Discount",
                table: "Statuses",
                type: "float(5)",
                precision: 5,
                scale: 2,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: new Guid("056b4a11-96b3-413c-a323-0cef9a5680c2"),
                column: "Discount",
                value: 0.29999999999999999);

            migrationBuilder.UpdateData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: new Guid("4c0170aa-cf87-46bd-88a6-bab3687f48b6"),
                column: "Discount",
                value: 0.14999999999999999);

            migrationBuilder.UpdateData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: new Guid("9647c387-b0fb-4336-9434-079249f37e76"),
                column: "Discount",
                value: 0.40000000000000002);

            migrationBuilder.UpdateData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: new Guid("e6957173-7aa6-4fcb-9dc0-2fc20c20ecae"),
                column: "Discount",
                value: 0.5);

            migrationBuilder.UpdateData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: new Guid("eb11af9e-f0c9-49b5-b3b3-149a9b4c7ebd"),
                column: "Discount",
                value: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Discount",
                table: "Statuses",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float(5)",
                oldPrecision: 5,
                oldScale: 2);

            migrationBuilder.UpdateData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: new Guid("056b4a11-96b3-413c-a323-0cef9a5680c2"),
                column: "Discount",
                value: 30);

            migrationBuilder.UpdateData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: new Guid("4c0170aa-cf87-46bd-88a6-bab3687f48b6"),
                column: "Discount",
                value: 15);

            migrationBuilder.UpdateData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: new Guid("9647c387-b0fb-4336-9434-079249f37e76"),
                column: "Discount",
                value: 40);

            migrationBuilder.UpdateData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: new Guid("e6957173-7aa6-4fcb-9dc0-2fc20c20ecae"),
                column: "Discount",
                value: 50);

            migrationBuilder.UpdateData(
                table: "Statuses",
                keyColumn: "Id",
                keyValue: new Guid("eb11af9e-f0c9-49b5-b3b3-149a9b4c7ebd"),
                column: "Discount",
                value: 0);
        }
    }
}
