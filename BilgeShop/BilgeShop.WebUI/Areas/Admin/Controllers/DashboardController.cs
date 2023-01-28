using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BilgeShop.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")] // Claim kısmındaki claims.Add(new Claim(ClaimTypes.Role, user.UserType.ToString())); kısmı ile bağlantılı. (AuthController) - Rolü admin olmayan kullanıcıların buradaki metotlara istek atmasını engeller.
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
