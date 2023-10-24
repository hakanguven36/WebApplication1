using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class migBir : Migration
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

            migrationBuilder.CreateTable(
                name: "Label",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    photoID = table.Column<int>(type: "INTEGER", nullable: false),
                    sizeFactor = table.Column<float>(type: "REAL", nullable: false),
                    shape = table.Column<int>(type: "INTEGER", nullable: false),
                    annoID = table.Column<int>(type: "INTEGER", nullable: false),
                    userID = table.Column<int>(type: "INTEGER", nullable: false),
                    projectID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Label", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Label_Photo_photoID",
                        column: x => x.photoID,
                        principalTable: "Photo",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Coordinate",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    X = table.Column<int>(type: "INTEGER", nullable: false),
                    Y = table.Column<int>(type: "INTEGER", nullable: false),
                    LabelID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coordinate", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Coordinate_Label_LabelID",
                        column: x => x.LabelID,
                        principalTable: "Label",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Annotation_ProjectID",
                table: "Annotation",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_Coordinate_LabelID",
                table: "Coordinate",
                column: "LabelID");

            migrationBuilder.CreateIndex(
                name: "IX_Label_photoID",
                table: "Label",
                column: "photoID");

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
                name: "Coordinate");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Label");

            migrationBuilder.DropTable(
                name: "Photo");

            migrationBuilder.DropTable(
                name: "Project");
        }
    }
}
