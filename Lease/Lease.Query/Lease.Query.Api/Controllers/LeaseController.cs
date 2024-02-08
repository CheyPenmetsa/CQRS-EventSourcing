using CQRS.Core.Infrastructure;
using Lease.Common.DTOs;
using Lease.Query.Api.DTOs;
using Lease.Query.Api.Queries;
using Lease.Query.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Lease.Query.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class LeaseController : ControllerBase
    {
        private readonly ILogger<LeaseController> _logger;

        private readonly IQueryDispatcher<LeaseEntity> _queryDispatcher;

        public LeaseController(IQueryDispatcher<LeaseEntity> queryDispatcher, 
            ILogger<LeaseController> logger)
        {
            _queryDispatcher = queryDispatcher;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllPostsAsync()
        {
            try
            {
                var leases = await _queryDispatcher.SendQueryAsync(new FindAllLeasesQuery());

                if (leases == null || !leases.Any())
                    return NoContent();

                return Ok(new LeaseDetailsResponse
                {
                    Leases = leases
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception when querying leases");

                return StatusCode(StatusCodes.Status500InternalServerError, new QueryResponse
                {
                    Message = "Exception when querying leases"
                });
            }
        }

        [HttpGet("{leaseId}")]
        public async Task<ActionResult> GetByLeaseIdAsync(Guid leaseId)
        {
            try
            {
                var leases = await _queryDispatcher.SendQueryAsync(new FindLeaseByIdQuery { LeaseId = leaseId });

                if (leases == null || !leases.Any())
                    return NoContent();

                return Ok(new LeaseDetailsResponse
                {
                    Leases = leases
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception when querying leases");

                return StatusCode(StatusCodes.Status500InternalServerError, new QueryResponse
                {
                    Message = "Exception when querying leases"
                });
            }
        }
    }
}
