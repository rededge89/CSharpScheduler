using System.Text.Json;

namespace Scheduler.Data;

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

        var json = JsonSerializer.Serialize(backupData, new JsonSerializerOptions
        {
            WriteIndented = true,
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
        });

        File.WriteAllText(filePath, json);
    }

    public void RestoreDatabase(string filePath)
    {
        var json = File.ReadAllText(filePath);
        var backupData = JsonSerializer.Deserialize<BackupData>(json, new JsonSerializerOptions
        {
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
        });

        if (backupData == null)
        {
            throw new InvalidOperationException("Failed to deserialize backup data.");
        }

        _dbContext.Users.AddRange(backupData.Users);
        _dbContext.Customers.AddRange(backupData.Customers);
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