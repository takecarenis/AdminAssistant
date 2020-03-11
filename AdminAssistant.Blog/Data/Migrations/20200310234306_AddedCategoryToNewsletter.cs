using Microsoft.EntityFrameworkCore.Migrations;

namespace AdminAssistant.Blog.Data.Migrations
{
    public partial class AddedCategoryToNewsletter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "NewsletterHistory",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Newsletter",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "NewsletterHistory");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Newsletter");
        }
    }
}
