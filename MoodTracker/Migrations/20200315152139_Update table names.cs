using Microsoft.EntityFrameworkCore.Migrations;

namespace MoodTracker.Migrations
{
    public partial class Updatetablenames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyMood_Moods_MoodId",
                table: "DailyMood");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Moods",
                table: "Moods");

            migrationBuilder.RenameTable(
                name: "Moods",
                newName: "Mood");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Mood",
                table: "Mood",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyMood_Mood_MoodId",
                table: "DailyMood",
                column: "MoodId",
                principalTable: "Mood",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyMood_Mood_MoodId",
                table: "DailyMood");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Mood",
                table: "Mood");

            migrationBuilder.RenameTable(
                name: "Mood",
                newName: "Moods");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Moods",
                table: "Moods",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyMood_Moods_MoodId",
                table: "DailyMood",
                column: "MoodId",
                principalTable: "Moods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
