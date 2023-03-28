using GBM.User;
using Microsoft.EntityFrameworkCore;

namespace GBM.psqlDB
{
    internal static class ModelBuilders
    {
        internal static void ConfigureUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GBM.User.User>()
                .Property(u => u.Id)
                .HasDefaultValueSql("uuid_generate_v4()");

            modelBuilder.Entity<GBM.User.User>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<GBM.User.User>()
                .HasIndex(u => u.Name)
                .IsUnique();

            modelBuilder.Entity<GBM.User.User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<GBM.User.User>()
                .Property(u => u.Email)
                .IsRequired()
                .IsUnicode(false)
                .HasMaxLength(256)
                .HasAnnotation("DataType", "nvarchar(256)")
                .HasConversion(
                    v => v.ToString(),
                    v => new System.Net.Mail.MailAddress(v).Address
                );

            modelBuilder.Entity<GBM.User.User>()
                .Property(u => u.Phone)
                .HasMaxLength(20);

            modelBuilder.Entity<GBM.User.User>()
                .Property(u => u.UserType)
                .IsRequired()
                .HasMaxLength(50);

            modelBuilder.Entity<GBM.User.User>()
                .Property(u => u.SaltAndHashString)
                .IsUnicode(false);
        }

        internal static void ConfigureCustomer(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .HasBaseType<GBM.User.User>();
        }

        internal static void ConfigureStaff(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Staff>()
                .HasBaseType<GBM.User.User>();
        }

        internal static void ConfigureAdmin(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Admin>()
                .HasBaseType<GBM.User.User>();
        }

        internal static void ConfigureService(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Service>()
                .HasBaseType<GBM.User.User>();
        }
    }
}
