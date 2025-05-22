using Microsoft.AspNetCore.Mvc;
using SalaScommesse2_0.Services;

namespace SalaScommesse2_0.Components
{
    public class SaldoUtenteViewComponent : ViewComponent
    {
        private readonly UtenteService _utenteService;

        public SaldoUtenteViewComponent(UtenteService utenteService)
        {
            _utenteService = utenteService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var username = HttpContext.Session.GetString("_Username");
            if (string.IsNullOrEmpty(username))
                return Content(string.Empty);

            var utente = _utenteService.GetUtente(username);
            var saldoFormattato = (utente?.Saldo ?? 0).ToString("C", System.Globalization.CultureInfo.CreateSpecificCulture("it-IT"));
            return Content($"Saldo: {saldoFormattato}");
        }
    }
}