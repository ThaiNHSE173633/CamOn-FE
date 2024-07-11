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
    public class BuyPackagesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Account> _userManager;

        public BuyPackagesController(UserManager<Account> userManager, AppDbContext context)
        {
            _userManager = userManager; 
            _context = context;
        }

        // GET: Packages
        public async Task<IActionResult> Index()
        {
              return _context.Packages != null ? 
                          View(await _context.Packages.ToListAsync()) :
                          Problem("Entity set 'AppDbContext.Packages'  is null.");
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Packages == null)
            {
                return NotFound();
            }

            var package = await _context.Packages
                .FirstOrDefaultAsync(m => m.Id == id);
            if (package == null)
            {
                return NotFound();
            }
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return RedirectToAction("Index","Home");
            }
            ViewData["userEmail"] = currentUser.Email;

            return View(package);
        }

    }
}
