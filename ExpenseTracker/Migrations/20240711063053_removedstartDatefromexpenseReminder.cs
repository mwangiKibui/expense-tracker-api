using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Migrations
{
    /// <inheritdoc />
    public partial class removedstartDatefromexpenseReminder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NatureOfRecurrence",
                table: "ExpenseReminders");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "ExpenseReminders");

            migrationBuilder.AddColumn<int>(
                name: "NatureOfRecurrence",
                table: "Expenses",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateOnly>(
                name: "ReminderStartDate",
                table: "Expenses",
                type: "date",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NatureOfRecurrence",
                table: "Expenses");

            migrationBuilder.DropColumn(
                name: "ReminderStartDate",
                table: "Expenses");

            migrationBuilder.AddColumn<int>(
                name: "NatureOfRecurrence",
                table: "ExpenseReminders",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateOnly>(
                name: "StartDate",
                table: "ExpenseReminders",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }
    }
}
