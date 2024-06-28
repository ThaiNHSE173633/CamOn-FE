using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObjects;

namespace CamOn_FE.Controllers
{
    public class BuyPackagesController : Controller
    {
        private readonly AppDbContext _context;

        public BuyPackagesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Packages
        public async Task<IActionResult> Index()
        {
              return _context.Packages != null ? 
                          View(await _context.Packages.ToListAsync()) :
                          Problem("Entity set 'AppDbContext.Packages'  is null.");
        }
    }
}
