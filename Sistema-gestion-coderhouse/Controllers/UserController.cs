﻿using Microsoft.AspNetCore.Mvc;
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
        public IActionResult Get()
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
    }
}