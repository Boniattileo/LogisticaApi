using System.ComponentModel.DataAnnotations;

namespace LogisticaApi.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Token é obrigatório")]
        public string IdToken { get; set; }        
    }
}
