using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTracker.EntityConfig{
    public class IncomeReminderConfig: IEntityTypeConfiguration<IncomeReminder>
    {
        public void Configure(EntityTypeBuilder<IncomeReminder> builder){
            builder.ToTable("IncomeReminders");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.IncomeReminderID).HasDefaultValueSql("gen_random_uuid()");
            
            builder.HasOne(t => t.Income)
            .WithMany(t => t.IncomeReminders)
            .HasForeignKey(t => t.IncomeId);

            builder.HasOne(t => t.User)
            .WithMany(t => t.IncomeReminders)
            .HasForeignKey(t => t.UserId);
        }
    }
}