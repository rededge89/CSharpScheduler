namespace Scheduler.Data.DTOs;

public class CustomerProfile
{
    public Models.CustomerModel Customer { get; set; }
    public Models.AddressModel Address { get; set; }
    public Models.CityModel City { get; set; }
    public Models.CountryModel Country { get; set; }
}