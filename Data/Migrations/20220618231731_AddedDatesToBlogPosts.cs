using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASPLiteBlog.Data.Migrations
{
    public partial class AddedDatesToBlogPosts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "draft",
                table: "BlogPost",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "timeCreated",
                table: "BlogPost",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "timeLastEdited",
                table: "BlogPost",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "timeLastEditedPublished",
                table: "BlogPost",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "timePublished",
                table: "BlogPost",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "draft",
                table: "BlogPost");

            migrationBuilder.DropColumn(
                name: "timeCreated",
                table: "BlogPost");

            migrationBuilder.DropColumn(
                name: "timeLastEdited",
                table: "BlogPost");

            migrationBuilder.DropColumn(
                name: "timeLastEditedPublished",
                table: "BlogPost");

            migrationBuilder.DropColumn(
                name: "timePublished",
                table: "BlogPost");
        }
    }
}
