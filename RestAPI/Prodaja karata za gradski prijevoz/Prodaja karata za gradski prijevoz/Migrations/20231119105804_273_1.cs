using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prodaja_karata_za_gradski_prijevoz.Migrations
{
    public partial class _273_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InnerMessage",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InnerMessage",
                table: "Logs");
        }
    }
}
