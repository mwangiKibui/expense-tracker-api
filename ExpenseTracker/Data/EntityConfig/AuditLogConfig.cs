using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTracker.EntityConfig{
    public class AuditLogConfig: IEntityTypeConfiguration<AuditLog>
    {
        public void Configure(EntityTypeBuilder<AuditLog> builder){
            builder.ToTable("AuditLogs");
            builder.HasKey(e => e.Id);
            builder.Property(t => t.AuditLogID).HasDefaultValueSql("gen_random_uuid()");

            builder.HasOne(e => e.User)
            .WithMany(e => e.AuditLogs)
            .HasForeignKey(e => e.UserId);
        }
    }
}