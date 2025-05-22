using System;
using System.ComponentModel.DataAnnotations;

namespace SalaScommesse2_0.Models
{
    public class Scommessa
    {
        public int Id { get; set; }

        [Required]
        public string Descrizione { get; set; }

        [Required]
        public decimal Quota { get; set; }

        [Required]
        public DateTime DataEvento { get; set; }

        public string Categoria { get; set; } // Es: "Calcio", "Tennis", etc.
        public bool Esito { get; set; } // true = vinta, false = persa
        public bool Chiusa { get; set; } // true = evento concluso
    }
}