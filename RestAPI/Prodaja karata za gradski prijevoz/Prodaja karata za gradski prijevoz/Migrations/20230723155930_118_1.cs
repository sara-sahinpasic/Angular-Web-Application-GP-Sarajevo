using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prodaja_karata_za_gradski_prijevoz.Migrations
{
    public partial class _118_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Request_UserStatusId",
                table: "Request");

            migrationBuilder.CreateIndex(
                name: "IX_Request_UserStatusId",
                table: "Request",
                column: "UserStatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Request_UserStatusId",
                table: "Request");

            migrationBuilder.CreateIndex(
                name: "IX_Request_UserStatusId",
                table: "Request",
                column: "UserStatusId",
                unique: true);
        }
    }
}
