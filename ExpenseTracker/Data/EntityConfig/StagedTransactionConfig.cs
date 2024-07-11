using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTracker.EntityConfig{
    public class StagedTransactionConfig: IEntityTypeConfiguration<StagedTransaction>
    {
        public void Configure(EntityTypeBuilder<StagedTransaction> builder){
            builder.ToTable("StagedTransactions");

            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.StagedTransactionID).HasDefaultValueSql("gen_random_uuid()");
        }
    }
}