using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTracker.EntityConfig{
    public class BudgetPlanConfig: IEntityTypeConfiguration<BudgetPlan>
    {
        public void Configure(EntityTypeBuilder<BudgetPlan> builder){
            builder.ToTable("BudgetPlans");

            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.BudgetPlanID).HasDefaultValueSql("gen_random_uuid()");

            builder.HasOne(e => e.User)
            .WithMany(e => e.BudgetPlans)
            .HasForeignKey(e => e.UserId);

            builder.HasOne(e => e.BudgetGoalCategory)
            .WithMany(e => e.BudgetPlans)
            .HasForeignKey(e => e.BudgetCategoryID);

            builder.HasOne(e => e.Currency)
            .WithMany(e => e.BudgetPlans)
            .HasForeignKey(e => e.CurrencyID);

            builder.HasOne(e => e.Account)
            .WithMany(e => e.BudgetPlans)
            .HasForeignKey(e => e.AccountID);
        }
    }
}