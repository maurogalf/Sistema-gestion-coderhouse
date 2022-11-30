using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sistema_gestion_coderhouse.Models;
using Sistema_gestion_coderhouse.Repositories;

namespace Sistema_gestion_coderhouse.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SoldProductController : Controller
    {
        private SoldProductRepository repository = new SoldProductRepository();
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                List<SoldProduct> list = repository.listSoldProducts();
                return Ok(list);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
