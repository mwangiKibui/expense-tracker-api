using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Migrations
{
    /// <inheritdoc />
    public partial class associatedbudgetplantoaccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountID",
                table: "BudgetPlans",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BudgetPlans_AccountID",
                table: "BudgetPlans",
                column: "AccountID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetPlans_Accounts_AccountID",
                table: "BudgetPlans",
                column: "AccountID",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetPlans_Accounts_AccountID",
                table: "BudgetPlans");

            migrationBuilder.DropIndex(
                name: "IX_BudgetPlans_AccountID",
                table: "BudgetPlans");

            migrationBuilder.DropColumn(
                name: "AccountID",
                table: "BudgetPlans");
        }
    }
}
