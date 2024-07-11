using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTracker.EntityConfig{
    public class AccountConfig: IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder){
            builder.ToTable("Accounts");

            builder.HasKey(e => e.Id);
            
            builder.Property(e => e.AccountID).HasDefaultValueSql("gen_random_uuid()");
        }
    }
}