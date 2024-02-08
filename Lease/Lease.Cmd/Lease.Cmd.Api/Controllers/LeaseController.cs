using CQRS.Core.Exceptions;
using CQRS.Core.Infrastructure;
using Lease.Cmd.Api.Commands;
using Lease.Cmd.Api.DTOs;
using Lease.Common.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Lease.Cmd.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class LeaseController : ControllerBase
    {
        private readonly ILogger<LeaseController> _logger;

        private readonly ICommandDispatcher _commandDispatcher;

        public LeaseController(ILogger<LeaseController> logger, 
            ICommandDispatcher commandDispatcher)
        {
            _logger = logger;
            _commandDispatcher = commandDispatcher;
        }

        [HttpPost("/create")]
        public async Task<ActionResult> CreateLeaseAsync([FromBody]CreateLeaseCommand command)
        {
            var leaseId = Guid.NewGuid();
            try
            {
                command.Id = leaseId;
                await _commandDispatcher.SendCommandAsync(command);

                return StatusCode(StatusCodes.Status201Created, new CreateLeaseCommandResponse
                {
                    Id = leaseId,
                    Message = "Lease Created successfully!"
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.Log(LogLevel.Warning, ex, "Bad request!");
                return BadRequest(new CommandResponse
                {
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, "Error while creating lease");

                return StatusCode(StatusCodes.Status500InternalServerError, new CreateLeaseCommandResponse
                {
                    Id = leaseId,
                    Message = "Error while creating lease"
                });
            }
        }

        [HttpPut("/send/{leaseId}")]
        public async Task<ActionResult> SendLeaseAsync([FromRoute]Guid leaseId)
        {
            try
            {
                var command = new SendLeaseCommand()
                {
                    Id = leaseId
                };
                await _commandDispatcher.SendCommandAsync(command);

                return Ok(new CommandResponse
                {
                    Message = "Lease Sent successfully!"
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.Log(LogLevel.Warning, ex, "Bad request!");
                return BadRequest(new CommandResponse
                {
                    Message = ex.Message
                });
            }
            catch (AggregateNotFoundException ex)
            {
                _logger.Log(LogLevel.Warning, ex, $"Could not retrieve aggregate for leaseId:{leaseId}");
                return BadRequest(new CommandResponse
                {
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, "Error while sending lease");

                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse
                {
                    Message = "Error while sending lease"
                });
            }
        }

        [HttpPut("/sign/{leaseId}/{customerEmail}")]
        public async Task<ActionResult> SignLeaseAsync([FromRoute] Guid leaseId,
            [FromRoute] string customerEmail)
        {
            try
            {
                var command = new SignLeaseCommand()
                {
                    Id = leaseId,
                    EmailAddress = customerEmail
                };
                await _commandDispatcher.SendCommandAsync(command);

                return Ok(new CommandResponse
                {
                    Message = "Lease/Customer updated successfully!"
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.Log(LogLevel.Warning, ex, "Bad request!");
                return BadRequest(new CommandResponse
                {
                    Message = ex.Message
                });
            }
            catch (ArgumentOutOfRangeException ex)
            {
                _logger.Log(LogLevel.Warning, ex, "Bad request!");
                return BadRequest(new CommandResponse
                {
                    Message = ex.Message
                });
            }
            catch (AggregateNotFoundException ex)
            {
                _logger.Log(LogLevel.Warning, ex, $"Could not retrieve aggregate for leaseId:{leaseId}");
                return BadRequest(new CommandResponse
                {
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, "Error while updating lease/customer for signing");

                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse
                {
                    Message = "Error while updating lease/customer for signing"
                });
            }
        }

        [HttpPut("/changeApartment/{leaseId}/{apartmentNumber}")]
        public async Task<ActionResult> ChangeApartmentAsync([FromRoute] Guid leaseId,
            [FromRoute] string apartmentNumber)
        {
            try
            {
                var command = new EditLeaseCommand()
                {
                    Id = leaseId,
                    ApartmentNumber = apartmentNumber,
                    ParkingSpace = string.Empty
                };
                await _commandDispatcher.SendCommandAsync(command);

                return Ok(new CommandResponse
                {
                    Message = "Lease updated successfully!"
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.Log(LogLevel.Warning, ex, "Bad request!");
                return BadRequest(new CommandResponse
                {
                    Message = ex.Message
                });
            }
            catch (ArgumentNullException ex)
            {
                _logger.Log(LogLevel.Warning, ex, "Bad request!");
                return BadRequest(new CommandResponse
                {
                    Message = ex.Message
                });
            }
            catch (AggregateNotFoundException ex)
            {
                _logger.Log(LogLevel.Warning, ex, $"Could not retrieve aggregate for leaseId:{leaseId}");
                return BadRequest(new CommandResponse
                {
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, "Error while updating lease");

                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse
                {
                    Message = "Error while updating lease"
                });
            }
        }

        [HttpPut("/cancel/{leaseId}/{userName}")]
        public async Task<ActionResult> CancelLeaseAsync([FromRoute] Guid leaseId,
            [FromRoute]string userName)
        {
            try
            {
                var command = new CancelLeaseCommand()
                {
                    Id = leaseId,
                    UserName = userName
                };
                await _commandDispatcher.SendCommandAsync(command);

                return Ok(new CommandResponse
                {
                    Message = "Lease cancelled successfully!"
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.Log(LogLevel.Warning, ex, "Bad request!");
                return BadRequest(new CommandResponse
                {
                    Message = ex.Message
                });
            }
            catch (AggregateNotFoundException ex)
            {
                _logger.Log(LogLevel.Warning, ex, $"Could not retrieve aggregate for leaseId:{leaseId}");
                return BadRequest(new CommandResponse
                {
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex, "Error while canceling lease");

                return StatusCode(StatusCodes.Status500InternalServerError, new CommandResponse
                {
                    Message = "Error while canceling lease"
                });
            }
        }
    }
}
