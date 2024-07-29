using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Halood.Repository.Migrations
{
    public partial class AddedUserEmotionReminder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserEmotionReminders",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Hour = table.Column<int>(type: "int", nullable: false),
                    ValidTo = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "rowversion", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEmotionReminders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserEmotionReminders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserEmotionReminders_UserId",
                table: "UserEmotionReminders",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserEmotionReminders");
        }
    }
}
