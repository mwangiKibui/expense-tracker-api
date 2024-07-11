using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTracker.EntityConfig{
    public class ExpenseReminderConfig: IEntityTypeConfiguration<ExpenseReminder>
    {
        public void Configure(EntityTypeBuilder<ExpenseReminder> builder){
            builder.ToTable("ExpenseReminders");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.ExpenseReminderID).HasDefaultValueSql("gen_random_uuid()");
            builder.HasOne(t => t.Expense)
            .WithMany(t => t.ExpenseReminders)
            .HasForeignKey(t => t.ExpenseId);
            builder.HasOne(t => t.User)
            .WithMany(t => t.ExpenseReminders)
            .HasForeignKey(t => t.UserId);
        }
    }
}