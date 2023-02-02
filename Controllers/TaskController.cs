using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;
using MyTask.Controllers;
using MyTask.Models;
using MyTask.services;
using MyTask.Interfaces;
using Task = MyTask.Models.Task;
using Microsoft.Net.Http.Headers;
using User.services;
using Microsoft.AspNetCore.Authorization;


namespace MyTask.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController : ControllerBase
    {
        private ITaskService TaskService;
        private int userId;

        public TaskController(ITaskService TaskService)
        {
            this.TaskService = TaskService;
        }


        [HttpGet]
        [Authorize(Policy = "User")]
        public ActionResult<List<Task>> Get()
        {
            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            return TaskService.GetTasksByUserId(TokenService.decode(token));
        }


        [HttpGet("{id}")]
        [Authorize(Policy = "User")]
        public ActionResult<Task> Get(int id)
        {
            var task = TaskService.Get(id);
            if (task == null)
                return NotFound();
            return task;
        }

        [HttpPost]
        [Authorize(Policy = "User")]
        public ActionResult Post(Task task)
        {
            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            task.UserId = TokenService.decode(token);
            TaskService.Add(task);
            return CreatedAtAction(nameof(Post), new { Id = task.Id }, task);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "User")]
        public ActionResult Put(int id, Task task)
        {
            var token = Request.Headers[HeaderNames.Authorization].ToString().Replace("Bearer ", "");
            task.UserId = TokenService.decode(token);
            if (id != task.Id)
                return BadRequest("id <> task.Id");

            var existingTask = TaskService.Get(id);
            if (existingTask is null)
                return NotFound();

            TaskService.Update(task);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "User")]
        public ActionResult Delete(int id)
        {
            var task = TaskService.Get(id);
            if (task == null)
                return NotFound();
            TaskService.Delete(id);
            return NoContent();
        }

    }
}