using System.ComponentModel.DataAnnotations;

namespace MaestroDetalle_CRUD.Models
{
    public class Cliente
    {
        public int ClienteId { get; set; }
        [Required]
        public string Nombre { get; set; } = null!;
    }
}