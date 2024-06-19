using Microsoft.AspNetCore.Mvc;

namespace CamOn_FE.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
