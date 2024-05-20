using Scheduler.Data;
using Scheduler.Managers;
using System.Linq;

namespace Scheduler.UI
{
    public partial class MainForm : Form
    {
        private readonly ApplicationDbContext _dbContext;
        private SessionManager _sessionManager;

        public MainForm(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            InitializeComponent();
            this.FormClosed += Main_FormClosed;

            LoadCustomers();
        }

        public void SetSession(SessionManager sessionManager)
        {
            _sessionManager = sessionManager;
            _dbContext.SetSession(sessionManager);
            UpdateAppointmentsView(DateTime.Now); // Ensure appointments are updated after setting session
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit(); // This will close the application entirely
        }

        private void LoadCustomers()
        {
            var customers = _dbContext.Customers
                .Select(c => new { c.CustomerId, c.CustomerName })
                .ToList();
            tblCustomers.DataSource = customers;
        }

        private void datCalendarPicker_ValueChanged(object sender, EventArgs e)
        {
        }

        private void UpdateAppointmentsView(DateTime selectedDate)
        {
            if (_sessionManager == null || _sessionManager.UserId == 0)
            {
                return; // Ensure session is set before updating appointments
            }

            lblViewingDate.Text = selectedDate.Date == DateTime.Now.Date
                ? "Today's Appointments:"
                : $"Appointments for {selectedDate:MM/dd/yyyy}";

            var appointments = _dbContext.Appointments
                .Where(appt => appt.Start.Date == selectedDate.Date && appt.UserId == _sessionManager.UserId)
                .ToList();
            tblAppointments.DataSource = ConvertToLocalTime(appointments);

            if (tblAppointments.Columns["AppointmentId"] == null) return;

            // Hide the ID Columns
            tblAppointments.Columns["AppointmentId"].Visible = false;
            tblAppointments.Columns["CustomerId"].Visible = false;
            tblAppointments.Columns["UserId"].Visible = false;
        }

