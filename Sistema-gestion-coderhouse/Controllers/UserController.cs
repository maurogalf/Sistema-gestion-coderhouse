using Microsoft.AspNetCore.Mvc;
using Sistema_gestion_coderhouse.Models;
using Sistema_gestion_coderhouse.Repositories;

namespace Sistema_gestion_coderhouse.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private UserRepository repository = new UserRepository();
        [HttpGet]
        // TRAE TODOS LOS USUARIOS
        public ActionResult Get()
        {
            try
            {
                List<User> list = repository.listUsers();
                return Ok(list);

            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        [HttpGet("{username}")]
        // TRAER USUARIO
        public ActionResult<User> Get(string username)
        {
            try
            {
                User? user= repository.getUserByUserName(username);
                if (user != null)
                {
                    return Ok(user);
                }
                else
                {
                    return NotFound("User does not exist.");
                }

            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("{username}/{password}")]
        // INICIO DE SESION
        public ActionResult<User> Get(string username, string password)
        {
            try
            {
                User? user = repository.getUserByUserName(username);
                if (user != null)
                {
                    bool logged = repository.login(username, password);
                    if (logged)
                    {
                        return Ok(user);
                    }
                    else
                    {
                        return Unauthorized("Invalid password.");
                    }
                }
                else
                {
                    return NotFound("User does not exist.");
                }

            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        [HttpDelete]
        //ELIMINAR USUARIO
        public ActionResult Delete([FromBody] int id)
        {
            try
            {
                bool deletedRows = repository.deleteUser(id);
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
        [HttpPost]
        // CREAR USUARIO
        public ActionResult Post([FromBody] User user)
        {
            try
            {
                repository.createUser(user);
                return Ok();
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        [HttpPut("{id}")]
        // MODIFICAR USUARIO
        public ActionResult<User> Put(int id, [FromBody] User updatedUser)
        {
            try
            {
                User user = repository.updateUser(id, updatedUser);
                if (user != null)
                {
                    return Ok(user);
                }
                else
                {
                    return NotFound($"User with id {id} does not exist.");
                }
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }

        }
    }
}
