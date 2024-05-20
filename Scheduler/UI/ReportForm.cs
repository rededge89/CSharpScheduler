using System.Data;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Scheduler.Data;
using Scheduler.Managers;

namespace Scheduler.UI
{
    public partial class ReportForm : Form
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly SessionManager _sessionManager;

        public ReportForm(ApplicationDbContext dbContext, SessionManager sessionManager)
        {
            _dbContext = dbContext;
            _sessionManager = sessionManager;
            InitializeComponent();
            LoadReportTypes();
        }

        private void LoadReportTypes()
        {
            cmbReportType.Items.Add("Number of Appointment Types by Month");
            cmbReportType.Items.Add("Schedule for Each User");
            cmbReportType.Items.Add("Custom Report");
            cmbReportType.SelectedIndex = 0;
        }

        private void cmbReportType_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadReportData();
        }

        private void LoadReportData()
        {
            var selectedReport = cmbReportType.SelectedItem.ToString();
            DataTable dataTable = null;

            switch (selectedReport)
            {
                case "Number of Appointment Types by Month":
                    dataTable = GetAppointmentTypesByMonth();
                    break;
                case "Schedule for Each User":
                    dataTable = GetUserSchedules();
                    break;
                case "Custom Report":
                    dataTable = GetCustomReport();
                    break;
            }

            if (dataTable != null)
            {
                dataGridViewReport.DataSource = dataTable;
            }
        }

        private DataTable GetAppointmentTypesByMonth()
        {
            var reportData = _dbContext.Appointments
                .Where(a => a.UserId == _sessionManager.UserId)
                .GroupBy(a => new { a.Type, Month = a.Start.Month })
                .Select(g => new
                {
                    g.Key.Type,
                    g.Key.Month,
                    Count = g.Count()
                }).ToList();

            return ToDataTable(reportData);
        }

        private DataTable GetUserSchedules()
        {
            var table = new DataTable("User Schedule");
            table.Columns.Add("UserName", typeof(string));
            var daysOfWeek = Enum.GetValues(typeof(DayOfWeek))
                .Cast<DayOfWeek>()
                .Where(d => d >= DayOfWeek.Monday && d <= DayOfWeek.Friday)
                .ToList();

            foreach (var day in daysOfWeek)
            {
                table.Columns.Add(day.ToString(), typeof(string));
            }

            var users = _dbContext.Users
                .Include(u => u.Appointments)
                .ToList();

            foreach (var user in users)
            {
                var row = table.NewRow();
                row["UserName"] = user.UserName;

                foreach (var day in daysOfWeek)
                {
                    var appointments = user.Appointments
                        .Where(a => a.Start.DayOfWeek == day)
                        .OrderBy(a => a.Start)
                        .Select(a =>
                        {
                            var localStart = TimeZoneInfo.ConvertTimeFromUtc(a.Start, _sessionManager.CurrentTimeZone);
                            var localEnd = TimeZoneInfo.ConvertTimeFromUtc(a.End, _sessionManager.CurrentTimeZone);
                            return $"{localStart:hh:mm tt} - {localEnd:hh:mm tt}";
                        })
                        .ToList();

                    row[day.ToString()] = string.Join(", ", CondenseAppointments(appointments));
                }

                table.Rows.Add(row);
            }

            return table;
        }

        private List<string> CondenseAppointments(List<string> appointments)
        {
            var condensed = new List<string>();
            if (appointments.Count == 0)
            {
                return condensed;
            }

            DateTime start =
                DateTime.ParseExact(appointments[0].Split(" - ")[0], "hh:mm tt", CultureInfo.InvariantCulture);
            DateTime end =
                DateTime.ParseExact(appointments[0].Split(" - ")[1], "hh:mm tt", CultureInfo.InvariantCulture);

            for (int i = 1; i < appointments.Count; i++)
            {
                DateTime nextStart = DateTime.ParseExact(appointments[i].Split(" - ")[0], "hh:mm tt",
                    CultureInfo.InvariantCulture);
                DateTime nextEnd = DateTime.ParseExact(appointments[i].Split(" - ")[1], "hh:mm tt",
                    CultureInfo.InvariantCulture);

                if (nextStart <= end)
                {
                    if (nextEnd > end)
                    {
                        end = nextEnd;
                    }
                }
                else
                {
                    condensed.Add($"{start:hh:mm tt} - {end:hh:mm tt}");
                    start = nextStart;
                    end = nextEnd;
                }
            }

            condensed.Add($"{start:hh:mm tt} - {end:hh:mm tt}");

            return condensed;
        }

        private DataTable GetCustomReport()
        {
            var reportData = _dbContext.Customers
                .Select(c => new
                {
                    c.CustomerName,
                    c.Address.Phone
                }).ToList();

            return ToDataTable(reportData);
        }

        private DataTable ToDataTable<T>(List<T> items)
        {
            var dataTable = new DataTable(typeof(T).Name);
            var props = typeof(T).GetProperties(System.Reflection.BindingFlags.Public |
                                                System.Reflection.BindingFlags.Instance);
            foreach (var prop in props)
            {
                dataTable.Columns.Add(prop.Name, prop.PropertyType);
            }

            foreach (var item in items)
            {
                var values = new object[props.Length];
                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(item, null);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (dataGridViewReport.DataSource == null)
            {
                MessageBox.Show("No data to export.", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            using (var sfd = new SaveFileDialog())
            {
                sfd.Filter = "CSV files (*.csv)|*.csv";
                sfd.Title = "Save report as CSV file";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    ExportDataTableToCsv((DataTable)dataGridViewReport.DataSource, sfd.FileName);
                }
            }
        }

        private void ExportDataTableToCsv(DataTable dataTable, string filePath)
        {
            var lines = new List<string>();

            string[] columnNames = dataTable.Columns.Cast<DataColumn>().Select(column => column.ColumnName).ToArray();
            var header = string.Join(",", columnNames);
            lines.Add(header);

            var valueLines = dataTable.AsEnumerable().Select(row => string.Join(",", row.ItemArray));
            lines.AddRange(valueLines);

            File.WriteAllLines(filePath, lines);
        }
    }
}