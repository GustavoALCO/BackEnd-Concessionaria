using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Concessionaria.Migrations
{
    /// <inheritdoc />
    public partial class v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Store",
                columns: table => new
                {
                    IdStore = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Adress = table.Column<string>(type: "TEXT", nullable: false),
                    AdressNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    CEP = table.Column<string>(type: "TEXT", nullable: false),
                    PhoneNumbers = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Store", x => x.IdStore);
                });

            migrationBuilder.CreateTable(
                name: "Motos",
                columns: table => new
                {
                    IdMoto = table.Column<Guid>(type: "TEXT", nullable: false),
                    MotoBrand = table.Column<string>(type: "TEXT", nullable: false),
                    Model = table.Column<string>(type: "TEXT", nullable: false),
                    Fuel = table.Column<string>(type: "TEXT", nullable: false),
                    Color = table.Column<string>(type: "TEXT", nullable: false),
                    Plate = table.Column<string>(type: "TEXT", nullable: false),
                    Age = table.Column<int>(type: "INTEGER", nullable: false),
                    Km = table.Column<int>(type: "INTEGER", nullable: false),
                    Price = table.Column<int>(type: "INTEGER", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: false),
                    IdStore = table.Column<int>(type: "INTEGER", nullable: false),
                    Auditable_DateUpload = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Auditable_CreateUserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Auditable_DateUpdate = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    Auditable_AlterationUserId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Motos", x => x.IdMoto);
                    table.ForeignKey(
                        name: "FK_Motos_Store_IdStore",
                        column: x => x.IdStore,
                        principalTable: "Store",
                        principalColumn: "IdStore",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    IdUser = table.Column<Guid>(type: "TEXT", nullable: false),
                    IdStore = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: false),
                    Login = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    Auditable_DateUpload = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Auditable_CreateUserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Auditable_DateUpdate = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    Auditable_AlterationUserId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.IdUser);
                    table.ForeignKey(
                        name: "FK_Users_Store_IdStore",
                        column: x => x.IdStore,
                        principalTable: "Store",
                        principalColumn: "IdStore",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Motos_IdStore",
                table: "Motos",
                column: "IdStore");

            migrationBuilder.CreateIndex(
                name: "IX_Users_IdStore",
                table: "Users",
                column: "IdStore");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Motos");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Store");
        }
    }
}
