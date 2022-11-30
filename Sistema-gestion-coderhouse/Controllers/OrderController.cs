﻿using Microsoft.AspNetCore.Http;
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
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                List<Order> list = repository.listOrders();
                return Ok(list);
            }
            catch(Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        
    }
}
