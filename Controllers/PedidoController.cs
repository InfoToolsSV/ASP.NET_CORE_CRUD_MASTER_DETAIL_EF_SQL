using MaestroDetalle_CRUD.Data;
using MaestroDetalle_CRUD.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MaestroDetalle_CRUD.Controllers
{
    public class PedidoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PedidoController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ActionResult> Index()
        {
            var pedidos = await _context.Pedidos
            .Include(p => p.Cliente)
            .ToListAsync();

            return View(pedidos);
        }

        public async Task<ActionResult> Create()
        {
            ViewBag.Clientes = await _context.Clientes.ToListAsync();
            ViewBag.Productos = await _context.Productos.ToListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Pedido pedido, int[] productoIds, int[] cantidades)
        {
            var cli = await _context.Clientes.FirstOrDefaultAsync(m => m.ClienteId == pedido.ClienteId);

            if (cli != null)
                pedido.Cliente = cli;

            foreach (var item in productoIds)
            {
                var producto = await _context.Productos.FindAsync(item);
                if (producto != null)
                {
                    pedido.Detalles.Add(new PedidoDetalle
                    {
                        ProductoId = item,
                        Cantidad = cantidades[Array.IndexOf(productoIds, item)],
                        Producto = producto
                    });
                }
            }
            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<ActionResult> Details(int id)
        {
            var pedido = await _context.Pedidos
            .Include(p => p.Detalles)
                .ThenInclude(d => d.Producto)
            .Include(p => p.Cliente)
            .FirstOrDefaultAsync(p => p.PedidoId == id);

            if (pedido == null)
            {
                return NotFound();
            }
            return View(pedido);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
                return NotFound();

            ViewBag.Clientes = _context.Clientes.ToList();
            return View(pedido);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Pedido pedido)
        {
            if (id != pedido.PedidoId)
                return NotFound();

            _context.Update(pedido);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var pedido = await _context.Pedidos
            .Include(p => p.Cliente)
            .FirstOrDefaultAsync(p => p.PedidoId == id);

            if (pedido == null)
                return NotFound();

            var detallesPedido = await _context.PedidoDetalles
            .Include(d => d.Producto)
                .Where(d => d.PedidoId == id)
                .ToListAsync();

            ViewBag.DetallesPedido = detallesPedido;

            return View(pedido);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
            {
                return NotFound();
            }

            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult EditDetalle(int id)
        {
            var detalle = _context.PedidoDetalles.Find(id);
            if (detalle == null)
                return NotFound();

            var producto = _context.Productos.FirstOrDefault(p => p.ProductoId == detalle.ProductoId);
            if (producto == null)
                return NotFound();

            ViewBag.ProductoPrecio = producto.Precio;

            ViewBag.Productos = _context.Productos.ToList();

            return View(detalle);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDetalle(int id, PedidoDetalle detalle)
        {
            var existingDetalle = _context.PedidoDetalles
            .Include(d => d.Pedido)
            .FirstOrDefault(d => d.PedidoDetalleId == id);

            if (existingDetalle == null)
                return NotFound();

            if (id != detalle.PedidoDetalleId)
                return NotFound();

            existingDetalle.Cantidad = detalle.Cantidad;
            existingDetalle.ProductoId=detalle.ProductoId;

            _context.Update(existingDetalle);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = existingDetalle.PedidoId });
        }

        public async Task<IActionResult> DeleteDetalle(int id)
        {
            var detalle = await _context.PedidoDetalles
            .Include(d => d.Producto)
            .FirstOrDefaultAsync(d => d.PedidoDetalleId == id);

            if (detalle == null)
                return NotFound();

            return View(detalle);
        }

        [HttpPost, ActionName("DeleteDetalle")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDetalleConfirmed(int id)
        {
            var detall = await _context.PedidoDetalles.FindAsync(id);
            if (detall == null)
                return NotFound();

            _context.PedidoDetalles.Remove(detall);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = detall.PedidoId });
        }

    }
}