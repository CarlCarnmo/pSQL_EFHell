using GBM.User;
using Microsoft.EntityFrameworkCore;



namespace GBM.psqlDB
{
    internal class psqlContext : DbContext
    {
        public psqlContext() { }
        public psqlContext(DbContextOptions<psqlContext> options) : base(options) { }

        public DbSet<User.User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Service> Services { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "Server=localhost;Database=postgres;User Id=postgres;Password=123;";
            optionsBuilder.UseNpgsql(connectionString);
        }
        /* THIS CODE IS FOR 'App.config'. But it refused to correctly use the connectionstring that way.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .AddXmlFile("App.config", optional: false, reloadOnChange: true)
                .Build();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseNpgsql(connectionString);
        }*/

        public Repository<T> GetRepository<T>() where T : class
        {
            return new Repository<T>(this);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("gbm");
            modelBuilder.Entity<User.User>().ToTable("Users", "gbm"); // This might be overkill but I really don't want to find out.
            ModelBuilders.ConfigureUser(modelBuilder);
            ModelBuilders.ConfigureCustomer(modelBuilder);
            ModelBuilders.ConfigureStaff(modelBuilder);
            ModelBuilders.ConfigureAdmin(modelBuilder);
            ModelBuilders.ConfigureService(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }
    }
}
