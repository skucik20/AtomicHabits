using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WpfApp.Core.Migrations
{
    /// <inheritdoc />
    public partial class checkadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsHabitDone",
                table: "AtomicHabits",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Streak",
                table: "AtomicHabits",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHabitDone",
                table: "AtomicHabits");

            migrationBuilder.DropColumn(
                name: "Streak",
                table: "AtomicHabits");
        }
    }
}
