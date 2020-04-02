using Microsoft.EntityFrameworkCore.Migrations;

namespace MoodTracker.Migrations
{
    public partial class MoodIntensityIsDouble : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "MoodIntensity",
                table: "DailyMood",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MoodIntensity",
                table: "DailyMood",
                type: "int",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
