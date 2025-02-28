using System.ComponentModel.DataAnnotations;

namespace LogisticaApi.Models
{
    public class ApplicationUser
    {
        public string Id { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]        
        public UserRole Role  { get; set; }

    }
}
