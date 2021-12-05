using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events.Task
{
    public class CreatedTask : BaseDomainEvent<Domain.Models.Task, Guid>
    {
        private CreatedTask()
        {

        }

        public CreatedTask(Models.Task createdTask) : base(createdTask)
        {
            Title = createdTask.Title;
            CreatedBy = createdTask.CreatedBy;
            Description = createdTask.Description;
        }

        public string Title { get; private set; }
        public string CreatedBy { get; private set; }
        public string Description { get; private set; }
    }
}
