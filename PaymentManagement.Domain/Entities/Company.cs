using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentManagement.Domain
{
    [Table("company")]
    public class Company
    {
        [Column("id")]
        public Guid Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("country_code")]
        public string CountryCode{ get; set; }
        public Country? Country { get; set; }

        //public Company(string name, string countryCode)
        //{
        //    Name = name ?? throw new ArgumentNullException(nameof(name));
        //    CountryCode = countryCode ?? throw new ArgumentNullException(nameof(countryCode));
        //}

        //// EF Core still needs a parameterless constructor
        //private Company() { }
    }
}
