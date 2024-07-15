using BusinessObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Text;

namespace CamOn_FE.Controllers
{
    public class UserCameraController : Controller
    {
        private readonly string _baseUrl;
        private IWebHostEnvironment _environment;
        private readonly AppDbContext _context;
        private readonly UserManager<Account> _userManager;

        public UserCameraController(IWebHostEnvironment environment,AppDbContext context,
            IConfiguration config, UserManager<Account> userManager)
        {
            _environment = environment;
            _context = context;
            _baseUrl = config["ApiSettings:BaseUrl"];
            _userManager = userManager;
        }
        public async Task<IActionResult> IndexAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var items = _context.Cameras.Where(c => c.AccountId == currentUser.Id).ToList();
            // Pass the list of items to the view
            return View(items);
        }
        public async Task<IActionResult> Details(string id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var items = _context.Cameras.Where(c => c.AccountId == currentUser.Id).ToList();
            // Pass the list of items to the view
            ViewBag.UserCameras = items;
            return View((object)id);
        }
        public IActionResult AddCamera()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddCamera(string cameraName, string cameraIPAddress)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var userId = currentUser.Id;

            var currentPackage = GetCurrentActivePackage(userId);
            if (currentPackage == null || DateTime.Now > currentPackage.EndDate)
            {
                ViewBag.ErrorMessage = "You do not have an active package.";
                return View();
            }

            var userCameraCount = _context.Cameras.Count(c => c.AccountId == userId);
            var currentPackageDetails = _context.Packages.FirstOrDefault(p => p.Id == currentPackage.PackageId);
            if (userCameraCount >= currentPackageDetails.CameraValue)
            {
                ViewBag.ErrorMessage = "You have reached the camera limit for your current package.";
                return View();
            }
            var camera = new Camera();
            camera.Name = cameraName;
            camera.IpAddress = cameraIPAddress;
            if (currentUser != null) {
                camera.AccountId = currentUser.Id;
                _context.Add(camera);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "UserCamera");
            }
            ViewBag.ErrorMessage = "Error adding camera. Please try again later.";
            return View(camera);
        }
        //[HttpPost]
        //public async Task<ActionResult> AddCamera(string cameraName, string cameraIPAddress)
        //{
        //    var currentUser = await _userManager.GetUserAsync(User);
        //    var userId = currentUser.Id;

        //    var currentPackage = GetCurrentActivePackage(userId);
        //    if (currentPackage == null || DateTime.Now > currentPackage.EndDate)
        //    {
        //        ViewBag.ErrorMessage = "You do not have an active package.";
        //        return View();
        //    }

        //    var userCameraCount = _context.Cameras.Count(c => c.AccountId == userId);
        //    var currentPackageDetails = _context.Packages.FirstOrDefault(p => p.Id == currentPackage.PackageId);
        //    if (userCameraCount >= currentPackageDetails.CameraValue)
        //    {
        //        ViewBag.ErrorMessage = "You have reached the camera limit for your current package.";
        //        return View();
        //    }

        //    // Prepare form-urlencoded data
        //    var formData = new Dictionary<string, string>
        //    {
        //        { "user_id", userId },
        //        { "camera_name", cameraName },
        //        { "camera_url", cameraIPAddress }
        //    };

        //    // Create form-urlencoded content
        //    var content = new FormUrlEncodedContent(formData);

        //    using (var client = new HttpClient())
        //    {
        //        client.BaseAddress = new Uri(_baseUrl);

        //        try
        //        {
        //            var response = await client.PostAsync("v1/camera/add_camera", content);

        //            if (response.IsSuccessStatusCode)
        //            {
        //                return RedirectToAction("Index", "UserCamera");
        //            }
        //            else
        //            {
        //                ViewBag.ErrorMessage = "Error adding camera. Please try again later.";
        //                return View();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            ViewBag.ErrorMessage = "An error occurred: " + ex.Message;
        //            return View();
        //        }
        //    }
        //}
        [HttpPost]
        public async Task<IActionResult> EditCamera(Camera camera)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var existingCamera = _context.Cameras.FirstOrDefault(c => c.Id == camera.Id && c.AccountId == currentUser.Id);
            if (existingCamera != null)
            {
                existingCamera.Name = camera.Name;
                existingCamera.IpAddress = camera.IpAddress;
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Camera updated successfully.";
                return RedirectToAction("Index");
            }
            TempData["FailMessage"] = "Failed to update camera.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCamera(int id)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var camera = _context.Cameras.FirstOrDefault(c => c.Id == id && c.AccountId == currentUser.Id);
            if (camera != null)
            {
                _context.Cameras.Remove(camera);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Camera deleted successfully.";
                return RedirectToAction("Index");
            }
            TempData["FailMessage"] = "Failed to delete camera.";
            return RedirectToAction("Index");
        }


        public IActionResult UploadImage([FromBody] CaptureRequest request)
        {
            var fileName = $"image_{DateTime.Now.Ticks}.png";
            var filePath = Path.Combine(_environment.WebRootPath, fileName);
            var data = Convert.FromBase64String(request.ImageData.Split(',')[1]);
            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                using (var bw = new BinaryWriter(fs))
                {
                    bw.Write(data);
                }
            }
            var image = new CaptureImage
            {
                FileName = fileName,
                FilePath = filePath
            };

            _context.CaptureImages.Add(image);
            _context.SaveChanges();

            return Json(new { success = true, imagePath = filePath });
        }
        private UserPackage GetCurrentActivePackage(string userId)
        {
            return _context.UserPackages
                .Where(up => up.UserId == userId && up.EndDate >= DateTime.Now)
                .OrderByDescending(up => up.EndDate)
                .FirstOrDefault();
        }

    }
}
public class CaptureRequest
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string ImageData { get; set; }
}