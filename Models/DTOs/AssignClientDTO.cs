using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace Cw6.Models.DTOs
{
    public class AssignClientDTO
    {
        [Required]
        [MaxLength(120)]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        [MaxLength(120)]
        public string LastName { get; set; } = string.Empty;
        [Required]
        [MaxLength(120)]
        public string Email { get; set; } = string.Empty;
        [Required]
        [MaxLength(120)]
        public string Telephone { get; set; } = string.Empty;
        [Required]
        [MaxLength(120)]
        public string Pesel { get; set; } = string.Empty;
        [Required]
        public int IdTrip { get; set; }
        [Required]
        [MaxLength(120)]
        public string TripName { get; set; } = string.Empty;
        [MaxLength(120)]
        public string PaymentDate { get; set; } = string.Empty;
    }
}
