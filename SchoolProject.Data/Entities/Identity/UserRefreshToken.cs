using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolProject.Data.Entities.Identity
{
    public class UserRefreshToken
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string? Token { get; set; }
        public string? JWTId { get; set; }
        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }
        [NotMapped] // Tells Entity Framework NOT to create a column for this in SQL Server
        public bool IsActive => !IsRevoked && !IsUsed && ExpiryDate > DateTime.UtcNow;
        public DateTime AddedTime { get; set; }
        public DateTime ExpiryDate { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }
        
    }
}
