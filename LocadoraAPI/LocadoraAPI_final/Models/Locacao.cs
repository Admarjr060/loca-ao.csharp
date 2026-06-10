using System.ComponentModel.DataAnnotations;

namespace LocadoraAPI.Models
{
    public class Locacao
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nome do cliente é obrigatório.")]
        public string Cliente { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "Quantidade de dias deve ser maior que zero.")]
        public int Dias { get; set; }

        public double Total { get; set; }

        // Chave estrangeira
        public int CarroId { get; set; }

        // Navegação
        public Carro? Carro { get; set; }

        // Regra de negócio: desconto de 10% para mais de 7 dias
        public void CalcularTotal()
        {
            if (Carro == null) return;

            Total = Dias * Carro.ValorDiaria;

            if (Dias > 7)
            {
                Total *= 0.9;
            }
        }
    }
}
