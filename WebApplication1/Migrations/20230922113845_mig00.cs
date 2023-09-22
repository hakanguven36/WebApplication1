using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApplication1.Migrations
{
    public partial class mig00 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Etiket",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    imageIndex = table.Column<int>(type: "int", nullable: false),
                    choice = table.Column<int>(type: "int", nullable: false),
                    cursorCol = table.Column<int>(type: "int", nullable: false),
                    cursorRow = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Etiket", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Tamamlayan",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    imageIndex = table.Column<int>(type: "int", nullable: false),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tamamlayan", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    username = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    password = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true),
                    hatali = table.Column<int>(type: "int", nullable: false),
                    kilitli = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Etiket");

            migrationBuilder.DropTable(
                name: "Tamamlayan");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
