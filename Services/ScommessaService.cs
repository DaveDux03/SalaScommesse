using SalaScommesse2_0.Models;
using System.Text.Json;
using System.IO;

namespace SalaScommesse2_0.Services
{
    public class ScommessaService
    {
        private readonly string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "scommesse.json");
        private List<Scommessa> _scommesse;

        public ScommessaService()
        {
            _scommesse = new List<Scommessa>();
            CaricaScommesse();
        }

        private void CaricaScommesse()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                _scommesse = JsonSerializer.Deserialize<List<Scommessa>>(json) ?? new List<Scommessa>();
            }
        }

        private void SalvaScommesse()
        {
            var directory = Path.GetDirectoryName(_filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var json = JsonSerializer.Serialize(_scommesse);
            File.WriteAllText(_filePath, json);
        }

        public List<Scommessa> GetScommesseDisponibili()
        {
            return _scommesse.Where(s => !s.Chiusa && s.DataEvento > DateTime.Now).ToList();
        }

        public Scommessa GetScommessa(int id)
        {
            return _scommesse.FirstOrDefault(s => s.Id == id);
        }

        public void AggiornaScommessa(Scommessa scommessa)
        {
            var index = _scommesse.FindIndex(s => s.Id == scommessa.Id);
            if (index >= 0)
            {
                _scommesse[index] = scommessa;
                SalvaScommesse();
            }
        }
    }
}