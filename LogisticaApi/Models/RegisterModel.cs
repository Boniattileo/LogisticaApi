using System.ComponentModel.DataAnnotations;

namespace LogisticaApi.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; }
        [StringLength(100, MinimumLength = 8, ErrorMessage = "A senha deve ter entre 8 e 100 caracteres")]
        [Required(ErrorMessage = "Senha é obrigatória")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Role é obrigatório")]
        public UserRole Role { get; set; }
    }
}
