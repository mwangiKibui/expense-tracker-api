using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Migrations
{
    /// <inheritdoc />
    public partial class systemdefaultonincomeandexpensetypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_StagedTransactions_StagedTransactionId",
                table: "Transactions");

            migrationBuilder.AlterColumn<int>(
                name: "StagedTransactionId",
                table: "Transactions",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<bool>(
                name: "SystemDefault",
                table: "IncomeTypes",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SystemDefault",
                table: "ExpenseTypes",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_StagedTransactions_StagedTransactionId",
                table: "Transactions",
                column: "StagedTransactionId",
                principalTable: "StagedTransactions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_StagedTransactions_StagedTransactionId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "SystemDefault",
                table: "IncomeTypes");

            migrationBuilder.DropColumn(
                name: "SystemDefault",
                table: "ExpenseTypes");

            migrationBuilder.AlterColumn<int>(
                name: "StagedTransactionId",
                table: "Transactions",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_StagedTransactions_StagedTransactionId",
                table: "Transactions",
                column: "StagedTransactionId",
                principalTable: "StagedTransactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
