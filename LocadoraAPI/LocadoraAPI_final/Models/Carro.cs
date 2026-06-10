using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LocadoraAPI.Models
{
    public class Carro
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Modelo é obrigatório.")]
        public string Modelo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Placa é obrigatória.")]
        public string Placa { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue, ErrorMessage = "Valor da diária deve ser maior que zero.")]
        public double ValorDiaria { get; set; }

        public bool Disponivel { get; set; } = true;

        // Navegação
        [JsonIgnore]
        public List<Locacao> Locacoes { get; set; } = new();
    }
}
