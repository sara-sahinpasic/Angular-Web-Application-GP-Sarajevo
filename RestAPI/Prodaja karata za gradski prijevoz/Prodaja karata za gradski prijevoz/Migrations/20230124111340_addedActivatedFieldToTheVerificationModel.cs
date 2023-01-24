using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prodaja_karata_za_gradski_prijevoz.Migrations
{
    public partial class addedActivatedFieldToTheVerificationModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Activated",
                table: "VerificationCodes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Activated",
                table: "VerificationCodes");
        }
    }
}
