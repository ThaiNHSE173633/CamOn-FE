using BusinessObjects;
using CamOn_FE.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CamOn_FE.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Dashboard()
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
        public async Task<IActionResult> Dashboard(string userId, int packageId)
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
            oldPackage.EndDate = DateTime.Now;

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

            return RedirectToAction("Dashboard");
        }
        
        // Method to search users by email
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

        // Method to search packages by name
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
