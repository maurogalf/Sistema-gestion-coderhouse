using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sistema_gestion_coderhouse.Models;
using Sistema_gestion_coderhouse.Repositories;

namespace Sistema_gestion_coderhouse.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private OrderRepository repository = new OrderRepository();
        [HttpPost]
        // CARGAR VENTA
        public ActionResult Post([FromBody] Order order)
        {
            try
            {
                repository.createOrder(order);
                return Ok();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        [HttpGet]
        // TRAE VENTA
        public IActionResult Get()
        {
            try
            {
                List<Order> list = repository.listOrders(null);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        [HttpGet("{id}")]
        // TRAE VENTA POR ID
        public IActionResult Get(int id)
        {
            try
            {
                List<Order> list = repository.listOrders(id);
                return Ok(list);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        [HttpDelete("{id}")]
        // ELIMINAR VENTA
        public ActionResult Delete(int id)
        {
            try
            {
                int deletedRows = repository.deleteOrder(id);
                if (deletedRows > 0)
                {
                    return Ok($"Se elimino correctamente la venta con ID:{id} y {deletedRows} lineas de la tabla ProductoVendido.");
                }
                else
                {
                    return NotFound($"No se encontraron ventas para el ID:{id}.");
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
