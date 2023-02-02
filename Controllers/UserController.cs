using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using User.Interfaces;
using user = User.Models.user;
using User.Models;
using System.Security.Claims;
using User.services;
using Microsoft.AspNetCore.Authorization;
using MyTask.services;
using Microsoft.Net.Http.Headers;

namespace User.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private IUserService UserService;
        public UserController(IUserService UserService)
        {
            this.UserService = UserService;
        }

        [HttpPost]
        [Route("[action]")]
        public ActionResult<String> Login([FromBody] user user)
        {
            List<user> users = UserService.GetAll();

            user u = users.FirstOrDefault(p => p.Name == user.Name && p.Password == user.Password);
            if (u == null)
            {
                return Unauthorized();
            }

            var claims = new List<Claim>();
            if (u.IsAdmin == true)
                claims.Add(new Claim("type", "Admin"));
            if (u.IsAdmin == false)
                claims.Add(new Claim("type", "User"));
            claims.Add(new Claim("Id", u.Id.ToString()));


            var token = TokenService.GetToken(claims);
            return new OkObjectResult(TokenService.WriteToken(token));
        }
        [HttpGet]
        [Authorize(Policy = "Admin")]
        public ActionResult<IEnumerable<user>> Get() =>
            UserService.GetAll();


        [HttpGet("{id}")]
        [Authorize(Policy = "User")]
        public ActionResult<user> Get(int id)
        {
            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            int userId = TokenService.decode(token);
            var u = UserService.Get(userId);
            if (u == null)
                return NotFound();
            return u;
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public ActionResult Post(user u)
        {

            UserService.Add(u);
            return CreatedAtAction(nameof(Post), new { Id = u.Id }, u);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "Admin")]
        public ActionResult Delete(int id)
        {
            var u = UserService.Get(id);
            if (u == null)
                return NotFound();
            UserService.Delete(id);
            return NoContent();
        }

    }
}