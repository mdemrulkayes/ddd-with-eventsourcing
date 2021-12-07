using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events.Task
{
    public class AssignedTask : BaseDomainEvent<Models.Task, Guid>
    {
        public string AssignedTo { get; private set; }
        public AssignedTask()
        {

        }
        public AssignedTask(Models.Task assignedTask, string assignedTo) : base(assignedTask)
        {
            this.AssignedTo = assignedTo;
        }
    }
}
