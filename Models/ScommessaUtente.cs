namespace SalaScommesse2_0.Models
{
    public class ScommessaUtente
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public int ScommessaId { get; set; }
        public decimal Importo { get; set; }
        public bool Vinta { get; set; }
        public bool Pagata { get; set; }
        public DateTime Data { get; set; }
    }
}