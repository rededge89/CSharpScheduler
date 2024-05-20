using Scheduler.Data;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Scheduler.Services
{
    public class AlertService
    {
        private readonly ApplicationDbContext _dbContext;

        public AlertService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Models.AppointmentModel> GetUpcomingAppointments(int userId)
        {
            var now = DateTime.UtcNow;
            var upcomingAppointments = _dbContext.Appointments
                .Where(appt => appt.UserId == userId && appt.Start <= now.AddMinutes(15) && appt.Start > now)
                .ToList();

            return upcomingAppointments;
        }
    }
}