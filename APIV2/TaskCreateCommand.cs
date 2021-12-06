using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace APIV2
{
    public class TaskCreateCommand : IRequest<bool>
    {
        public string Name { get; set; }
        public class TaskCreateCommandHandler : IRequestHandler<TaskCreateCommand, bool>
        {
            public Task<bool> Handle(TaskCreateCommand request, CancellationToken cancellationToken)
            {
                return Task.FromResult(true);
            }
        }
    }
}
