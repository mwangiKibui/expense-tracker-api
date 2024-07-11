using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTracker.EntityConfig{
    public class NotificationConfig: IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder){
            builder.ToTable("Notifications");
            builder.HasKey(e => e.Id);
            builder.Property(t => t.NotificationID).HasDefaultValueSql("gen_random_uuid()");

            builder.HasOne(e => e.User)
            .WithMany(e => e.Notifications)
            .HasForeignKey(e => e.UserId);
        }
    }
}