using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Migrations
{
    /// <inheritdoc />
    public partial class multiplebudgetplansonanaccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BudgetPlans_AccountID",
                table: "BudgetPlans");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetPlans_AccountID",
                table: "BudgetPlans",
                column: "AccountID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BudgetPlans_AccountID",
                table: "BudgetPlans");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetPlans_AccountID",
                table: "BudgetPlans",
                column: "AccountID",
                unique: true);
        }
    }
}
