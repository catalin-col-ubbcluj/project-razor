using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using project_razor.Models;
using System.Security.Cryptography;
// using System.Text;

namespace project_razor.Pages.Oauth2
{
    public class AuthorizeModel : PageModel
    {
        private readonly IamContext _context;

        public AuthorizeModel(IamContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(string client_id, string redirect_uri)
        {
            if (string.IsNullOrEmpty(client_id) || string.IsNullOrEmpty(redirect_uri))
                return BadRequest("Missing parameters.");

            var client = await _context.OauthClients
                .FirstOrDefaultAsync(c => c.ClientId == client_id && c.RedirectUri == redirect_uri);

            if (client == null)
                return BadRequest("Invalid client_id or redirect_uri.");

            if (!User.Identity?.IsAuthenticated ?? true)
                {
                    var returnUrl = Url.Page("/oauth2/authorize", null, new { client_id, redirect_uri }, Request.Scheme);
                    return Redirect($"/Auth/Login?returnUrl={Uri.EscapeDataString(returnUrl)}");

                }
            var token = GenerateToken();
            var username = User.Identity?.Name ?? throw new Exception("User not authenticated");
            var hoursAvailable = 1;
            var tokenRecord = new Token
            {
                AccessToken = token,
                Audience = client_id,
                Username = username,
                IssuedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddHours(hoursAvailable)
            };

            _context.Tokens.Add(tokenRecord);
            await _context.SaveChangesAsync();

            var redirectWithToken = $"{redirect_uri}#access_token={token}&token_type=Bearer&expires_in={hoursAvailable * 3600}";
            return Redirect(redirectWithToken);
        }

        private string GenerateToken()
        {
            var bytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }
    }
}
