using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PaymentManagement.Domain.Entities
{
    [Table("payment_method")]
    public class PaymentMethod
    {
        [Column("id")]
        public Guid Id { get; set; }
        [Column("customenr_id")]
        public Guid CustomerId { get; set; }
        [Column("payment_id")]
        public Guid PaymentId { get; set; }
        [Column("type")]
        public int Type { get; set; }
        [Column("payment_token")]
        public string PaymentToken { get; set; }
        [Column("card_brand")]
        public string CardBrand { get; set; }
        [Column("bank_name")]
        public string Bank_name { get; set; }
        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; }
        

    }
}
