using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AdminAssistant.Blog.Data.Migrations
{
    public partial class AddedBrenchmarking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Benchmarking",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UniqueUrl = table.Column<string>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    isActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Benchmarking", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BenchmarkingQuestions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Answer1 = table.Column<string>(nullable: true),
                    Answer2 = table.Column<string>(nullable: true),
                    Answer3 = table.Column<string>(nullable: true),
                    Answer4 = table.Column<string>(nullable: true),
                    Answer5 = table.Column<string>(nullable: true),
                    Answer6 = table.Column<string>(nullable: true),
                    Answer7 = table.Column<string>(nullable: true),
                    BenchmarkingId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BenchmarkingQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BenchmarkingQuestions_Benchmarking_BenchmarkingId",
                        column: x => x.BenchmarkingId,
                        principalTable: "Benchmarking",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BenchmarkingQuestions_BenchmarkingId",
                table: "BenchmarkingQuestions",
                column: "BenchmarkingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BenchmarkingQuestions");

            migrationBuilder.DropTable(
                name: "Benchmarking");
        }
    }
}
