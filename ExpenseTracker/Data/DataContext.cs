using ExpenseTracker.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options): base(options){

        }

        // define the tables here.
        public DbSet<User> Users { set;get;}
        public DbSet<ForgotPasswordRequest> ForgotPasswordRequests{ set;get;}
        public DbSet<ExpenseType> ExpenseTypes { set;get;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(e => e.UserID)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<ForgotPasswordRequest>()
                .Property(e => e.ForgotPasswordId)
                .HasDefaultValueSql("gen_random_uuid()");

            modelBuilder.Entity<ExpenseType>()
                .Property(e => e.ExpenseTypeId)
                .HasDefaultValueSql("gen_random_uuid()");
        }
    }
}