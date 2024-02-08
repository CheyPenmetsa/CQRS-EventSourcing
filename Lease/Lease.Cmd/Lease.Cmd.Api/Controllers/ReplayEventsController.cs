using CQRS.Core.Infrastructure;
using Lease.Cmd.Api.Commands;
using Lease.Cmd.Api.DTOs;
using Lease.Common.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Lease.Cmd.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ReplayEventsController  : ControllerBase
    {
        private readonly ILogger<ReplayEventsController> _logger;

        private readonly ICommandDispatcher _commandDispatcher;

        public ReplayEventsController(ILogger<ReplayEventsController> logger,
            ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost("{aggregateId}")]
        public async Task<ActionResult> ReplayEventsAsync([FromRoute] Guid aggregateId)
        {
            try
            {
                var command = new ReplayEventsCommand()
                {
                    Id = aggregateId
                };
                await _commandDispatcher.SendCommandAsync(command);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, "Error while replaying events");

                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse
                {
                    Message = "Error while replaying events"
                });
            }
        }
    }
}
