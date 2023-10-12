using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class mig_init : Migration
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
                    sizekb = table.Column<double>(type: "REAL", nullable: false),
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
                    beginX = table.Column<int>(type: "INTEGER", nullable: false),
                    beginY = table.Column<int>(type: "INTEGER", nullable: false),
                    endX = table.Column<int>(type: "INTEGER", nullable: false),
                    endY = table.Column<int>(type: "INTEGER", nullable: false),
                    AnnotationID = table.Column<int>(type: "INTEGER", nullable: false),
                    PhotoID = table.Column<int>(type: "INTEGER", nullable: false),
                    UserID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Label", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Label_Annotation_AnnotationID",
                        column: x => x.AnnotationID,
                        principalTable: "Annotation",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Label_Photo_PhotoID",
                        column: x => x.PhotoID,
                        principalTable: "Photo",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Label_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Annotation_ProjectID",
                table: "Annotation",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_Label_AnnotationID",
                table: "Label",
                column: "AnnotationID");

            migrationBuilder.CreateIndex(
                name: "IX_Label_PhotoID",
                table: "Label",
                column: "PhotoID");

            migrationBuilder.CreateIndex(
                name: "IX_Label_UserID",
                table: "Label",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Photo_ProjectID",
                table: "Photo",
                column: "ProjectID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Label");

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
