using BilgeShop.Business.Dtos;
using BilgeShop.Business.Services;
using BilgeShop.WebUI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BilgeShop.WebUI.Controllers
{
    // Authentication ve Authorization İşlemleri
    // (Kimlik Doğrulama ve Yetkilendirme)
    public class AuthController : Controller
    {
        private readonly IUserService _userService;
        public AuthController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("kayit-ol")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("kayit-ol")]
        public IActionResult Register(RegisterViewModel formData)
        {
            if(!ModelState.IsValid)
            {
                return View(formData); // formData'yı geri göndermezseniz, açılan view boş gelecek, yani kullanıcı inputlara doldurduğu bütün verileri tekrardan girmek zorunda kalacak.
            }

            //Kullanıcı kayıt

            var addUserDto = new AddUserDto()
            {
                FirstName = formData.FirstName.Trim(),
                LastName = formData.LastName.Trim(),
                Email = formData.Email.Trim(),
                Password = formData.Password.Trim()
            };

           var response = _userService.AddUser(addUserDto);

            if (response.IsSucceed)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ErrorMessage = response.Message;
                return View(formData);
            }

            
        }


        public async Task<IActionResult> Login(LoginViewModel formData)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index" , "Home"); 
            }

            var loginDto = new LoginDto()
            {
                Email = formData.Email,
                Password = formData.Password
            };

            var userDto = _userService.Login(loginDto);

            if(userDto is null)
            {
                // RedirectToAction ile geriye bir action dönüyorsam , veriyi viewbag ile değil , tempdata ile taşırım.

                TempData["LoginMessage"] = "Kullanıcı adı veya şifreyi hatalı girdiniz.";
                
                return RedirectToAction("index", "home");
            }

            // Oturum Açma

            // Cookie(çerez) -> tarayıcıda saklanan dosya.
            // Claim -> Cookie'deki her bir bilgi.

            var claims = new List<Claim>();

            claims.Add(new Claim("id", userDto.Id.ToString()));
            claims.Add(new Claim("email", userDto.Email));
            claims.Add(new Claim("firstName", userDto.FirstName));
            claims.Add(new Claim("lastName", userDto.LastName));
            claims.Add(new Claim("userType", userDto.UserType.ToString()));

            claims.Add(new Claim(ClaimTypes.Role, userDto.UserType.ToString()));

            // claims.add(new Claim("password", userDto.password)); BU ÇOK BÜYÜK BİR GÜVENLİK AÇIĞIDIR. PASSWORD KESİNLİKLE VE KESİNLİKLE CLAIM VEYA BAŞKA BİR YERDE (view üzeri) TUTULMAZ.

            var claimIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            // bu bilgilerle oturum oluştur ^

            var autProperties = new AuthenticationProperties
            {
                AllowRefresh = true, //  sayfa yenilenince oturum açık kalsın
                ExpiresUtc = new DateTimeOffset(DateTime.Now.AddHours(48)) //oturum 48 saat açık kalsın.
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity), autProperties);
            // oluşturulan bilgi dosyaları ve özelliklerle oturumu aç ^

            return RedirectToAction("index", "home");
        }
        // await keywordü ile çalışacaksanız ( yapılar birbirini beklesin. ) - metodunuzu async Task<..> olarak tanımlamanız gerekir.

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(); // oturumu kapat.

            return RedirectToAction("Index", "Home"); // ana sayfaya gönder.
        }
    }
}



