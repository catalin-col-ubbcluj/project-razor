using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using project_razor.Models;

namespace project_razor.Pages.Tokens
{
    public class EditModel : PageModel
    {
        private readonly project_razor.Models.IamContext _context;

        public EditModel(project_razor.Models.IamContext context)
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

            var token =  await _context.Tokens.FirstOrDefaultAsync(m => m.Id == id);
            if (token == null)
            {
                return NotFound();
            }
            Token = token;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Token).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TokenExists(Token.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool TokenExists(int id)
        {
            return _context.Tokens.Any(e => e.Id == id);
        }
    }
}