        private List<dynamic> ConvertToLocalTime(List<Models.AppointmentModel> appointments)
        {
            var converted = appointments.Select(appt =>
            {
                try
                {
                    var startLocal = appt.Start.Kind == DateTimeKind.Utc
                        ? TimeZoneInfo.ConvertTimeFromUtc(appt.Start, _sessionManager.CurrentTimeZone)
                        : appt.Start;
                    var endLocal = appt.End.Kind == DateTimeKind.Utc
                        ? TimeZoneInfo.ConvertTimeFromUtc(appt.End, _sessionManager.CurrentTimeZone)
                        : appt.End;

                    // Debugging output to verify the conversion
                    // Console.WriteLine($"Original Start: {appt.Start}, Local Start: {startLocal}");
                    // Console.WriteLine($"Original End: {appt.End}, Local End: {endLocal}");

                    return new
                    {
                        appt.AppointmentId,
                        appt.CustomerId,
                        appt.UserId,
                        appt.Title,
                        Start = startLocal,
                        End = endLocal,
                        appt.Description,
                        appt.Location,
                        appt.Contact,
                        appt.Type,
                        appt.Url,
                        appt.CreateDate,
                        appt.CreatedBy,
                        LastUpdate = appt.LastUpdate.Kind == DateTimeKind.Utc
                            ? TimeZoneInfo.ConvertTimeFromUtc(appt.LastUpdate, _sessionManager.CurrentTimeZone)
                            : appt.LastUpdate,
                        appt.LastUpdateBy
                    };
                }
                catch (NullReferenceException ex)
                {
                    // Log the null reference exception details for debugging
                    Console.WriteLine($"Null reference encountered: {ex.Message}");
                    Console.WriteLine($"AppointmentId: {appt?.AppointmentId}");
                    Console.WriteLine($"CustomerId: {appt?.CustomerId}");
                    Console.WriteLine($"UserId: {appt?.UserId}");
                    return null;
                }
            }).Where(a => a != null).Cast<dynamic>().ToList();

            return converted;
        }

        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            using (var addCustomerForm = new CustomerForm(_dbContext, OperationType.Add))
            {
                var result = addCustomerForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    LoadCustomers(); // Refresh the customer list after the form is closed
                }
            }
        }

        private void btnEditCustomer_Click(object sender, EventArgs e)
        {
            if (tblCustomers.CurrentRow == null) return;
            int customerId = (int)tblCustomers.CurrentRow.Cells["CustomerId"].Value;

            using (var updateCustomerForm = new CustomerForm(_dbContext, OperationType.Update, customerId))
            {
                updateCustomerForm.ShowDialog();
                LoadCustomers(); // Refresh the customer list after the form is closed
            }
        }

        private void btnDeleteCustomer_Click(object sender, EventArgs e)
        {
            if (tblCustomers.CurrentRow != null)
            {
                var result = MessageBox.Show("Are you sure you want to delete this customer?", "Delete Customer",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    int customerId = (int)tblCustomers.CurrentRow.Cells["CustomerId"].Value;
                    var customer = _dbContext.Customers.Find(customerId);
                    if (customer != null)
                    {
                        _dbContext.Customers.Remove(customer);
                        _dbContext.SaveChanges();
                        LoadCustomers(); // Refresh the customer list
                    }
                }
            }
        }

        private void btnAddAppt_Click(object sender, EventArgs e)
        {
            using (var apptForm = new AppointmentForm(_dbContext, OperationType.Add))
            {
                apptForm.ShowDialog(); // Show the form as a modal dialog box
                UpdateAppointmentsView(datCalendarPicker
                    .SelectionStart); // Refresh the appointment list after closing the form
            }
        }

        private void btnEditAppt_Click(object sender, EventArgs e)
        {
            if (tblAppointments.CurrentRow == null)
            {
                MessageBox.Show("Please select an appointment to edit.", "Selection Required", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            int appointmentId = Convert.ToInt32(tblAppointments.CurrentRow.Cells["AppointmentId"].Value);
            using (var apptForm = new AppointmentForm(_dbContext, OperationType.Update, appointmentId))
            {
                apptForm.ShowDialog();
                UpdateAppointmentsView(datCalendarPicker
                    .SelectionStart); // Refresh the appointment list after the form is closed
            }
        }

        private void btnDeleteAppt_Click(object sender, EventArgs e)
        {
            if (tblAppointments.CurrentRow == null)
            {
                MessageBox.Show("Please select an appointment to delete.", "Selection Required", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            int appointmentId = Convert.ToInt32(tblAppointments.CurrentRow.Cells["AppointmentId"].Value);
            var result = MessageBox.Show("Are you sure you want to delete this appointment?", "Confirm Deletion",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                var appointment = _dbContext.Appointments.Find(appointmentId);
                if (appointment != null)
                {
                    _dbContext.Appointments.Remove(appointment);
                    _dbContext.SaveChanges();
                    MessageBox.Show("Appointment deleted successfully.", "Deletion Successful", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    UpdateAppointmentsView(datCalendarPicker.SelectionStart); // Refresh the appointment list
                }
                else
                {
                    MessageBox.Show("Failed to delete the appointment.", "Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void CheckForUpcomingAppointments(List<dynamic> localAppointments)
        {
            var now = DateTime.Now;
            var upcomingAppointments = localAppointments.Where(appt =>
                    appt.Start <= now.AddMinutes(15) && appt.Start > now &&
                    appt.UserId == _sessionManager.UserId)
                .ToList();

            if (upcomingAppointments.Any())
            {
                MessageBox.Show("You have an appointment starting within the next 15 minutes.", "Upcoming Appointment",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void datCalendarPicker_DateChanged(object sender, DateRangeEventArgs e)
        {
        }

        private void datCalendarPicker_DateSelected(object sender, DateRangeEventArgs e)
        {
            DateTime selectedDate = e.Start;
            UpdateAppointmentsView(selectedDate);
        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            var reportForm = new ReportForm(_dbContext, _sessionManager);
            reportForm.ShowDialog();
        }
    }
}