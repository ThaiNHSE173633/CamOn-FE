using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObjects;
using Microsoft.AspNetCore.Identity;

namespace CamOn_FE.Controllers
{
    public class ProfileController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Account> _userManager;

        public ProfileController(UserManager<Account> userManager, AppDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: UserPackages
        public async Task<IActionResult> MyPackages()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var userPackages = _context.UserPackages.Include(u => u.Package).Include(u => u.User).Where(p => p.UserId.Equals(currentUser.Id)).OrderByDescending(p => p.EndDate);
            return View(await userPackages.ToListAsync());
        }
    }
}
