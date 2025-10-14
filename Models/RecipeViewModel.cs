using System.ComponentModel.DataAnnotations;

namespace CamCook.Models
{
    public class RecipeViewModel
    {
        [Required]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required, StringLength(120)]
        public string Titulo { get; set; } = string.Empty;

        [Range(0, 5000)]
        public int Calorias { get; set; }

        [Range(1, 50)]
        public int Porciones { get; set; }

        [Display(Name = "Tiempo de preparación (min)")]
        public string? TiempoPreparacion { get; set; }

        // Imagen final (opcional)
        public string? ImagenFinalUrl { get; set; }

        public List<Ingredient> Ingredientes { get; set; } = new();
        public List<Step> Pasos { get; set; } = new();
    }

    public class Ingredient
    {
        public string Nombre { get; set; } = string.Empty;
        public string Cantidad { get; set; } = string.Empty; // libre: "1", "1/2", "una pizca"
        public string Unidad { get; set; } = string.Empty;
    }

    public class Step
    {
        public int Orden { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public string? ImagenUrl { get; set; }
    }
}
