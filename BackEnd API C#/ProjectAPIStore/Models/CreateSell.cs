namespace ProjectAPIStore.Models
{
    public class CreateSell
    {
        public string Cliente { get; set; } = "";
        public List<ItemDto> Items { get; set; } = new();
        public class ItemDto
        {
            public int ProductoId { get; set; }
            public int Cantidad { get; set; }
            public decimal? PrecioUnitario { get; set; } // null => usar precio de Productos
        }
    }
}