using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using project_razor.Models;
using System.Security.Cryptography;
using System.Text;

namespace project_razor.Pages.Users
{
    public class MyProfileModel : PageModel
    {
        private readonly IamContext _context;

        public MyProfileModel(IamContext context)
        {
            _context = context;
        }

        [BindProperty]
        public User CurrentUser { get; set; } = new();

        [BindProperty]
        public string? NewPassword { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username)) return Unauthorized();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return NotFound();

            CurrentUser = user;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == CurrentUser.Id);
            if (user == null || user.Username != User.Identity?.Name) return Unauthorized();

            user.Email = CurrentUser.Email;

            if (!string.IsNullOrEmpty(NewPassword))
                user.PasswordHash = ComputeSha256Hash(NewPassword);

            await _context.SaveChangesAsync();
            TempData["Success"] = "Profile updated.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username)) return Unauthorized();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Auth/Logout");
        }

        private string ComputeSha256Hash(string raw)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(raw));
            return Convert.ToBase64String(bytes);
        }
    }
}
