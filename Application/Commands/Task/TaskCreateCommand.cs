using Domain.Interface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Task
{
    public class TaskCreateCommand : INotification
    {
        public TaskCreateCommand(Guid id, string title, string description, string craetedBy)
        {
            this.Id = id;
            this.Title = title;
            this.Description = description;
            this.CreatedBy = craetedBy;
        }
        public Guid Id { get; }
        public string Title { get; }
        public string Description { get; }
        public string CreatedBy { get; }
    }

    public class TaskCreateCommandHandler : INotificationHandler<TaskCreateCommand>
    {
        private readonly IEventService<Domain.Models.Task, Guid> _eventService;

        public TaskCreateCommandHandler(IEventService<Domain.Models.Task, Guid> eventService)
        {
            _eventService = eventService;
        }

        public async System.Threading.Tasks.Task Handle(TaskCreateCommand notification, CancellationToken cancellationToken)
        {
            var aggragate = await _eventService.RehydrateAsync(notification.Id);

            aggragate.CreateTask(notification.Title, notification.Description, notification.CreatedBy);

            await _eventService.PersistAsync(aggragate);
        }
    }
}
