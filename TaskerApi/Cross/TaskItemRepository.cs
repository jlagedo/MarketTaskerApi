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

        public async Task<bool> Delete(string id)
        {
            var result =  await context.Tasks.DeleteOneAsync(d => d.Id == id);
            return result.IsAcknowledged && result.DeletedCount > 0;
        }

        public async Task<List<TaskItem>> GetAllFromUserAsync(string userid)
        {
            var list = await context.Tasks.FindAsync(t => t.UserId == userid);
            return await list.ToListAsync();
        }
    }
}
