using Microsoft.AspNetCore.Mvc;
using SalaScommesse2_0.Models;
using SalaScommesse2_0.Services;

namespace SalaScommesse2_0.Controllers
{
    public class ScommesseController : Controller
    {
        private readonly ScommessaService _scommessaService;
        private readonly UtenteService _utenteService;

        public ScommesseController(ScommessaService scommessaService, UtenteService utenteService)
        {
            _scommessaService = scommessaService;
            _utenteService = utenteService;
        }

        public IActionResult Index()
        {
            var scommesse = _scommessaService.GetScommesseDisponibili();
            return View(scommesse);
        }

        [HttpPost]
        public IActionResult Piazzascommessa(int scommessaId, decimal importo)
        {
            var username = HttpContext.Session.GetString("_Username");
            if (string.IsNullOrEmpty(username))
            {
                return RedirectToAction("Login", "Home");
            }

            if (_utenteService.Piazzascommessa(username, scommessaId, importo))
            {
                TempData["SuccessMessage"] = "Scommessa piazzata con successo!";
            }
            else
            {
                TempData["ErrorMessage"] = "Errore nel piazzare la scommessa. Verifica il saldo.";
            }

            return RedirectToAction("Index");
        }

        public IActionResult Live()
        {
            // Filtra solo le scommesse in corso (data evento tra 1 ora fa e 2 ore dopo)
            var fromDate = DateTime.Now.AddHours(-1);
            var toDate = DateTime.Now.AddHours(2);

            var scommesseLive = _scommessaService.GetScommesseDisponibili()
                .Where(s => s.DataEvento >= fromDate && s.DataEvento <= toDate)
                .ToList();

            return View(scommesseLive);
        }
    }
}