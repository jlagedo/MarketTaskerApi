using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskerApi.Model
{
    public class TaskItem
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public DateTime RegisterDate { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }

        public TaskItem()
        {

        }
    }
}
