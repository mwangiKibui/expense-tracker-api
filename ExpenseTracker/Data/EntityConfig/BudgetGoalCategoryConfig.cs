using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTracker.EntityConfig{
    public class BudgetGoalCategoryConfig: IEntityTypeConfiguration<BudgetGoalCategory>
    {
        public void Configure(EntityTypeBuilder<BudgetGoalCategory> builder){
            builder.ToTable("BudgetGoalCategories");

            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.BudgetGoalCategoryId).HasDefaultValueSql("gen_random_uuid()");
        }
    }
}