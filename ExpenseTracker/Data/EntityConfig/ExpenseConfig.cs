using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTracker.EntityConfig{
    public class ExpenseConfig: IEntityTypeConfiguration<Expense>
    {
        public void Configure(EntityTypeBuilder<Expense> builder){
            builder.ToTable("Expenses");

            builder.HasKey(e => e.Id);

            builder.Property(t => t.ExpenseID).HasDefaultValueSql("gen_random_uuid()");

            builder.HasOne(t => t.ExpenseType)
            .WithMany(t => t.Expenses)
            .HasForeignKey(t => t.ExpenseTypeId);

            builder.HasOne(t => t.User)
            .WithMany(t => t.Expenses)
            .HasForeignKey(t => t.UserId);
        }
    }
}