using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentManagement.Domain
{
    [Table("user")]
    public class User
    {
        [Column("id")]
        public Guid Id { get; set; }
        [Column("name")]
        public string Name { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("Password")]
        public string Password { get; set; }
        [Column("company_id")]
        public Guid CompanyId { get; set; }
        [Column("is_admin")]
        public bool IsAdmin { get; set; }
    }
}
