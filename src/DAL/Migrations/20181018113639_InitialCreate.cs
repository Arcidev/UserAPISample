using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Email = table.Column<string>(maxLength: 100, nullable: false),
                    Name = table.Column<string>(maxLength: 255, nullable: false),
                    PasswordSalt = table.Column<string>(maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(maxLength: 100, nullable: false),
                    CreatedOn = table.Column<DateTime>(nullable: false),
                    LastUpdatedOn = table.Column<DateTime>(nullable: true),
                    LastLoginOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Phones",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    Number = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phones", x => new { x.UserId, x.Number });
                    table.ForeignKey(
                        name: "FK_Phones_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Phones");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
