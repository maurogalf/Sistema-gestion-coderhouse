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
        [HttpDelete]
        // ELIMINAR VENTA
        public ActionResult Delete([FromBody]int id)
        {
            try
            {
                bool deletedRows = repository.deleteOrder(id);
                if (deletedRows)
                {
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
