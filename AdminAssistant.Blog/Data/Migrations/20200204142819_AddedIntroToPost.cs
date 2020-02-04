using Microsoft.EntityFrameworkCore.Migrations;

namespace AdminAssistant.Blog.Data.Migrations
{
    public partial class AddedIntroToPost : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Intro",
                table: "Post",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Intro",
                table: "Post");
        }
    }
}
