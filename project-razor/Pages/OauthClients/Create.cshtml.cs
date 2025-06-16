using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using project_razor.Models;

namespace project_razor.Pages.OauthClients
{
    public class CreateModel : PageModel
    {
        private readonly project_razor.Models.IamContext _context;

        public CreateModel(project_razor.Models.IamContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public OauthClient OauthClient { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.OauthClients.Add(OauthClient);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
