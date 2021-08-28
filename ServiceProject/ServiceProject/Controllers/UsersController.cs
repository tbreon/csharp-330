using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ServiceProject.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ServiceProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private static int currentId = 101;
        public static List<User> users = new List<User>();

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var user = users.FirstOrDefault(t => t.Id == id);

            if (user == null)
            {
                return new NotFoundResult();
            }
            else
            {
                return Ok(user);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] User value)
        {
            if (value == null)
            {
                return new BadRequestResult();
            }

            if (value.Name == null)
            {
                return BadRequest(new ErrorResponse { Message = "Name field is null", DBCode = 120, Data = value });
            }

            value.Id = currentId++;
            value.DateAdded = DateTime.Now;
            users.Add(value);

            return CreatedAtAction(nameof(Get), new { id = value.Id }, value);
        }


        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] User value)
        {
            var user = users.FirstOrDefault(t => t.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            //contact = value;
            user.Name = value.Name;
            user.Email = value.Email;
            user.Password = value.Password;

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {

            var usersDeleted = users.RemoveAll(t => t.Id == id);

            if (usersDeleted == 0)
            {
                return NotFound();
            }
            else
            {
                return Ok();
            }
        }
    }
}

