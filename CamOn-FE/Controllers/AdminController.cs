using BusinessObjects;
using CamOn_FE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CamOn_FE.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        public ActionResult Dashboard()
        {

            var packagesBoughtPerMonth = _context.UserPackages
                .GroupBy(up => new { up.StartDate.Year, up.StartDate.Month })
                .Select(g => new { g.Key.Year, g.Key.Month, Count = g.Count() })
                .ToList()
                .ToDictionary(x => $"{x.Year}-{x.Month:D2}", x => x.Count);

            var viewModel = new AdminDashboardViewModel
            {
                PackagesBoughtPerMonth = packagesBoughtPerMonth
            };

            return View(viewModel);
        }

        public async Task<IActionResult> AssignPackages()
        {
            var users = await _context.Users.ToListAsync();
            var packages = await _context.Packages.ToListAsync();

            var viewModel = new AdminAssignPackageViewModel
            {
                Users = users,
                Packages = packages
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AssignPackages(string userId, int packageId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var package = await _context.Packages.FindAsync(packageId);
            if (package == null)
            {
                return NotFound("Package not found");
            }
            var oldPackage = await _context.UserPackages.OrderByDescending(p => p.EndDate).FirstOrDefaultAsync(p => p.UserId.Equals(userId));
            if (oldPackage != null)
            {
                oldPackage.EndDate = DateTime.Now;
            }
            var userPackage = new UserPackage
            {
                UserId = user.Id,
                PackageId = package.Id,
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(package.MonthValue)
            };

            _context.UserPackages.Add(userPackage);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Package assigned successfully!";

            return RedirectToAction("AssignPackages");
        }
        
        public async Task<IActionResult> SearchUsers(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                var allUsers = await _context.Users.ToListAsync();
                return Json(allUsers);
            }

            var users = await _context.Users
                                      .Where(u => u.Email.Contains(email))
                                      .ToListAsync();
            return Json(users);
        }

        public async Task<IActionResult> SearchPackages(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                var allPackages = await _context.Packages.ToListAsync();
                return Json(allPackages);
            }

            var packages = await _context.Packages
                                         .Where(p => p.Name.Contains(name))
                                         .ToListAsync();
            return Json(packages);
        }
    }
}
