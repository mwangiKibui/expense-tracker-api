using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTracker.EntityConfig{
    public class ForgotPasswordRequestConfig: IEntityTypeConfiguration<ForgotPasswordRequest>
    {
        public void Configure(EntityTypeBuilder<ForgotPasswordRequest> builder){
            builder.ToTable("ForgotPasswordRequests");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.ForgotPasswordId).HasDefaultValueSql("gen_random_uuid()");
            builder.HasOne(t => t.User)
            .WithMany(t => t.ForgotPasswordRequests)
            .HasForeignKey(t => t.UserId);
        }
    }
}