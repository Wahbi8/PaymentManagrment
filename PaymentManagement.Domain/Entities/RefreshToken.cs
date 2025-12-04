using System.ComponentModel.DataAnnotations.Schema;

namespace PaymentManagement.Domain.models
{
    [Table("refresh_token")]
    public class RefreshToken
    {
        [Column("id")]
        public int Id { get; set; }
        [Column("user_id")]
        public Guid UserId { get; set; }
        [Column("token")]
        public string Token { get; set; }
        [Column("jwt_id")]
        public string TokenId { get; set; }
        [Column("is_used")]
        public bool IsUsed { get; set; }
        [Column("is_revoked")]
        public bool IsRevoked { get; set; }
        [Column("added_date")]
        public DateTime AddedDate { get; set; }
        [Column("expire_date")]
        public DateTime ExpireDate { get; set; }
    }
}
