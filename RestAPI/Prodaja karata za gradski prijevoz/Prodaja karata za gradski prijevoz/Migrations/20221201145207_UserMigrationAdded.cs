using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prodajakaratazagradskiprijevoz.Migrations
{
    /// <inheritdoc />
    public partial class UserMigrationAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    UlogaId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Ime = table.Column<string>(type: "TEXT", nullable: true),
                    Prezime = table.Column<string>(type: "TEXT", nullable: true),
                    Email = table.Column<string>(type: "TEXT", nullable: true),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    PasswordSalt = table.Column<byte[]>(type: "BLOB", nullable: true),
                    DatumRodjenja = table.Column<DateTime>(type: "TEXT", nullable: false),
                    BrojTelefona = table.Column<string>(type: "TEXT", nullable: true),
                    Adresa = table.Column<string>(type: "TEXT", nullable: true),
                    DatumRegistracije = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DatumIzmjena = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Aktivan = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegistrationTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    KorisnikId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Token = table.Column<string>(type: "TEXT", nullable: true),
                    Istekao = table.Column<bool>(type: "INTEGER", nullable: false),
                    Aktiviran = table.Column<bool>(type: "INTEGER", nullable: false),
                    Kreiran = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrationTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrationTokens_Users_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegistrationTokens_KorisnikId",
                table: "RegistrationTokens",
                column: "KorisnikId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegistrationTokens");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
