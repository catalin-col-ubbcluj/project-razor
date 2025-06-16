using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using project_razor.Models;

namespace project_razor.Pages.Tokens
{
    public class IndexModel : PageModel
    {
        private readonly IamContext _context;

        public IndexModel(IamContext context)
        {
            _context = context;
        }

        public List<Token> MyTokens { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                return Unauthorized();

            MyTokens = await _context.Tokens
                .Where(t => t.Username == username)
                .OrderByDescending(t => t.IssuedAt)
                .ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostRevokeAsync(int id)
        {
            var token = await _context.Tokens.FirstOrDefaultAsync(t => t.Id == id);
            if (token == null || token.Username != User.Identity?.Name)
                return Unauthorized();

            _context.Tokens.Remove(token);
            await _context.SaveChangesAsync();
            return RedirectToPage();
        }
    }
}
