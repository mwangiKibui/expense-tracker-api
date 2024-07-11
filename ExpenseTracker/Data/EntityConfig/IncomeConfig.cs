using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTracker.EntityConfig{
    public class IncomeConfig: IEntityTypeConfiguration<Income>
    {
        public void Configure(EntityTypeBuilder<Income> builder){
            builder.ToTable("Incomes");
            builder.HasKey(e => e.Id);
            builder.Property(t => t.IncomeID).HasDefaultValueSql("gen_random_uuid()");

            builder.HasOne(e => e.IncomeType)
            .WithMany(e => e.Incomes)
            .HasForeignKey(e => e.IncomeTypeId);

            builder.HasOne(e => e.User)
            .WithMany(e => e.Incomes)
            .HasForeignKey(e => e.UserId);
        }
    }
}