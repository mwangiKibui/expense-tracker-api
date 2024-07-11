using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTracker.EntityConfig{
    public class IncomeTypeConfig: IEntityTypeConfiguration<IncomeType>
    {
        public void Configure(EntityTypeBuilder<IncomeType> builder){
            builder.ToTable("IncomeTypes");
            builder.HasKey(e => e.Id);
            builder.Property(t => t.IncomeTypeID).HasDefaultValueSql("gen_random_uuid()");
        }
    }
}