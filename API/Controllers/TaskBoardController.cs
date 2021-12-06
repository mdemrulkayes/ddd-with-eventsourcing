using API.DTOs;
using Application.Commands.Task;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskBoardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TaskBoardController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto task)
        {
            var command = new TaskCreateCommand(Guid.NewGuid(), task.Title, task.Description, task.CreatedBy);
            await _mediator.Send(command);
            return Ok(command);
        }

        [HttpPatch]
        public async Task<IActionResult> CreateTask(Guid id, [FromBody] AssignTaskDto task)
        {
            var command = new TaskAssignedCommand(id, task.AssignedTo);
            await _mediator.Send(command);
            return Ok(command);
        }
    }
}
