using Google.Cloud.Firestore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LogisticaApi.Models
{
    [FirestoreData]
    public class Product
    {
        [Required(ErrorMessage = "ID é obrigatório")]
        [FirestoreProperty]
        public int Id { get; set; }

        [Required(ErrorMessage = "Largura é obrigatória")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Largura deve ser maior que 0")]
        [FirestoreProperty]
        public double Width { get; set; }

        [Required(ErrorMessage = "Altura é obrigatória")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Altura deve ser maior que 0")]
        [FirestoreProperty]
        public double Height { get; set; }

        [Required(ErrorMessage = "Comprimento é obrigatório")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Comprimento deve ser maior que 0")]
        [FirestoreProperty]
        public double Length { get; set; }

        [Required(ErrorMessage = "Peso é obrigatório")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Peso deve ser maior que 0")]
        [FirestoreProperty]
        public double Weight { get; set; }

        [JsonConstructor]
        public Product() { }
    }
}
