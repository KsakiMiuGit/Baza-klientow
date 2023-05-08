using System.ComponentModel.DataAnnotations;

namespace Klienci.Models
{
    public class Klient
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Imie { get; set; }

        [Required]
        [StringLength(50)]
        public string Nazwisko { get; set; }

        [Required]
        [StringLength(20)]
        public string NrTelefonu { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }
    }
}
