using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ExpenseTracker.EntityConfig{
    public class UserConfig: IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder){
            builder.ToTable("Users");
            builder.HasKey(t => t.Id);
            builder.Property(t => t.UserID).HasDefaultValueSql("gen_random_uuid()");
        }
    }
}