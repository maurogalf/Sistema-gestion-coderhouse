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
        // TRAER TODOS LOS PRODUCTOS
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
        // TRAER PRODUCTOS POR ID
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
        // CREAR PRODUCTO
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
        [HttpDelete("{id}")]
        // ELIMINAR PRODUCTO
        public ActionResult Delete(int id)
        {
            try
            {
                bool deletedRows = repository.deleteProduct(id);
                if (deletedRows)
                {
                    return Ok("Product deleted succesfully.");
                }
                else
                {
                    return NotFound($"Product with ID {id} does not exist.");
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        [HttpPut("{id}")]
        // ACTUALIZAR PRODUCTO
        public ActionResult<Product> Put(int id, [FromBody] Product updatedProduct)
        {
            try
            {
                Product? product = repository.updateProduct(id, updatedProduct);
                if (product != null)
                {
                    return Ok(product);
                }
                else
                {
                    return NotFound($"Product with id {id} does not exist.");
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
