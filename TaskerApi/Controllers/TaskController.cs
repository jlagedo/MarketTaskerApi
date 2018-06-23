using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskerApi.dto;
using TaskerApi.Model;

namespace TaskerApi.Controllers
{
    [Route("api/[controller]")]
    public class TaskController : Controller
    {
        private readonly ITaskItemRepository _taskItemRepository;

        public TaskController(ITaskItemRepository taskItemRepository)
        {
            _taskItemRepository = taskItemRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// <remarks>
        /// testing model validation https://tahirnaushad.com/2017/08/22/asp-net-core-2-0-mvc-model-validation/
        /// maybe
        /// http://www.talkingdotnet.com/validate-model-state-automatically-asp-net-core-2-0/
        /// 
        /// 2.1 upgrade ?
        /// https://blogs.msdn.microsoft.com/webdev/2018/02/02/asp-net-core-2-1-roadmap/
        /// [ApiController]
        /// </remarks>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(TaskDTO))]
        public async Task<IActionResult> Post([FromBody] NewTaskDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var task = new TaskItem
            {
                RegisterDate = DateTime.Now,
                Text = dto.Text,
                Title = dto.Title
            };

            await _taskItemRepository.AddAsync(task);

            return CreatedAtAction(nameof(Get), new { id = task.Id }, dto);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var documents = await _taskItemRepository.GetAll();

            var ret = new List<TaskDTO>();

            foreach (var item in documents)
            {
                ret.Add(new TaskDTO
                {
                    Id = item.Id,
                    RegisterDate = item.RegisterDate,
                    Text = item.Text,
                    Title = item.Title
                });
            }

            return this.Json(ret);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            TaskItem item = await _taskItemRepository.Get(id);

            var dto = new TaskDTO
            {
                Id = item.Id,
                RegisterDate = item.RegisterDate,
                Text = item.Text,
                Title = item.Title
            };

            return Json(dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(string id)
        {
            var isDeleted = await _taskItemRepository.Delete(id);

            if (isDeleted)
                return Ok();
            else
                return NotFound();
        }



    }
}