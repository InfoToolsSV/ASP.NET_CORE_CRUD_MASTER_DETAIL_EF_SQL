using System.ComponentModel.DataAnnotations;

namespace MaestroDetalle_CRUD.Models
{
    public class Producto
    {
        public int ProductoId { get; set; }

        [Required] 
        public string Nombre { get; set; } = null!;
        [Required] 
        public decimal Precio { get; set; }
    }
}