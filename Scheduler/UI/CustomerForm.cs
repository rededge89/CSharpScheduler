using Scheduler.Data;
using Scheduler.Data.DTOs;
using Scheduler.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Scheduler.UI
{
    public partial class CustomerForm : Form
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly OperationType _operationType;
        private CustomerProfile _customerProfile;

        public CustomerForm(ApplicationDbContext dbContext, OperationType operationType, int? customerId = null)
        {
            _dbContext = dbContext;
            _operationType = operationType;
            InitializeComponent();

            if (_operationType == OperationType.Update && customerId.HasValue)
            {
                PopulateCustomerProfile(customerId.Value);
            }
            else
            {
                txtId.Visible = false;
                lblId.Visible = false;
                ckbxIsActive.Checked = true; // Default to active
                _customerProfile = new CustomerProfile
                {
                    Customer = new Models.CustomerModel(),
                    Address = new Models.AddressModel(),
                    City = new Models.CityModel(),
                    Country = new Models.CountryModel()
                };
            }
        }

        private void PopulateCustomerProfile(int customerId)
        {
            var customer = _dbContext.Customers
                .Include(c => c.Address)
                .ThenInclude(a => a.City)
                .ThenInclude(c => c.Country)
                .FirstOrDefault(c => c.CustomerId == customerId);

            if (customer != null)
            {
                _customerProfile = new CustomerProfile
                {
                    Customer = customer,
                    Address = customer.Address,
                    City = customer.Address.City,
                    Country = customer.Address.City.Country
                };

                txtId.Text = customer.CustomerId.ToString();
                txtname.Text = customer.CustomerName;
                txtAddress1.Text = customer.Address.Address;
                txtAddress2.Text = customer.Address.Address2 ?? ""; // Address2 might be null
                txtCity.Text = customer.Address.City.City;
                txtPostalCode.Text = customer.Address.PostalCode;
                txtCountry.Text = customer.Address.City.Country.Country;
                txtPhone.Text = customer.Address.Phone;
                ckbxIsActive.Checked = customer.Active;
            }
            else
            {
                MessageBox.Show("Customer profile could not be loaded.", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnAccept_Click(object sender, EventArgs e)
        {
            // Trim all input fields
            txtname.Text = txtname.Text.Trim();
            txtAddress1.Text = txtAddress1.Text.Trim();
            txtAddress2.Text = txtAddress2.Text.Trim();
            txtCity.Text = txtCity.Text.Trim();
            txtCountry.Text = txtCountry.Text.Trim();
            txtPhone.Text = txtPhone.Text.Trim();
            txtPostalCode.Text = txtPostalCode.Text.Trim();

            if (!ValidateCustomer(txtname.Text, txtAddress1.Text, txtCity.Text, txtPostalCode.Text, txtCountry.Text,
                    txtPhone.Text))
            {
                return; // Validation failed
            }

            try
            {
                _customerProfile.Customer.CustomerName = txtname.Text.Trim();
                _customerProfile.Customer.Active = ckbxIsActive.Checked;
                _customerProfile.Address.Address = txtAddress1.Text.Trim();
                _customerProfile.Address.Address2 = txtAddress2.Text.Trim();
                _customerProfile.Address.Phone = txtPhone.Text.Trim();
                _customerProfile.Address.PostalCode = txtPostalCode.Text.Trim();
                _customerProfile.City.City = txtCity.Text.Trim();
                _customerProfile.Country.Country = txtCountry.Text.Trim();

                // Establish relationships
                _customerProfile.Address.City = _customerProfile.City;
                _customerProfile.City.Country = _customerProfile.Country;
                _customerProfile.Customer.Address = _customerProfile.Address;

                // Attach entities to context if they are new
                if (_operationType == OperationType.Add)
                {
                    _dbContext.Countries.Add(_customerProfile.Country);
                    _dbContext.SaveChanges();
                    _customerProfile.City.CountryId = _customerProfile.Country.CountryId;

                    _dbContext.Cities.Add(_customerProfile.City);
                    _dbContext.SaveChanges();
                    _customerProfile.Address.CityId = _customerProfile.City.CityId;

                    _dbContext.Addresses.Add(_customerProfile.Address);
                    _dbContext.SaveChanges();
                    _customerProfile.Customer.AddressId = _customerProfile.Address.AddressId;

                    _dbContext.Customers.Add(_customerProfile.Customer);
                }
                else
                {
                    _dbContext.Countries.Update(_customerProfile.Country);
                    _dbContext.Cities.Update(_customerProfile.City);
                    _dbContext.Addresses.Update(_customerProfile.Address);
                    _dbContext.Customers.Update(_customerProfile.Customer);
                }

                _dbContext.SaveChanges();
                this.DialogResult = DialogResult.OK; // Indicate success
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private bool ValidateCustomer(string name, string address1, string city, string postalCode, string country,
            string phone)
        {
            if (!ValidationUtility.IsNonEmptyTrimmed(name) ||
                !ValidationUtility.IsNonEmptyTrimmed(address1) ||
                !ValidationUtility.IsNonEmptyTrimmed(city) ||
                !ValidationUtility.IsNonEmptyTrimmed(country) ||
                !ValidationUtility.IsNonEmptyTrimmed(postalCode) ||
                !ValidationUtility.IsNonEmptyTrimmed(phone))
            {
                MessageBox.Show(
                    "Please provide valid customer details. Name, Address, City, Postal Code, Country, and Phone are required fields.",
                    "Validation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            if (!ValidationUtility.IsValidPhoneNumber(phone))
            {
                MessageBox.Show("Please provide a valid phone number.  Must only contain digits and/or dashes",
                    "Validation Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}