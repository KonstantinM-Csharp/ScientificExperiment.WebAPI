using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ScientificExperiment.WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FileName = table.Column<string>(type: "TEXT", nullable: false),
                    Author = table.Column<string>(type: "TEXT", nullable: true),
                    CreationDateTime = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Results",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    LastTime = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    MaxTimeWork = table.Column<int>(type: "INTEGER", nullable: false),
                    MinTimeWork = table.Column<int>(type: "INTEGER", nullable: false),
                    AverageTimeWork = table.Column<double>(type: "REAL", nullable: false),
                    AverageIndicator = table.Column<double>(type: "REAL", nullable: false),
                    MedianByIndicator = table.Column<double>(type: "REAL", nullable: false),
                    MaxIndicator = table.Column<double>(type: "REAL", nullable: false),
                    MinIndicator = table.Column<double>(type: "REAL", nullable: false),
                    CountOfExperiments = table.Column<int>(type: "INTEGER", nullable: false),
                    FileId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Results", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Results_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Values",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    StartDateTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    WorkTime = table.Column<int>(type: "INTEGER", nullable: false),
                    Indicator = table.Column<double>(type: "REAL", nullable: false),
                    FileId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Values", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Values_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Results_FileId",
                table: "Results",
                column: "FileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Values_FileId",
                table: "Values",
                column: "FileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Results");

            migrationBuilder.DropTable(
                name: "Values");

            migrationBuilder.DropTable(
                name: "Files");
        }
    }
}
