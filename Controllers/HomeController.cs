using Microsoft.AspNetCore.Mvc;
using SalaScommesse2_0.Models;
using SalaScommesse2_0.Services;

namespace SalaScommesse2_0.Controllers
{
    public class HomeController : Controller
    {
        private readonly UtenteService _utenteService;
        private const string SessionKeyUsername = "_Username";

        public HomeController(UtenteService utenteService)
        {
            _utenteService = utenteService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Registrazione()
        {
            return View(new Utente());
        }

        [HttpPost]
        [HttpPost]
        public IActionResult Registrazione(Utente utente, string PasswordConfirm)
        {
            if (ModelState.IsValid)
            {
                if (utente.Password != PasswordConfirm)
                {
                    ModelState.AddModelError("", "Le password non corrispondono");
                    return View(utente);
                }

                if (_utenteService.RegistraUtente(utente))
                {
                    return RedirectToAction("Login");
                }
                ModelState.AddModelError("", "Username o email già esistenti o utente minorenne");
            }
            return View(utente);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var utente = _utenteService.Login(username, password);
            if (utente != null)
            {
                HttpContext.Session.SetString(SessionKeyUsername, username);
                return RedirectToAction("Index");
            }
            ModelState.AddModelError("", "Credenziali non valide");
            return View();
        }

        [HttpGet]
        public IActionResult Deposito()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyUsername)))
            {
                return RedirectToAction("Login");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Deposito(decimal importo)
        {
            var username = HttpContext.Session.GetString(SessionKeyUsername);
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login");
            }

            if (_utenteService.Deposita(username, importo))
            {
                ViewBag.SuccessMessage = $"Deposito di {importo}€ effettuato con successo!";
            }
            else
            {
                ViewBag.ErrorMessage = "Errore durante il deposito. Verifica l'importo.";
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove(SessionKeyUsername);
            return RedirectToAction("Index");
        }
    }
}