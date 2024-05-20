namespace Scheduler.Data;

public class DataSeeder
{
    // private readonly ApplicationDbContext _dbContext;
    //
    // public DataSeeder(ApplicationDbContext dbContext)
    // {
    //     _dbContext = dbContext;
    // }
    //
    // public void Seed()
    // {
    //     Console.WriteLine("Seeding data...");
    //     SeedUsers();
    //     SeedCustomers();
    //     SeedAppointments();
    // }
    //
    // private void SeedUsers()
    // {
    //     if (_dbContext.Users.Any()) return;
    //
    //     var userFaker = new Faker<Models.UserModel>()
    //         .RuleFor(u => u.UserName, f => f.Internet.UserName())
    //         .RuleFor(u => u.Password, f => f.Internet.Password())
    //         .RuleFor(u => u.Active, f => f.Random.Bool())
    //         .RuleFor(u => u.CreateDate, f => f.Date.Past())
    //         .RuleFor(u => u.CreatedBy, f => f.Internet.UserName())
    //         .RuleFor(u => u.LastUpdate, f => f.Date.Past())
    //         .RuleFor(u => u.LastUpdateBy, f => f.Internet.UserName());
    //
    //     var testUser = new Models.UserModel
    //     {
    //         UserName = "test",
    //         Password = "test",
    //         Active = true,
    //         CreateDate = DateTime.UtcNow,
    //         CreatedBy = "system",
    //         LastUpdate = DateTime.UtcNow,
    //         LastUpdateBy = "system"
    //     };
    //
    //     _dbContext.Users.Add(testUser);
    //     _dbContext.Users.AddRange(userFaker.Generate(10)); // Generate 10 additional users
    //     _dbContext.SaveChanges(); // Save after adding users
    // }
    //
    // private void SeedCustomers()
    // {
    //     if (_dbContext.Customers.Any()) return;
    //
    //     var countryFaker = new Faker<Models.CountryModel>()
    //         .RuleFor(c => c.Country, f => f.Address.Country())
    //         .RuleFor(c => c.CreateDate, f => f.Date.Past())
    //         .RuleFor(c => c.CreatedBy, f => f.Internet.UserName())
    //         .RuleFor(c => c.LastUpdate, f => f.Date.Past())
    //         .RuleFor(c => c.LastUpdateBy, f => f.Internet.UserName());
    //
    //     var cityFaker = new Faker<Models.CityModel>()
    //         .RuleFor(c => c.City, f => f.Address.City())
    //         .RuleFor(c => c.Country, f => countryFaker.Generate())
    //         .RuleFor(c => c.CreateDate, f => f.Date.Past())
    //         .RuleFor(c => c.CreatedBy, f => f.Internet.UserName())
    //         .RuleFor(c => c.LastUpdate, f => f.Date.Past())
    //         .RuleFor(c => c.LastUpdateBy, f => f.Internet.UserName());
    //
    //     var addressFaker = new Faker<Models.AddressModel>()
    //         .RuleFor(a => a.Address, f => f.Address.StreetAddress())
    //         .RuleFor(a => a.Address2, f => f.Address.SecondaryAddress())
    //         .RuleFor(a => a.City, f => cityFaker.Generate())
    //         .RuleFor(a => a.PostalCode, f => f.Address.ZipCode())
    //         .RuleFor(a => a.Phone,
    //             f => Regex.Replace(f.Phone.PhoneNumber(), "[^0-9-]", "").PadRight(15, '0').Substring(0, 15))
    //         .RuleFor(a => a.CreateDate, f => f.Date.Past())
    //         .RuleFor(a => a.CreatedBy, f => f.Internet.UserName())
    //         .RuleFor(a => a.LastUpdate, f => f.Date.Past())
    //         .RuleFor(a => a.LastUpdateBy, f => f.Internet.UserName());
    //
    //     var customerFaker = new Faker<Models.CustomerModel>()
    //         .RuleFor(c => c.CustomerName, f => f.Company.CompanyName())
    //         .RuleFor(c => c.Address, f => addressFaker.Generate())
    //         .RuleFor(c => c.Active, f => f.Random.Bool())
    //         .RuleFor(c => c.CreateDate, f => f.Date.Past())
    //         .RuleFor(c => c.CreatedBy, f => f.Internet.UserName())
    //         .RuleFor(c => c.LastUpdate, f => f.Date.Past())
    //         .RuleFor(c => c.LastUpdateBy, f => f.Internet.UserName());
    //
    //     _dbContext.Customers.AddRange(customerFaker.Generate(20)); // Generate 20 customers
    //     _dbContext.SaveChanges(); // Save after adding customers
    // }
    //
    // private void SeedAppointments()
    // {
    //     if (_dbContext.Appointments.Any()) return;
    //
    //     var users = _dbContext.Users.Include(u => u.Appointments).ToList();
    //     var customers = _dbContext.Customers.ToList();
    //
    //     var allAppointments = new List<Models.AppointmentModel>();
    //
    //     foreach (var user in users)
    //     {
    //         for (int monthOffset = -1; monthOffset <= 1; monthOffset++)
    //         {
    //             allAppointments.AddRange(GenerateAppointmentsForUser(user, customers, monthOffset));
    //         }
    //     }
    //
    //     _dbContext.Appointments.AddRange(allAppointments);
    //     _dbContext.SaveChanges(); // Save once after generating all appointments
    // }
    //
    // private List<Models.AppointmentModel> GenerateAppointmentsForUser(Models.UserModel user,
    //     List<Models.CustomerModel> customers, int monthOffset)
    // {
    //     var faker = new Faker();
    //     var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
    //     var appointments = new List<Models.AppointmentModel>();
    //
    //     var startDate = DateTime.UtcNow.AddMonths(monthOffset);
    //     var endDate = startDate.AddMonths(1);
    //
    //     for (var date = startDate; date < endDate; date = date.AddDays(1))
    //     {
    //         if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
    //         {
    //             continue; // Skip weekends
    //         }
    //
    //         var appointmentCount = faker.Random.Int(2, 6); // Generate 2 to 6 appointments per day
    //
    //         for (int i = 0; i < appointmentCount; i++)
    //         {
    //             DateTime start, end;
    //
    //             do
    //             {
    //                 var hour = faker.Random.Int(9, 16); // Between 9 AM and 4 PM
    //                 var minute = faker.Random.Int(0, 3) * 15; // Quarter hour intervals
    //                 start = new DateTime(date.Year, date.Month, date.Day, hour, minute, 0,
    //                     DateTimeKind.Unspecified);
    //                 start = TimeZoneInfo.ConvertTimeToUtc(start, timeZoneInfo);
    //                 end = start.AddHours(1); // Ensure 1-hour appointments
    //             } while (IsOverlapping(start, end, appointments));
    //
    //             var customer = faker.PickRandom(customers); // Randomly pick a customer for each appointment
    //
    //             appointments.Add(new Models.AppointmentModel
    //             {
    //                 Title = faker.Lorem.Sentence(),
    //                 Description = faker.Lorem.Paragraph(),
    //                 Location = faker.Address.FullAddress(),
    //                 Contact = faker.Person.FullName,
    //                 Type = faker.PickRandom(new[] { "Consultation", "Follow-up", "Routine Check" }),
    //                 Url = faker.Internet.Url(),
    //                 CreateDate = faker.Date.Past(),
    //                 CreatedBy = faker.Internet.UserName(),
    //                 LastUpdate = faker.Date.Past(),
    //                 LastUpdateBy = faker.Internet.UserName(),
    //                 Start = start,
    //                 End = end,
    //                 User = user,
    //                 Customer = customer
    //             });
    //         }
    //     }
    //
    //     return appointments;
    // }
    //
    // private bool IsOverlapping(DateTime start, DateTime end, List<Models.AppointmentModel> appointments)
    // {
    //     return appointments.Any(a => a.Start < end && a.End > start);
    // }
}