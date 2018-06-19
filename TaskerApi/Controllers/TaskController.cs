using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using TaskerApi.dto;
using TaskerApi.Model;

namespace TaskerApi.Controllers
{
    [Route("api/[controller]")]
    public class TaskController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NewTaskDTO dto)
        {

            var task = new TaskItem
            {
                RegisterDate = DateTime.Now,
                Text = dto.Text,
                Title = dto.Title
            };            

            var client = new MongoClient("mongodb://localhost:32768");
            var database = client.GetDatabase("tasker");

            var collection = database.GetCollection<TaskItem>("taskitem");

           await collection.InsertOneAsync(task);            

            return CreatedAtAction(nameof(Get), new { id = "asd" }, dto);
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var client = new MongoClient("mongodb://localhost:32768");
            var database = client.GetDatabase("tasker");

            var collection = database.GetCollection<TaskItem>("taskitem");

            var documents = await collection.Find(new BsonDocument()).ToListAsync();

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
        public IActionResult Get(string id)
        {
            var itens = new List<TaskItem>
            {
                new TaskItem
                {
                    RegisterDate = DateTime.Now,
                    Text = "Example Test"
                }
            };

            return this.Json(itens);
        }
    }
}