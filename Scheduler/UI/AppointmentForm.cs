using Scheduler.Data;
using Microsoft.EntityFrameworkCore;

namespace Scheduler.UI
{

    public partial class AppointmentForm : Form
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly OperationType _operationType;
        private Models.AppointmentModel _appointment;

        public AppointmentForm(ApplicationDbContext dbContext, OperationType operationType, int? appointmentId = null)
        {
            _dbContext = dbContext;
            _operationType = operationType;
            InitializeComponent();
            ConfigureDateTimePickers();

            if (_operationType == OperationType.Update && appointmentId.HasValue)
            {
                LoadCustomersIntoComboBox();
                PopulateAppointmentDetails(appointmentId.Value);
            }
            else
            {
                txtApptId.Visible = false;
                label1.Visible = false;
                _appointment = new Models.AppointmentModel();
                LoadCustomersIntoComboBox();
            }
        }

        private void ConfigureDateTimePickers()
        {
            dtStartTime.Format = DateTimePickerFormat.Custom;
            dtStartTime.CustomFormat = "MM/dd/yyyy hh:mm tt";
            dtEndTime.Format = DateTimePickerFormat.Custom;
            dtEndTime.CustomFormat = "MM/dd/yyyy hh:mm tt";
        }

        private void PopulateAppointmentDetails(int appointmentId)
        {
            _appointment = _dbContext.Appointments
                .Include(a => a.Customer)
                .FirstOrDefault(a => a.AppointmentId == appointmentId);

            if (_appointment == null)
            {
                MessageBox.Show("Failed to load appointment details.", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                this.Close();
                return;
            }

            txtApptId.Text = _appointment.AppointmentId.ToString();
            txtTitle.Text = _appointment.Title;
            txtDescription.Text = _appointment.Description;
            txtLocation.Text = _appointment.Location;
            txtContact.Text = _appointment.Contact;
            txtType.Text = _appointment.Type;
            txtUrl.Text = _appointment.Url;
            dtStartTime.Value = TimeZoneInfo.ConvertTimeFromUtc(_appointment.Start, TimeZoneInfo.Local);
            dtEndTime.Value = TimeZoneInfo.ConvertTimeFromUtc(_appointment.End, TimeZoneInfo.Local);
            cmbCustomer.SelectedValue = _appointment.CustomerId;
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            if (ValidateAppointmentDetails())
            {
                BuildAppointmentObject();

                try
                {
                    if (_operationType == OperationType.Add)
                    {
                        _dbContext.Appointments.Add(_appointment);
                    }
                    else
                    {
                        _dbContext.Appointments.Update(_appointment);
                    }

                    _dbContext.SaveChanges();
                    MessageBox.Show("Appointment saved successfully.");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to save the appointment: {ex.Message}", "Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BuildAppointmentObject()
        {
            _appointment.UserId = 1; // This should be replaced with the actual logged-in user ID
            _appointment.Title = txtTitle.Text;
            _appointment.Description = txtDescription.Text;
            _appointment.Location = txtLocation.Text;
            _appointment.Contact = txtContact.Text;
            _appointment.Type = txtType.Text;
            _appointment.Url = txtUrl.Text;
            _appointment.Start = dtStartTime.Value.ToUniversalTime();
            _appointment.End = dtEndTime.Value.ToUniversalTime();
            _appointment.CustomerId = Convert.ToInt32(cmbCustomer.SelectedValue);
        }

        private bool ValidateAppointmentDetails()
        {
            if (dtStartTime.Value.Date != dtEndTime.Value.Date)
            {
                MessageBox.Show("Appointment start and end times must be on the same day.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            DayOfWeek dayOfWeek = dtStartTime.Value.DayOfWeek;
            TimeSpan startTime = dtStartTime.Value.TimeOfDay;
            TimeSpan endTime = dtEndTime.Value.TimeOfDay;

            if (dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday)
            {
                MessageBox.Show("Appointments can only be scheduled Monday through Friday.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (startTime < new TimeSpan(9, 0, 0) || endTime > new TimeSpan(17, 0, 0))
            {
                MessageBox.Show("Appointments must be scheduled between 9:00 AM and 5:00 PM.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void LoadCustomersIntoComboBox()
        {
            var customers = _dbContext.Customers
                .Select(c => new { c.CustomerId, c.CustomerName })
                .ToList();
            cmbCustomer.DisplayMember = "CustomerName"; // Field to display
            cmbCustomer.ValueMember = "CustomerId"; // Field to use as the value
            cmbCustomer.DataSource = customers;
            cmbCustomer.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbCustomer.AutoCompleteSource = AutoCompleteSource.ListItems;
        }
    }
}