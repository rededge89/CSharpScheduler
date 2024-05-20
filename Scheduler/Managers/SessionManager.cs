using Scheduler.Data;

namespace Scheduler.Managers;

public class SessionManager
{
    // This holds the information of the current session
    public int UserId { get; set; }
    public string userName { get; set; }
    public string CurrentLocale { get; set; } = "en-US";
    public TimeZoneInfo CurrentTimeZone = TimeZoneInfo.Local;
    private readonly ApplicationDbContext _dbContext;

    public SessionManager(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void CheckForUpcomingAppointments()
    {
        var now = DateTime.UtcNow;
        var upcomingAppointments = _dbContext.Appointments
            .Where(appt => appt.UserId == UserId && appt.Start <= now.AddMinutes(15) && appt.Start > now)
            .ToList();

        if (upcomingAppointments.Any())
        {
            MessageBox.Show("You have an appointment starting within the next 15 minutes.", "Upcoming Appointment",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}