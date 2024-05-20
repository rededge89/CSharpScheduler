using Scheduler.Data;
using Scheduler.Services;
using System.Windows.Forms;

namespace Scheduler.Managers
{
    public class SessionManager
    {
        public int UserId { get; set; }
        public string userName { get; set; }
        public string CurrentLocale { get; set; } = "en-US";
        public TimeZoneInfo CurrentTimeZone = TimeZoneInfo.Local;
        private readonly ApplicationDbContext _dbContext;
        private readonly AlertService _alertService;

        public SessionManager(ApplicationDbContext dbContext, AlertService alertService)
        {
            _dbContext = dbContext;
            _alertService = alertService;
        }

        public void CheckForUpcomingAppointments()
        {
            var upcomingAppointments = _alertService.GetUpcomingAppointments(UserId);

            if (upcomingAppointments.Any())
            {
                MessageBox.Show("You have an appointment starting within the next 15 minutes.", "Upcoming Appointment",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}