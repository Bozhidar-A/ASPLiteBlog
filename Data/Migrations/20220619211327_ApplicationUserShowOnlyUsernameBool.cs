using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPLiteBlog.Data.Migrations
{
    public partial class ApplicationUserShowOnlyUsernameBool : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "displayUsername",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "displayUsername",
                table: "AspNetUsers");
        }
    }
}
