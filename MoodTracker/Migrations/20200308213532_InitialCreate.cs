using Microsoft.EntityFrameworkCore.Migrations;

namespace MoodTracker.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyMood_Mood_MoodId",
                table: "DailyMood");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Mood",
                table: "Mood");

            migrationBuilder.DropColumn(
                name: "PercentIntensity",
                table: "Mood");

            migrationBuilder.RenameTable(
                name: "Mood",
                newName: "Moods");

            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Moods",
                nullable: true);

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
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyMood_Moods_MoodId",
                table: "DailyMood");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Moods",
                table: "Moods");

            migrationBuilder.DropColumn(
                name: "Color",
                table: "Moods");

            migrationBuilder.RenameTable(
                name: "Moods",
                newName: "Mood");

            migrationBuilder.AddColumn<int>(
                name: "PercentIntensity",
                table: "Mood",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
                onDelete: ReferentialAction.Restrict);
        }
    }
}
