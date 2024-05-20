using System.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Scheduler.Data;
using Scheduler.Services;
using Scheduler.UI;

namespace Scheduler
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            var services = ConfigureServices();
            using (var serviceProvider = services.BuildServiceProvider())
            {
                var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
                var loggerService = serviceProvider.GetRequiredService<LoggerService>();
                var localizationService = serviceProvider.GetRequiredService<LocalizationService>();
                var authenticationService = serviceProvider.GetRequiredService<AuthenticationService>();

                // Ensure the database is created
                dbContext.Database.EnsureCreated();

                var backupService = new DatabaseBackupService(dbContext);

                // Backup the database
                // backupService.BackupDatabase("backup.json");


                // Truncate all tables before seeding
                dbContext.TruncateAllTables(); // Comment out this line if you want to keep data between runs

                backupService.RestoreDatabase("backup.json");

                // Seed the database if the database is empty
                // if (!dbContext.Users.Any())
                // {
                //     var seeder = new DataSeeder(dbContext);
                //     seeder.Seed();
                // }

                Application.SetHighDpiMode(HighDpiMode.SystemAware);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                var mainForm = serviceProvider.GetRequiredService<MainForm>();
                var loginForm = new LogInForm(authenticationService, mainForm, dbContext);
                Application.Run(loginForm);
            }
        }

        private static IServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();
            // Add DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySQL(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString));
            services.AddTransient<LoggerService>();
            services.AddTransient<LocalizationService>();
            services.AddTransient<AuthenticationService>();
            services.AddTransient<MainForm>();

            return services;
        }
    }
}