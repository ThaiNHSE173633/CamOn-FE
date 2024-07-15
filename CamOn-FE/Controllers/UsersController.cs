using BusinessObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CamOn_FE.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Account> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<Account> userManager, RoleManager<IdentityRole> roleManager,AppDbContext context)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = _context.Users.ToList();
            var accountRoles = users.Select(u => new AccountRole
            {
                Id = u.Id,
                UserName = u.UserName,
                RoleName = _userManager.GetRolesAsync(u).Result.FirstOrDefault()
            }).ToList();

            ViewBag.Roles = _roleManager.Roles.ToList();

            return View(accountRoles);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUserRole(string userId, string newRole)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                return Json(new { success = false, message = "User not found." });
            }

            var roles = await _userManager.GetRolesAsync(user);
            var removedResult = await _userManager.RemoveFromRolesAsync(user, roles.ToArray());
            if (!removedResult.Succeeded)
            {
                return Json(new { success = false, message = "Error removing user from existing roles." });
            }

            var addResult = await _userManager.AddToRoleAsync(user, newRole);
            if (addResult.Succeeded)
            {
                return Json(new { success = true, message = "User role updated successfully." });
            }
            else
            {
                return Json(new { success = false, message = "Error adding user to new role." });
            }
        }
    }
}
public class AccountRole : Account
{
    public string? RoleName { get; set; }
}