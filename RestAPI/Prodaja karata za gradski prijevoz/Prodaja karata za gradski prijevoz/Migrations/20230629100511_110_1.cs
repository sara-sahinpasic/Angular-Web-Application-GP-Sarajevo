using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prodaja_karata_za_gradski_prijevoz.Migrations
{
    public partial class _110_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "Id", "Discount", "Name" },
                values: new object[,]
                {
                    { new Guid("056b4a11-96b3-413c-a323-0cef9a5680c2"), 30, "Student" },
                    { new Guid("4c0170aa-cf87-46bd-88a6-bab3687f48b6"), 15, "Employed" },
                    { new Guid("9647c387-b0fb-4336-9434-079249f37e76"), 40, "Unemployed" },
                    { new Guid("e6957173-7aa6-4fcb-9dc0-2fc20c20ecae"), 50, "Pensioner" },
                    { new Guid("eb11af9e-f0c9-49b5-b3b3-149a9b4c7ebd"), 0, "Default" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Statuses");
        }
    }
}
