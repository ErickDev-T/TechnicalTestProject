using System.ComponentModel.DataAnnotations;

namespace ProjectAPIStore.Models
{
    public class Sales
    {

        public int Id { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required, StringLength(120)]
        public string Cliente { get; set; } = "";

        [Required] // guarda la lista de productos como texto 
        public string ListaProductos { get; set; } = "";

        public decimal Total { get; set; }

    }
}



