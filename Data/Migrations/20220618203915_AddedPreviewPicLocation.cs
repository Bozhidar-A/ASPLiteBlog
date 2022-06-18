using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPLiteBlog.Data.Migrations
{
    public partial class AddedPreviewPicLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "previewPicLocation",
                table: "BlogPost",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "previewPicLocation",
                table: "BlogPost");
        }
    }
}
