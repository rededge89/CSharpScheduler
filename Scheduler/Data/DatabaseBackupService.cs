using System.Text.Json;
using System.Text.Json.Serialization;

namespace Scheduler.Data
{
    public class DatabaseBackupService
    {
        private readonly ApplicationDbContext _dbContext;

        public DatabaseBackupService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void BackupDatabase(string filePath)
        {
            var backupData = new BackupData
            {
                Users = _dbContext.Users.ToList(),
                Customers = _dbContext.Customers.ToList(),
                Appointments = _dbContext.Appointments.ToList(),
                Addresses = _dbContext.Addresses.ToList(),
                Cities = _dbContext.Cities.ToList(),
                Countries = _dbContext.Countries.ToList()
            };

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                ReferenceHandler = ReferenceHandler.Preserve
            };

            var json = JsonSerializer.Serialize(backupData, options);

            File.WriteAllText(filePath, json);
        }

        public void RestoreDatabase(string filePath)
        {
            var json = File.ReadAllText(filePath);

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            };

            var backupData = JsonSerializer.Deserialize<BackupData>(json, options);

            if (backupData == null)
            {
                throw new InvalidOperationException("Failed to deserialize backup data.");
            }

            _dbContext.Users.AddRange(backupData.Users);
            _dbContext.Customers.AddRange(backupData.Customers);

            foreach (var appointment in backupData.Appointments)
            {
                appointment.Start = DateTime.SpecifyKind(appointment.Start, DateTimeKind.Utc);
                appointment.End = DateTime.SpecifyKind(appointment.End, DateTimeKind.Utc);
                appointment.LastUpdate = DateTime.SpecifyKind(appointment.LastUpdate, DateTimeKind.Utc);
                appointment.CreateDate = DateTime.SpecifyKind(appointment.CreateDate, DateTimeKind.Utc);
            }

            _dbContext.Appointments.AddRange(backupData.Appointments);
            _dbContext.Addresses.AddRange(backupData.Addresses);
            _dbContext.Cities.AddRange(backupData.Cities);
            _dbContext.Countries.AddRange(backupData.Countries);

            _dbContext.SaveChanges();
        }
    }

    public class BackupData
    {
        public List<Models.UserModel> Users { get; set; }
        public List<Models.CustomerModel> Customers { get; set; }
        public List<Models.AppointmentModel> Appointments { get; set; }
        public List<Models.AddressModel> Addresses { get; set; }
        public List<Models.CityModel> Cities { get; set; }
        public List<Models.CountryModel> Countries { get; set; }
    }
}