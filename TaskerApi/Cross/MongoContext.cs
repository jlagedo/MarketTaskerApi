using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskerApi.Model;

namespace TaskerApi.Cross
{
    public class MongoContext
    {
        private readonly IMongoDatabase database;

        public MongoContext(IOptions<Settings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<TaskItem> Tasks
        {
            get
            {
                return database.GetCollection<TaskItem>("taskitem");
            }
        }
    }
}
