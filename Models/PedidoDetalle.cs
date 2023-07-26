using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace MaestroDetalle_CRUD.Models
{
    public class PedidoDetalle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PedidoDetalleId { get; set; }

        [Required]
        public int Cantidad { get; set; }
        [Required]
        public int PedidoId { get; set; }
        public Pedido Pedido { get; set; } = null!;
        [Required]
        public int ProductoId { get; set; }
        public Producto Producto { get; set; } = null!;
    }
}