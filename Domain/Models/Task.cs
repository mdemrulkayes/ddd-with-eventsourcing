using Domain.Entities;
using Domain.Events.Task;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Task : BaseAggregateRoot<Task, Guid>
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string AssignedTo { get; private set; } = string.Empty;
        public bool IsCompleted { get; private set; } = false;
        public string CreatedBy { get; private set; } = string.Empty;
        private Task() { }
        public Task(Guid id, string title, string description, string createdBy) : base(id)
        {
            if (Version > 0)
            {
                throw new Exception("Task Already Created");
            }
            this.Title = title;
            this.Description = description;
            this.CreatedBy = createdBy;

            this.AddEvent(new CreatedTask(this));
        }
        protected override void Apply(IDomainEvent<Guid> @event)
        {
            switch (@event)
            {
                case CreatedTask createdTask:
                    OnCreatedTask(createdTask);
                    break;
            }
        }

        private void OnCreatedTask(CreatedTask createdTask)
        {
            Id = createdTask.AggegateId;
            Title = createdTask.Title;
            Description = createdTask.Description;
            CreatedBy = createdTask.CreatedBy;
        }
    }
}
