using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using project_razor.Models;

namespace project_razor.Pages.OauthClients
{
    public class DetailsModel : PageModel
    {
        private readonly project_razor.Models.IamContext _context;

        public DetailsModel(project_razor.Models.IamContext context)
        {
            _context = context;
        }

        public OauthClient OauthClient { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var oauthclient = await _context.OauthClients.FirstOrDefaultAsync(m => m.Id == id);

            if (oauthclient is not null)
            {
                OauthClient = oauthclient;

                return Page();
            }

            return NotFound();
        }
    }
}
