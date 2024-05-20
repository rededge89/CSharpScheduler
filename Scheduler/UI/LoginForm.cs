using Scheduler.Managers;
using Scheduler.Services;
using System;
using System.Globalization;
using System.Resources;
using System.Threading;
using System.Windows.Forms;
using Scheduler.Data;

namespace Scheduler.UI
{
    public partial class LogInForm : Form
    {
        private readonly AuthenticationService _authenticationService;
        private readonly MainForm _mainForm;
        private ResourceManager _rm;
        private readonly ApplicationDbContext _dbContext;

        public LogInForm(AuthenticationService authenticationService, MainForm mainForm, ApplicationDbContext dbContext)
        {
            InitializeComponent();
            _authenticationService = authenticationService;
            _mainForm = mainForm;
            _dbContext = dbContext;
            SetCulture();
        }

        private void SetCulture(string cultureName = "en-US")
        {
            CultureInfo culture = new CultureInfo(cultureName);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            _rm = new ResourceManager("Scheduler.Resources", typeof(LogInForm).Assembly);
        }

        private void DisplayLocale(object sender, EventArgs e)
        {
            var userLocale = LocalizationService.GetUserLocale();
            lbl_Locale.Text = $"Detected Locale: {userLocale}";
            SetCulture(userLocale);
            LoadLocalizedText();
        }

        private void LoadLocalizedText()
        {
            btnLogin.Text = _rm.GetString("LoginButton");
            lblUsername.Text = _rm.GetString("UserName");
            lblPassword.Text = _rm.GetString("Password");
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUserName.Text.Trim();
            string password = txtPassword.Text.Trim();
            try
            {
                var userId = _authenticationService.Login(username, password);

                if (userId > 0)
                {
                    var userSession = new SessionManager(_dbContext)
                    {
                        UserId = userId,
                        userName = username,
                        CurrentLocale = LocalizationService.GetUserLocale()
                    };
                    _mainForm.SetSession(userSession);
                    userSession.CheckForUpcomingAppointments();
                    _mainForm.Show();
                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(_rm.GetString("LoginError"));
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}