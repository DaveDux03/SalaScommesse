using SalaScommesse2_0.Models;
using System.Text.Json;
using System.IO;

namespace SalaScommesse2_0.Services
{
    public class UtenteService
    {
        private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "utenti.json");
        private List<Utente> _utenti;

        public UtenteService()
        {
            _utenti = new List<Utente>();
            CaricaUtenti();
        }

        private void CaricaUtenti()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                _utenti = JsonSerializer.Deserialize<List<Utente>>(json) ?? new List<Utente>();
            }
        }

        private void SalvaUtenti()
        {
            var directory = Path.GetDirectoryName(_filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var json = JsonSerializer.Serialize(_utenti);
            File.WriteAllText(_filePath, json);
        }

        public bool RegistraUtente(Utente nuovoUtente)
        {
            // Verifica se l'utente è maggiorenne
            if (DateTime.Today.AddYears(-18) < nuovoUtente.DataNascita)
            {
                return false;
            }

            // Verifica se username o email già esistenti
            if (_utenti.Any(u => u.Username == nuovoUtente.Username ||
                                u.Email == nuovoUtente.Email))
            {
                return false;
            }

            // Imposta il saldo iniziale
            nuovoUtente.Saldo = 0;
            _utenti.Add(nuovoUtente);
            SalvaUtenti();
            return true;
        }

        public Utente? Login(string username, string password)
        {
            var utente = _utenti.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (utente != null)
            {
                utente.IsAuthenticated = true;
                return utente;
            }
            return null;
        }

        public bool Deposita(string username, decimal importo)
        {
            var utente = _utenti.FirstOrDefault(u => u.Username == username);
            if (utente == null || importo <= 0)
            {
                return false;
            }

            utente.Saldo += importo;
            SalvaUtenti();
            return true;
        }

        private readonly string _scommesseUtentePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "scommesseUtente.json");

        public bool Piazzascommessa(string username, int scommessaId, decimal importo)
        {
            if (importo <= 0) return false;

            var utente = _utenti.FirstOrDefault(u => u.Username == username);
            if (utente == null || utente.Saldo < importo) return false;

            utente.Saldo -= importo;

            var scommesseUtente = CaricaScommesseUtente();
            scommesseUtente.Add(new ScommessaUtente
            {
                Id = scommesseUtente.Count + 1,
                Username = username,
                ScommessaId = scommessaId,
                Importo = importo,
                Vinta = false,
                Pagata = false,
                Data = DateTime.Now
            });

            SalvaScommesseUtente(scommesseUtente);
            SalvaUtenti();
            return true;
        }

        private List<ScommessaUtente> CaricaScommesseUtente()
        {
            if (File.Exists(_scommesseUtentePath))
            {
                var json = File.ReadAllText(_scommesseUtentePath);
                return JsonSerializer.Deserialize<List<ScommessaUtente>>(json) ?? new List<ScommessaUtente>();
            }
            return new List<ScommessaUtente>();
        }

        private void SalvaScommesseUtente(List<ScommessaUtente> scommesse)
        {
            var directory = Path.GetDirectoryName(_scommesseUtentePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var json = JsonSerializer.Serialize(scommesse);
            File.WriteAllText(_scommesseUtentePath, json);
        }

        public Utente? GetUtente(string username)
        {
            return _utenti.FirstOrDefault(u => u.Username == username);
        }

    }
}