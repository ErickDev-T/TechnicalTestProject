using System.ComponentModel.DataAnnotations;

namespace ProjectAPIStore.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required, StringLength(120)]  // no null or empty and max 120 charactered
        public string Nombre { get; set; } = "";

        [StringLength(255)]
        public string? Descripcion { get; set; }

        public decimal Precio { get; set; }

        public int Stock { get; set; }
    }
}

