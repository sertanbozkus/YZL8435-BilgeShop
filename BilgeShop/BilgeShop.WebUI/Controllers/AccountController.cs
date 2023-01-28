using BilgeShop.Business.Dtos;
using BilgeShop.Business.Services;
using BilgeShop.WebUI.Extensions;
using BilgeShop.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace BilgeShop.WebUI.Controllers
{
    public class AccountController : Controller
    {
        // UserManager bir class, bunun içerisinde metotlar var. Benim bu metotları kullanmam için, UserManager classını newlemem gerekiyor.
        // Newlemek yerine kullanabileceğim bir diğer yöntem ise Dependency Injection ile servisi burada tanımlamak.
        // Interface üzerinden bir servis tanımlayıp bunu constuctor injection'da yaratıyorum.
        // Artık _userService... diyerek , bana sunulan hizmetleri/metotları kullanabilirim.

        // Dependency Injection ile oluşturduğum servis, istek gönderildiğinde newlenip, istek bitiminde silinecek.
        // Eskiden using keyword kullanılırdı, artık gerek yok.

        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("Hesabim")]
        public IActionResult Index()
        {
            var viewModel = new AccountEditViewModel()
            {
                FirstName = User.GetFirstName(),
                LastName = User.GetLastName(),
                Email = User.GetEmail(),
                EmailConfirm = User.GetEmail()
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Update(AccountEditViewModel formData)
        {
            if(!ModelState.IsValid)
            {
                return View("Index", formData);
            }

            var userEditDto = new UserEditDto()
            {
                Id = User.GetId(),
                FirstName = formData.FirstName,
                LastName = formData.LastName,
                Email = formData.Email
            };



            _userService.UpdateUser(userEditDto);



            return RedirectToAction("Logout", "Auth");
            
            
            // TODO : Tamam çıkış yapıyorum ama , yeniden giriş de yapmalı ki kullanıcının hoş geldiniz .. ... gördüğü kısım güncellensin. Nasıl yaparım ? ÖDEV!

        }
    }
}
