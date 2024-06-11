using BusinessObjects;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Text;

namespace CamOn_FE.Controllers
{
    public class CameraController : Controller
    {
        private readonly string _baseUrl;
        private IWebHostEnvironment _environment;
        private readonly AppDbContext _context;
        private readonly UserManager<Account> _userManager;

        public CameraController(IWebHostEnvironment environment,AppDbContext context,
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
        public async Task<ActionResult> AddCamera(string cameraName, string cameraIPAddress)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var userId = currentUser.Id;

            // Prepare form-urlencoded data
            var formData = new Dictionary<string, string>
            {
                { "user_id", userId },
                { "camera_name", cameraName },
                { "camera_url", cameraIPAddress }
            };

            // Create form-urlencoded content
            var content = new FormUrlEncodedContent(formData);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);

                try
                {
                    var response = await client.PostAsync("v1/camera/add_camera", content);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index", "Camera");
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "Error adding camera. Please try again later.";
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

        public async Task<IActionResult> Stream(string cameraId)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_baseUrl);
                var requestUrl = $"v1/camera/stream?camera_id={cameraId}";
                try
                {
                    var response = await client.GetAsync("http://127.0.0.1:8000/v1/camera/stream?camera_id=5");

                    if (response.IsSuccessStatusCode)
                    {
                        var stream = await response.Content.ReadAsStreamAsync();
                        return new FileStreamResult(stream, "multipart/x-mixed-replace; boundary=frame");
                    }

                    return StatusCode((int)response.StatusCode);
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "An error occurred: " + ex.Message;
                    return View();
                }
            }
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
    }
}
public class CaptureRequest
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string ImageData { get; set; }
}