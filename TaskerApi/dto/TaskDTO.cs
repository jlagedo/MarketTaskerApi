﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskerApi.dto
{
    public class TaskDTO
    {
        public string Id { get; set; }
        public DateTime RegisterDate { get; set; }
        public string Text { get; set; }
        public string Title { get; set; }

    }
}
