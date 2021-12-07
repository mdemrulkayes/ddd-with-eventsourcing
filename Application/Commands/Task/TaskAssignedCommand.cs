using Domain.Interface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Task
{
    public class TaskAssignedCommand : IRequest<int>
    {
        private TaskAssignedCommand() { }
        public TaskAssignedCommand(Guid id, string assignedTo)
        {
            this.Id = id;
            this.AssignedTo = assignedTo;
        }
        public Guid Id { get; }
        public string AssignedTo { get; }

        public class TaskAssignedCommandHandler : IRequestHandler<TaskAssignedCommand, int>
        {
            private readonly IEventService<Domain.Models.Task, Guid> _eventService;

            public TaskAssignedCommandHandler(IEventService<Domain.Models.Task, Guid> eventService)
            {
                _eventService = eventService;
            }

            public async Task<int> Handle(TaskAssignedCommand request, CancellationToken cancellationToken)
            {
                var data = await _eventService.RehydrateAsync(request.Id);
                if (data == null)
                    throw new Exception("Invalid event Id");

                data.AssignTask(request.AssignedTo);
                await _eventService.PersistAsync(data);
                return 1;
            }
        }
    }
}
