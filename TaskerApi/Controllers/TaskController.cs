using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskerApi.dto;
using TaskerApi.Model;

namespace TaskerApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : Controller
    {
        private readonly ITaskItemRepository _taskItemRepository;

        public TaskController(ITaskItemRepository taskItemRepository)
        {
            _taskItemRepository = taskItemRepository;
        }

        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<ActionResult<TaskDTO>> Post(NewTaskDTO dto)
        {
            var task = new TaskItem
            {
                RegisterDate = DateTime.Now,
                Text = dto.Text,
                Title = dto.Title
            };

            await _taskItemRepository.AddAsync(task);

            var taskDTO = new TaskDTO
            {
                Id = task.Id,
                RegisterDate = task.RegisterDate,
                Text = task.Text,
                Title = task.Title
            };

            return CreatedAtAction(nameof(Get), new { id = task.Id }, taskDTO);
        }

        [HttpGet]
        public async Task<IEnumerable<TaskDTO>> Index()
        {
            var tasks = await _taskItemRepository.GetAll();

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

            var taskDTO = new TaskDTO
            {
                Id = task.Id,
                RegisterDate = task.RegisterDate,
                Text = task.Text,
                Title = task.Title
            };

            return taskDTO;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Remove(string id)
        {
            var isDeleted = await _taskItemRepository.Delete(id);

            if (isDeleted)
                return Ok();
            else
                return NotFound();
        }
    }
}