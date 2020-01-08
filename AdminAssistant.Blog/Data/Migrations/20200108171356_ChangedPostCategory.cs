using Microsoft.EntityFrameworkCore.Migrations;

namespace AdminAssistant.Blog.Data.Migrations
{
    public partial class ChangedPostCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostCategory_Category_TagId",
                table: "PostCategory");

            migrationBuilder.DropIndex(
                name: "IX_PostCategory_TagId",
                table: "PostCategory");

            migrationBuilder.DropColumn(
                name: "TagId",
                table: "PostCategory");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "PostCategory",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostCategory_CategoryId",
                table: "PostCategory",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostCategory_Category_CategoryId",
                table: "PostCategory",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostCategory_Category_CategoryId",
                table: "PostCategory");

            migrationBuilder.DropIndex(
                name: "IX_PostCategory_CategoryId",
                table: "PostCategory");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "PostCategory");

            migrationBuilder.AddColumn<int>(
                name: "TagId",
                table: "PostCategory",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostCategory_TagId",
                table: "PostCategory",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostCategory_Category_TagId",
                table: "PostCategory",
                column: "TagId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
