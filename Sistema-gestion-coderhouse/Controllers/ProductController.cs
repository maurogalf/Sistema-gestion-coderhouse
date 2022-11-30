using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sistema_gestion_coderhouse.Models;
using Sistema_gestion_coderhouse.Repositories;

namespace Sistema_gestion_coderhouse.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private ProductRepository repository = new ProductRepository();
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                List<Product> products = repository.listProducts();
                return Ok(products);

            }
            catch(Exception ex)
            {
                return Problem(ex.Message);
            }
        }

    }
}
