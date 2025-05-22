using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace SalaScommesse2_0.Models
{
    public class Utente
    {
        [Required(ErrorMessage = "Username obbligatorio")]
        [StringLength(20, MinimumLength = 3, ErrorMessage = "L'username deve essere tra 3 e 20 caratteri")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password obbligatoria")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "La password deve avere almeno 6 caratteri")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email obbligatoria")]
        [EmailAddress(ErrorMessage = "Inserisci un indirizzo email valido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Data di nascita obbligatoria")]
        [DataType(DataType.Date)]
        [MinimumAge(18, ErrorMessage = "Devi avere almeno 18 anni")]
        public DateTime DataNascita { get; set; }

        public decimal Saldo { get; set; }

        [JsonIgnore]
        public bool IsAuthenticated { get; set; }
    }
}