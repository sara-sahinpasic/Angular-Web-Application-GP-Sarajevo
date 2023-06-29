using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prodaja_karata_za_gradski_prijevoz.Migrations
{
    public partial class _114_1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discount = table.Column<double>(type: "float(5)", precision: 5, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Request",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserStatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Approved = table.Column<bool>(type: "bit", nullable: false),
                    DocumentLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Request", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Request_Statuses_UserStatusId",
                        column: x => x.UserStatusId,
                        principalTable: "Statuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "Id", "Discount", "Name" },
                values: new object[,]
                {
                    { new Guid("056b4a11-96b3-413c-a323-0cef9a5680c2"), 0.29999999999999999, "Student" },
                    { new Guid("4c0170aa-cf87-46bd-88a6-bab3687f48b6"), 0.14999999999999999, "Employed" },
                    { new Guid("9647c387-b0fb-4336-9434-079249f37e76"), 0.40000000000000002, "Unemployed" },
                    { new Guid("e6957173-7aa6-4fcb-9dc0-2fc20c20ecae"), 0.5, "Pensioner" },
                    { new Guid("eb11af9e-f0c9-49b5-b3b3-149a9b4c7ebd"), 0.0, "Default" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Request_UserStatusId",
                table: "Request",
                column: "UserStatusId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Request");

            migrationBuilder.DropTable(
                name: "Statuses");
        }
    }
}
