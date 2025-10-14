using CamCook.Models;
using Microsoft.AspNetCore.Mvc;

namespace CamCook.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Login() => View(new LoginViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // DEMO: reemplaza por tu autenticación real
            if (model.Username == "admin" && model.Password == "1234")
                return RedirectToAction("Index", "Home");

            ModelState.AddModelError(string.Empty, "Usuario o contraseña incorrectos.");
            return View(model);
        }
        [HttpGet]
        public IActionResult Register() => View(new RegisterViewModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Aquí deberías guardar en DB
            TempData["Message"] = "Usuario registrado correctamente. Ahora inicia sesión.";
            return RedirectToAction("Login");
        }
    }
}
