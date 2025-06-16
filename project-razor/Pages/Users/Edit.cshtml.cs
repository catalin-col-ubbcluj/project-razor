using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using project_razor.Models;

namespace project_razor.Pages.Users
{
    public class EditModel : PageModel
    {
        private readonly IamContext _context;

        public EditModel(IamContext context)
        {
            _context = context;
        }

        [BindProperty]
        public User User { get; set; } = default!;

        [BindProperty]
        public List<int> SelectedRoleIds { get; set; } = new();

        public List<Role> AllRoles { get; private set; } = new();

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return NotFound();

            User = await _context.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (User == null) return NotFound();

            AllRoles = await _context.Roles.ToListAsync();
            SelectedRoleIds = User.UserRoles.Select(ur => ur.RoleId).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            var userToUpdate = await _context.Users
                .Include(u => u.UserRoles)
                .FirstOrDefaultAsync(u => u.Id == User.Id);

            if (userToUpdate == null) return NotFound();

            // Update basic fields
            userToUpdate.Username = User.Username;
            userToUpdate.Email = User.Email;

            // Replace existing roles
            var existingRoles = _context.UserRoles.Where(ur => ur.UserId == userToUpdate.Id);
            _context.UserRoles.RemoveRange(existingRoles);

            foreach (var roleId in SelectedRoleIds)
            {
                _context.UserRoles.Add(new UserRole
                {
                    UserId = userToUpdate.Id,
                    RoleId = roleId,
                    AssignedAt = DateTime.UtcNow
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToPage("./Index");
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
