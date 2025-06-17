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

        // The registering user object
        [BindProperty]
        public User NewUser { get; set; } = new();

        // Password
        [BindProperty]
        [Required]
        [StringLength(18, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 18 characters.")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).+$",
            ErrorMessage = "Password must contain an uppercase letter, a number, and a special character.")]
        public string Password { get; set; } = string.Empty;

        [BindProperty]
        public bool IsDeveloper { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            NewUser.PasswordHash = ComputeSha256Hash(Password);
            NewUser.CreatedAt = DateTime.Now;

            if (!ModelState.IsValid)
                return Page();

            // Check if username already exists
            var exists = await _context.Users.AnyAsync(u => u.Username == NewUser.Username);
            if (exists)
            {
                ModelState.AddModelError("NewUser.Username", "Username is already taken.");
                return Page();
            }

            _context.Users.Add(NewUser);
            await _context.SaveChangesAsync();

            var roleName = IsDeveloper ? "developer" : "user";
            var roleId = await _context.Roles
                .Where(r => r.Name == roleName)
                .Select(r => r.Id)
                .FirstOrDefaultAsync();

            if (roleId != 0)
            {
                _context.UserRoles.Add(new UserRole
                {
                    UserId = NewUser.Id,
                    RoleId = roleId,
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
