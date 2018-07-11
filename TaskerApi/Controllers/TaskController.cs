using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskerApi.dto;
using TaskerApi.Model;

namespace TaskerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskController : Controller
    {
        private string Userid => "3Bob3RvLmpwZyIsImdpdmVuX25hbWUiOiJKb8OjbyBBbWFybyIsImZh";

        private readonly ITaskItemRepository _taskItemRepository;

        public TaskController(ITaskItemRepository taskItemRepository)
        {
            _taskItemRepository = taskItemRepository;
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<ActionResult<TaskDTO>> Post(NewTaskDTO dto)
        {
            var task = new TaskItem
            {
                RegisterDate = DateTime.Now,
                Text = dto.Text,
                Title = dto.Title,
                UserId = Userid
            };

            await _taskItemRepository.AddAsync(task);

            var taskDTO = new TaskDTO
            {
                Id = task.Id,
                RegisterDate = task.RegisterDate,
                Text = task.Text,
                Title = task.Title
            };

            return CreatedAtAction(nameof(Get), new {id = task.Id}, taskDTO);
        }

        [HttpGet]
        public async Task<IEnumerable<TaskDTO>> Index()
        {


            var tasks = await _taskItemRepository.GetAllFromUserAsync(Userid);

            var returnList = tasks.Select(t => new TaskDTO
            {
                Id = t.Id,
                RegisterDate = t.RegisterDate,
                Text = t.Text,
                Title = t.Title
            });

            return returnList;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TaskDTO>> Get(string id)
        {
            TaskItem task = await _taskItemRepository.Get(id);

            if (task == null)
                return NotFound();


            return new TaskDTO
            {
                Id = task.Id,
                RegisterDate = task.RegisterDate,
                Text = task.Text,
                Title = task.Title
            };
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Remove(string id)
        {
            var isDeleted = await _taskItemRepository.Delete(id);

            if (isDeleted)
                return Ok();
            return NotFound();
        }
    }
}