using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Halood.Repository.Migrations
{
    public partial class AddedHasBlockedBotColumnToUserTbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasBlockedBot",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.Sql(@"UPDATE dbo.Users SET HasBlockedBot = 0");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasBlockedBot",
                table: "Users");
        }
    }
}
