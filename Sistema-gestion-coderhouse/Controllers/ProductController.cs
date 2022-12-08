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
        public ActionResult Get()
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
        [HttpGet("{id}")]
        public ActionResult<Product> Get(int id)
        {
            try
            {
                Product? product = repository.getProductById(id);
                if(product != null)
                {
                    return Ok(product);
                }
                else
                {
                    return NotFound("Product Not Found.");
                }

            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult Post([FromBody] Product product)
        {
            try
            {
                repository.createProduct(product);
                return Ok();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        [HttpDelete]
        public ActionResult Delete([FromBody] int id)
        {
            try
            {
                bool deletedRows = repository.deleteProduct(id);
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
