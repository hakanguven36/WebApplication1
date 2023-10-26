using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class miginit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.ID);
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
                name: "Annotation",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", nullable: true),
                    color = table.Column<string>(type: "TEXT", nullable: true),
                    textColor = table.Column<string>(type: "TEXT", nullable: true),
                    ProjectID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Annotation", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Annotation_Project_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "Project",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Photo",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    sysname = table.Column<string>(type: "TEXT", nullable: true),
                    orjname = table.Column<string>(type: "TEXT", nullable: true),
                    extention = table.Column<string>(type: "TEXT", nullable: true),
                    contentType = table.Column<string>(type: "TEXT", nullable: true),
                    sizeMB = table.Column<double>(type: "REAL", nullable: false),
                    imageFormat = table.Column<string>(type: "TEXT", nullable: true),
                    completed = table.Column<bool>(type: "INTEGER", nullable: false),
                    labels = table.Column<string>(type: "TEXT", nullable: true),
                    ProjectID = table.Column<int>(type: "INTEGER", nullable: false),
                    date = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Photo", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Photo_Project_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "Project",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Annotation_ProjectID",
                table: "Annotation",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_Photo_ProjectID",
                table: "Photo",
                column: "ProjectID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Annotation");

            migrationBuilder.DropTable(
                name: "Photo");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Project");
        }
    }
}
