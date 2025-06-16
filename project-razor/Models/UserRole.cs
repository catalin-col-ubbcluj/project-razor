using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace project_razor.Models
{
    public class UserRole
    {
        [Key, Column(Order = 0)]
        public int UserId { get; set; }
        public User? User { get; set; }

        [Key, Column(Order = 1)]
        public int RoleId { get; set; }
        public Role? Role { get; set; }

        public DateTime AssignedAt { get; set; }
    }
}
