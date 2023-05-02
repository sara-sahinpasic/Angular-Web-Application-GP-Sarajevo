using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prodaja_karata_za_gradski_prijevoz.Migrations
{
    public partial class _35_01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegistrationTokens_Users_KorisnikId",
                table: "RegistrationTokens");

            migrationBuilder.RenameColumn(
                name: "UlogaId",
                table: "Users",
                newName: "RoleId");

            migrationBuilder.RenameColumn(
                name: "Prezime",
                table: "Users",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "Ime",
                table: "Users",
                newName: "FirstName");

            migrationBuilder.RenameColumn(
                name: "DatumRodjenja",
                table: "Users",
                newName: "RegistrationDate");

            migrationBuilder.RenameColumn(
                name: "DatumRegistracije",
                table: "Users",
                newName: "ModifiedDate");

            migrationBuilder.RenameColumn(
                name: "DatumIzmjena",
                table: "Users",
                newName: "DateOfBirth");

            migrationBuilder.RenameColumn(
                name: "BrojTelefona",
                table: "Users",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "Aktivan",
                table: "Users",
                newName: "Active");

            migrationBuilder.RenameColumn(
                name: "Adresa",
                table: "Users",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "Kreiran",
                table: "RegistrationTokens",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "KorisnikId",
                table: "RegistrationTokens",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "Istekao",
                table: "RegistrationTokens",
                newName: "IsExpired");

            migrationBuilder.RenameColumn(
                name: "Aktiviran",
                table: "RegistrationTokens",
                newName: "IsActivated");

            migrationBuilder.RenameIndex(
                name: "IX_RegistrationTokens_KorisnikId",
                table: "RegistrationTokens",
                newName: "IX_RegistrationTokens_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RegistrationTokens_Users_UserId",
                table: "RegistrationTokens",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RegistrationTokens_Users_UserId",
                table: "RegistrationTokens");

            migrationBuilder.RenameColumn(
                name: "RoleId",
                table: "Users",
                newName: "UlogaId");

            migrationBuilder.RenameColumn(
                name: "RegistrationDate",
                table: "Users",
                newName: "DatumRodjenja");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Users",
                newName: "BrojTelefona");

            migrationBuilder.RenameColumn(
                name: "ModifiedDate",
                table: "Users",
                newName: "DatumRegistracije");

            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Users",
                newName: "Prezime");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Users",
                newName: "Ime");

            migrationBuilder.RenameColumn(
                name: "DateOfBirth",
                table: "Users",
                newName: "DatumIzmjena");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Users",
                newName: "Adresa");

            migrationBuilder.RenameColumn(
                name: "Active",
                table: "Users",
                newName: "Aktivan");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "RegistrationTokens",
                newName: "KorisnikId");

            migrationBuilder.RenameColumn(
                name: "IsExpired",
                table: "RegistrationTokens",
                newName: "Istekao");

            migrationBuilder.RenameColumn(
                name: "IsActivated",
                table: "RegistrationTokens",
                newName: "Aktiviran");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "RegistrationTokens",
                newName: "Kreiran");

            migrationBuilder.RenameIndex(
                name: "IX_RegistrationTokens_UserId",
                table: "RegistrationTokens",
                newName: "IX_RegistrationTokens_KorisnikId");

            migrationBuilder.AddForeignKey(
                name: "FK_RegistrationTokens_Users_KorisnikId",
                table: "RegistrationTokens",
                column: "KorisnikId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
