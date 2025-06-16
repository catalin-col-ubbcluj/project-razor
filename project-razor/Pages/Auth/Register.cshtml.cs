using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using project_razor.Models;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace project_razor.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        private readonly IamContext _context;

        public RegisterModel(IamContext context)
        {
            _context = context;
        }

        [BindProperty]
        public User RegisterUser { get; set; } = new();

        [BindProperty]
        [Required]
        [StringLength(18, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 18 characters.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).+$",
            ErrorMessage = "Password must contain an uppercase letter, a number, and a special character.")]
        public string Password { get; set; } = string.Empty;

        public string? UsernameError { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            RegisterUser.PasswordHash = ComputeSha256Hash(Password);
            RegisterUser.CreatedAt = DateTime.Now;

            // Check if username already exists
            var exists = await _context.Users.AnyAsync(u => u.Username == RegisterUser.Username);
            if (!ModelState.IsValid)
                return Page();


            if (exists)
            {
                UsernameError = "Username is already taken.";
                return Page();
            }

            _context.Users.Add(RegisterUser);
            await _context.SaveChangesAsync();

            var userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "user");
            if (userRole != null)
            {
                _context.UserRoles.Add(new UserRole
                {
                    UserId = RegisterUser.Id,
                    RoleId = userRole.Id,
                    AssignedAt = DateTime.UtcNow
                });

                await _context.SaveChangesAsync(); 
            }
            return RedirectToPage("/Auth/Login");
        }

        private string ComputeSha256Hash(string rawData)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            return Convert.ToBase64String(bytes);
        }
    }
}
