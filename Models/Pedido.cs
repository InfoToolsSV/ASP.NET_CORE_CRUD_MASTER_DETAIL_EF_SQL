using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MaestroDetalle_CRUD.Models
{
    public class Pedido
    {

        public Pedido()
        {
            Detalles=new List<PedidoDetalle>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PedidoId { get; set; }
        public DateTime Fecha { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; } = null!;
        public List<PedidoDetalle> Detalles { get; set; } = null!;
    }
}