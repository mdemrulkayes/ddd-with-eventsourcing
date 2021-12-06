using Domain.Interface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.Task
{
    public class TaskCreateCommand : IRequest<int>
    {
        public TaskCreateCommand(Guid id, string title, string description, string craetedBy)
        {
            this.Id = id;
            this.Title = title;
            this.Description = description;
            this.CreatedBy = craetedBy;
        }
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
    }

    public class TaskCreateCommandHandler : IRequestHandler<TaskCreateCommand, int>
    {
        private readonly IEventService<Domain.Models.Task, Guid> _eventService;

        public TaskCreateCommandHandler(IEventService<Domain.Models.Task, Guid> eventService)
        {
            _eventService = eventService;
        }

        public async Task<int> Handle(TaskCreateCommand request, CancellationToken cancellationToken)
        {
            var task = new Domain.Models.Task(request.Id, request.Title, request.Description, request.CreatedBy);

            await _eventService.PersistAsync(task);
            return 1;
        }
    }
}


//using MediatR;
//using System.Threading;
//using System.Threading.Tasks;

//namespace Application.Commands.Task
//{
//    public class TaskCreateCommand : IRequest<bool>
//    {
//        public string Name { get; set; }
//        public class TaskCreateCommandHandler : IRequestHandler<TaskCreateCommand, bool>
//        {
//            public Task<bool> Handle(TaskCreateCommand request, CancellationToken cancellationToken)
//            {
//                return System.Threading.Tasks.Task.FromResult(true);
//            }
//        }
//    }
//}
