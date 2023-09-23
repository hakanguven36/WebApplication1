using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class migInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Proje",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proje", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    username = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    passwordEnc = table.Column<string>(type: "TEXT", nullable: true),
                    hatali = table.Column<int>(type: "INTEGER", nullable: false),
                    kilitli = table.Column<bool>(type: "INTEGER", nullable: false),
                    admin = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "HamResim",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    sysname = table.Column<int>(type: "INTEGER", nullable: false),
                    orjname = table.Column<string>(type: "TEXT", nullable: true),
                    extention = table.Column<string>(type: "TEXT", nullable: true),
                    contentType = table.Column<string>(type: "TEXT", nullable: true),
                    sizekb = table.Column<double>(type: "REAL", nullable: false),
                    imageFormat = table.Column<string>(type: "TEXT", nullable: true),
                    seenOrWhat = table.Column<int>(type: "INTEGER", nullable: false),
                    ProjeID = table.Column<int>(type: "INTEGER", nullable: false),
                    UserID = table.Column<int>(type: "INTEGER", nullable: false),
                    date = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HamResim", x => x.ID);
                    table.ForeignKey(
                        name: "FK_HamResim_Proje_ProjeID",
                        column: x => x.ProjeID,
                        principalTable: "Proje",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HamResim_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSetting",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserID = table.Column<int>(type: "INTEGER", nullable: false),
                    canvasSize = table.Column<int>(type: "INTEGER", nullable: false),
                    ucanCanvasSize = table.Column<int>(type: "INTEGER", nullable: false),
                    seenOrWhat = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSetting", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserSetting_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Etiket",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    HamResimID = table.Column<int>(type: "INTEGER", nullable: false),
                    choice = table.Column<int>(type: "INTEGER", nullable: false),
                    cursorCol = table.Column<int>(type: "INTEGER", nullable: false),
                    cursorRow = table.Column<int>(type: "INTEGER", nullable: false),
                    cursorSize = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Etiket", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Etiket_HamResim_HamResimID",
                        column: x => x.HamResimID,
                        principalTable: "HamResim",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Etiket_HamResimID",
                table: "Etiket",
                column: "HamResimID");

            migrationBuilder.CreateIndex(
                name: "IX_HamResim_ProjeID",
                table: "HamResim",
                column: "ProjeID");

            migrationBuilder.CreateIndex(
                name: "IX_HamResim_UserID",
                table: "HamResim",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_UserSetting_UserID",
                table: "UserSetting",
                column: "UserID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Etiket");

            migrationBuilder.DropTable(
                name: "UserSetting");

            migrationBuilder.DropTable(
                name: "HamResim");

            migrationBuilder.DropTable(
                name: "Proje");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
