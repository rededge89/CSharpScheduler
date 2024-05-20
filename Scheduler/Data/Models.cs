using System.ComponentModel.DataAnnotations;

namespace Scheduler.Data
{
    public class Models
    {
        public class AddressModel : TimeStampedEntity
        {
            [Key] public int AddressId { get; set; }
            public string Address { get; set; }
            public string Address2 { get; set; }
            public CityModel City { get; set; }
            public int CityId { get; set; }
            public string PostalCode { get; set; }
            [StringLength(15)] public string Phone { get; set; }
        }


        public class AppointmentModel : TimeStampedEntity
        {
            [Key] public int AppointmentId { get; set; }
            public CustomerModel Customer { get; set; }
            public int CustomerId { get; set; }
            public UserModel User { get; set; }
            public int UserId { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Location { get; set; }
            public string Contact { get; set; }
            public string Type { get; set; }
            public string Url { get; set; }
            public DateTime Start { get; set; }
            public DateTime End { get; set; }
        }

        public class CityModel : TimeStampedEntity
        {
            [Key] public int CityId { get; set; }
            public string City { get; set; }
            public CountryModel Country { get; set; }
            public int CountryId { get; set; }
        }

        public class CountryModel : TimeStampedEntity
        {
            [Key] public int CountryId { get; set; }
            public string Country { get; set; }
        }

        public class CustomerModel : TimeStampedEntity
        {
            [Key] public int CustomerId { get; set; }
            public string CustomerName { get; set; }
            public AddressModel Address { get; set; }
            public int AddressId { get; set; }
            public bool Active { get; set; }
        }

        public class UserModel : TimeStampedEntity
        {
            [Key] public int UserId { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
            public bool Active { get; set; }

            public virtual ICollection<AppointmentModel> Appointments { get; set; }
        }
    }
}