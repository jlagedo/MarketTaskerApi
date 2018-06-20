using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskerApi.Model;

namespace TaskerApi.Cross
{
    public class TaskItemRepository : ITaskItemRepository
    {
        private readonly MongoContext context;
        public TaskItemRepository(MongoContext context)
        {
            this.context = context;
        }

        public async Task AddAsync(TaskItem task)
        {
            await context.Tasks.InsertOneAsync(task);
        }

        public async Task<TaskItem> Get(string id)
        {
            return await context.Tasks.Find(t => t.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<TaskItem>> GetAll()
        {
            return await context.Tasks.Find(new BsonDocument()).ToListAsync();
        }
    }
}
