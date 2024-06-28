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
    public class UserPackagesController : Controller
    {
        private readonly AppDbContext _context;

        public UserPackagesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: UserPackages
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.UserPackages.Include(u => u.Package).Include(u => u.User);
            return View(await appDbContext.ToListAsync());
        }

        // GET: UserPackages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UserPackages == null)
            {
                return NotFound();
            }

            var userPackage = await _context.UserPackages
                .Include(u => u.Package)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userPackage == null)
            {
                return NotFound();
            }

            return View(userPackage);
        }

        // GET: UserPackages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UserPackages == null)
            {
                return NotFound();
            }

            var userPackage = await _context.UserPackages.FindAsync(id);
            if (userPackage == null)
            {
                return NotFound();
            }
            ViewData["PackageId"] = new SelectList(_context.Packages, "Id", "Id", userPackage.PackageId);
            ViewData["UserId"] = new SelectList(_context.Accounts, "Id", "Id", userPackage.UserId);
            return View(userPackage);
        }

        // POST: UserPackages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,PackageId,StartDate,EndDate")] UserPackage userPackage)
        {
            if (id != userPackage.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userPackage);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserPackageExists(userPackage.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["PackageId"] = new SelectList(_context.Packages, "Id", "Id", userPackage.PackageId);
            ViewData["UserId"] = new SelectList(_context.Accounts, "Id", "Id", userPackage.UserId);
            return View(userPackage);
        }

        // GET: UserPackages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UserPackages == null)
            {
                return NotFound();
            }

            var userPackage = await _context.UserPackages
                .Include(u => u.Package)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userPackage == null)
            {
                return NotFound();
            }

            return View(userPackage);
        }

        // POST: UserPackages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UserPackages == null)
            {
                return Problem("Entity set 'AppDbContext.UserPackages'  is null.");
            }
            var userPackage = await _context.UserPackages.FindAsync(id);
            if (userPackage != null)
            {
                _context.UserPackages.Remove(userPackage);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserPackageExists(int id)
        {
          return (_context.UserPackages?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
