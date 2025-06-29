using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using project_razor.Models;

namespace project_razor.Pages.Tokens
{
    public class DeleteModel : PageModel
    {
        private readonly project_razor.Models.IamContext _context;

        public DeleteModel(project_razor.Models.IamContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Token Token { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var token = await _context.Tokens.FirstOrDefaultAsync(m => m.Id == id);

            if (token is not null)
            {
                Token = token;

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var token = await _context.Tokens.FindAsync(id);
            if (token != null)
            {
                Token = token;
                _context.Tokens.Remove(Token);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
