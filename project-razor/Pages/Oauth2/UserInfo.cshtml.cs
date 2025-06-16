using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using project_razor.Models;

namespace project_razor.Pages.Oauth2
{
    public class UserInfoModel : PageModel
    {
        private readonly IamContext _context;

        public UserInfoModel(IamContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var authHeader = Request.Headers["Authorization"].ToString();

            if (!authHeader.StartsWith("Bearer "))
                return Unauthorized();

            var token = authHeader["Bearer ".Length..];

            var tokenRecord = await _context.Tokens
                .FirstOrDefaultAsync(t => t.AccessToken == token && t.ExpiresAt > DateTime.UtcNow);

            if (tokenRecord == null)
                return Unauthorized();

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == tokenRecord.Username);

            if (user == null)
                return NotFound();

            return new JsonResult(new
            {
                username = user.Username,
                email = user.Email,
                created_at = user.CreatedAt
            });
        }
    }
}
