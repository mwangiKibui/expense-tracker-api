using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTracker.EntityConfig{
    public class TransactionConfig: IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder){
            builder.ToTable("Transactions");

            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.TransactionID).HasDefaultValueSql("gen_random_uuid()");

            builder.HasOne(e => e.User)
            .WithMany(e => e.Transactions)
            .HasForeignKey(e => e.UserId);

            builder.HasOne(e => e.StagedTransaction)
            .WithMany(e => e.Transactions)
            .HasForeignKey(e => e.StagedTransactionId);

            builder.HasOne(e => e.BudgetPlan)
            .WithMany(e => e.Transactions)
            .HasForeignKey(e => e.BudgetPlanId);

            builder.HasOne(e => e.Income)
            .WithMany(e => e.Transactions)
            .HasForeignKey(e => e.IncomeId);

            builder.HasOne(e => e.Expense)
            .WithMany(e => e.Transactions)
            .HasForeignKey(e => e.ExpenseId);

            builder.HasOne(e => e.Account)
            .WithMany(e => e.Transactions)
            .HasForeignKey(e => e.AccountId);
        }
    }
}