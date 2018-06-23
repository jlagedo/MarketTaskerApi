using System.Collections.Generic;
using System.Threading.Tasks;

namespace TaskerApi.Model
{
    public interface ITaskItemRepository
    {
        Task<List<TaskItem>> GetAll();
        Task AddAsync(TaskItem task);
        Task<TaskItem> Get(string id);
        Task<bool> Delete(string id);
    }
}
