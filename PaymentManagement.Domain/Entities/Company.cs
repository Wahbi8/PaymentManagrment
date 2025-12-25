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
        [NotMapped]
        public Country? Country { get; set; }

        public Company(string name, string countryCode)
        {
            Name = string.IsNullOrEmpty(name) ? 
                throw new DomainException("The company name is empty") : name;

            CountryCode = string.IsNullOrEmpty(countryCode) ?
                throw new DomainException("The company country code is empty") : countryCode ;
        }

        private Company() { }
    }
}
