using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BusinessObjects;
using Microsoft.Data.SqlClient.Server;
using Newtonsoft.Json;
using NuGet.Common;
using System.Net.Http.Headers;
using System.Text;

namespace CamOn_FE.Controllers
{
    public class PackagesController : Controller
    {
        private readonly string _baseUrl;
        private readonly AppDbContext _context;

        public PackagesController(IConfiguration config, AppDbContext context)
        {
            _context = context;
            _baseUrl = config["ApiSettings:BaseUrl"];
        }

        // GET: Packages
        public async Task<IActionResult> Index()
        {
            /*
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);

                try
                {
                    var response = await client.GetAsync("v1/packages");

                    if (response.IsSuccessStatusCode)
                    {
                        var jsonResponse = await response.Content.ReadAsStringAsync();
                        var list = JsonConvert.DeserializeObject<List<Package>>(jsonResponse);
                        return View(list);
                    }
                    else
                    {

                        return RedirectToAction("Index", "Home");
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "An error occurred: " + ex.Message;
                    return View();
                }
            }*/
            return _context.Packages != null ?
                        View(await _context.Packages.ToListAsync()) :
                        Problem("Entity set 'AppDbContext.Packages'  is null.");
        }
        // GET: Packages/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Packages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,MonthValue,Price,Id")] Package package)
        {

            string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(package);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                try
                {
                    var response = await client.PostAsync("v1/packages", new StringContent(jsonContent, Encoding.UTF8, "application/json"));

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        return View();
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "An error occurred: " + ex.Message;
                    return View();
                }
            }
        }

        //// GET: Packages/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(_baseUrl);

        //        client.DefaultRequestHeaders.Accept.Clear();
        //        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //        try
        //        {
        //            // Send POST request with JSON content
        //            var filter = $"?$filter=CategoryId eq {id}";
        //            var response = await client.GetAsync("odata/Category" + filter);

        //            if (response.IsSuccessStatusCode)
        //            {
        //                var jsonResponse = await response.Content.ReadAsStringAsync();

        //                var odataResponse = JsonConvert.DeserializeObject<ODataResponse<Category>>(jsonResponse);

        //                return View(odataResponse.Value.First());
        //            }
        //            else
        //            {
        //                return RedirectToAction("Index", "Home");
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ViewBag.ErrorMessage = "An error occurred: " + ex.Message;
        //            return View();
        //        }
        //    }
        //}

        //// POST: Packages/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Name,MonthValue,Price,Id")] Package package)
        //{
        //    if (id != package.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(package);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!PackageExists(package.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(package);
        //}

        //// GET: Packages/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null || _context.Packages == null)
        //    {
        //        return NotFound();
        //    }

        //    var package = await _context.Packages
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (package == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(package);
        //}

        //// POST: Packages/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    if (_context.Packages == null)
        //    {
        //        return Problem("Entity set 'AppDbContext.Packages'  is null.");
        //    }
        //    var package = await _context.Packages.FindAsync(id);
        //    if (package != null)
        //    {
        //        _context.Packages.Remove(package);
        //    }
            
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool PackageExists(int id)
        //{
        //  return (_context.Packages?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}
