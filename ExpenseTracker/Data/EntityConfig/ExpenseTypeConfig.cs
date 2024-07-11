using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTracker.EntityConfig{
    public class ExpenseTypeConfig: IEntityTypeConfiguration<ExpenseType>
    {
        public void Configure(EntityTypeBuilder<ExpenseType> builder){
            builder.ToTable("ExpenseTypes");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.ExpenseTypeId).HasDefaultValueSql("gen_random_uuid()");
        }
    }
}