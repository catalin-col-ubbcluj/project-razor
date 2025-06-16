using System.ComponentModel.DataAnnotations;

namespace project_razor.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(12, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 12 characters.")]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; } = string.Empty;

        // [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }

        public ICollection<Token>? Tokens { get; set; }
        public ICollection<UserRole>? UserRoles { get; set; }
    }
}
