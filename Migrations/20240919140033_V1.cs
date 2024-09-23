using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Concessionaria.Migrations
{
    /// <inheritdoc />
    public partial class V1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    IdStore = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Adress = table.Column<string>(type: "TEXT", nullable: false),
                    CEP = table.Column<string>(type: "TEXT", nullable: false),
                    IsFull = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.IdStore);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    IdUser = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: false),
                    Login = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    Auditable_DateUpload = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Auditable_CreateUserId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Auditable_DateUpdate = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    Auditable_AlterationUserId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.IdUser);
                });

            migrationBuilder.CreateTable(
                name: "Motos",
                columns: table => new
                {
                    IdMoto = table.Column<Guid>(type: "TEXT", nullable: false),
                    MotoBrand = table.Column<string>(type: "TEXT", nullable: false),
                    Model = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Fuel = table.Column<string>(type: "TEXT", nullable: false),
                    Color = table.Column<string>(type: "TEXT", nullable: false),
                    Age = table.Column<int>(type: "INTEGER", nullable: false),
                    Url = table.Column<string>(type: "TEXT", nullable: false),
                    Auditable_DateUpload = table.Column<DateTimeOffset>(type: "TEXT", nullable: false),
                    Auditable_CreateUserId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Auditable_DateUpdate = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    Auditable_AlterationUserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    StoreIdStore = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Motos", x => x.IdMoto);
                    table.ForeignKey(
                        name: "FK_Motos_Stores_StoreIdStore",
                        column: x => x.StoreIdStore,
                        principalTable: "Stores",
                        principalColumn: "IdStore",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Motos_StoreIdStore",
                table: "Motos",
                column: "StoreIdStore");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Motos");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Stores");
        }
    }
}
