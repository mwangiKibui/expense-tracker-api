using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTracker.EntityConfig{
    public class CurrencyConfig: IEntityTypeConfiguration<Currency>
    {
        public void Configure(EntityTypeBuilder<Currency> builder){
            builder.ToTable("Currencies");
            builder.HasKey(e => e.Id);
            builder.Property(t => t.CurrencyID).HasDefaultValueSql("gen_random_uuid()");
        }
    }
}