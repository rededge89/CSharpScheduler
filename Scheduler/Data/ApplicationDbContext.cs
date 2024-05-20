using Microsoft.EntityFrameworkCore;
using Scheduler.Managers;

namespace Scheduler.Data
{
    public class ApplicationDbContext : DbContext
    {
        private SessionManager _sessionManager;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Models.UserModel> Users { get; set; }
        public DbSet<Models.CustomerModel> Customers { get; set; }
        public DbSet<Models.AppointmentModel> Appointments { get; set; }
        public DbSet<Models.AddressModel> Addresses { get; set; }
        public DbSet<Models.CityModel> Cities { get; set; }
        public DbSet<Models.CountryModel> Countries { get; set; }

        public void SetSession(SessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries()
                .Where(e => e.Entity is TimeStampedEntity &&
                            (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entry in entries)
            {
                var entity = (TimeStampedEntity)entry.Entity;
                var now = DateTime.UtcNow;
                var userName =
                    _sessionManager?.userName ??
                    "System"; // Fallback to "System" if sessionManager or CurrentUser is null

                if (entry.State == EntityState.Added)
                {
                    entity.CreateDate = now;
                    entity.CreatedBy = userName;
                }

                entity.LastUpdate = now;
                entity.LastUpdateBy = userName;
            }

            return base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Models.UserModel>().ToTable("user");
            modelBuilder.Entity<Models.CustomerModel>().ToTable("customer");
            modelBuilder.Entity<Models.AppointmentModel>().ToTable("appointment");
            modelBuilder.Entity<Models.AddressModel>().ToTable("address");
            modelBuilder.Entity<Models.CityModel>().ToTable("city");
            modelBuilder.Entity<Models.CountryModel>().ToTable("country");

            // Configure keys and relationships
            modelBuilder.Entity<Models.AddressModel>()
                .HasOne(a => a.City)
                .WithMany()
                .HasForeignKey(a => a.CityId)
                .OnDelete(DeleteBehavior.Cascade); // Configure cascading delete

            modelBuilder.Entity<Models.CityModel>()
                .HasOne(c => c.Country)
                .WithMany()
                .HasForeignKey(c => c.CountryId)
                .OnDelete(DeleteBehavior.Cascade); // Configure cascading delete

            modelBuilder.Entity<Models.CustomerModel>()
                .HasOne(c => c.Address)
                .WithMany()
                .HasForeignKey(c => c.AddressId)
                .OnDelete(DeleteBehavior.Cascade); // Configure cascading delete

            modelBuilder.Entity<Models.AppointmentModel>()
                .HasOne(a => a.Customer)
                .WithMany()
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(DeleteBehavior.Cascade); // Configure cascading delete

            modelBuilder.Entity<Models.AppointmentModel>()
                .HasOne(a => a.User)
                .WithMany(u => u.Appointments)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Configure cascading delete
        }

        public void TruncateAllTables()
        {
            Console.WriteLine("Truncating all tables...");
            Database.ExecuteSqlRaw("SET FOREIGN_KEY_CHECKS = 0;");
            var tableNames = new[] { "appointment", "customer", "address", "city", "country", "user" };
            foreach (var tableName in tableNames)
            {
                Database.ExecuteSqlRaw($"TRUNCATE TABLE `{tableName}`");
            }

            Database.ExecuteSqlRaw("SET FOREIGN_KEY_CHECKS = 1;");
        }
    }
}