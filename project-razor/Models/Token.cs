using System;
using System.ComponentModel.DataAnnotations;

namespace project_razor.Models
{
    public class Token
    {
        public int Id { get; set; }

        [Required]
        public string AccessToken { get; set; } = null!;
    
        [Required]
        public string Audience { get; set; } = string.Empty;

        public DateTime IssuedAt { get; set; }

        public DateTime ExpiresAt { get; set; }

        [Required]
        public string Username { get; set; } = null!;
    }
}
