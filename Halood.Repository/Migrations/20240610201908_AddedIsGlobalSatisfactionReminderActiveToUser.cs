using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Halood.Repository.Migrations
{
    public partial class AddedIsGlobalSatisfactionReminderActiveToUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsGlobalSatisfactionReminderActive",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsGlobalSatisfactionReminderActive",
                table: "Users");
        }
    }
}
