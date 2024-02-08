using Lease.Common.Events;
using Lease.Query.Domain.Entities;
using Lease.Query.Domain.Repositories;

namespace Lease.Query.Infrastructure.Handlers
{
    public class EventConsumerHandler : IEventConsumerHandler
    {
        private readonly ILeaseRepository _leaseRepository;

        private readonly ICustomerRepository _customerRepository;

        public EventConsumerHandler(ILeaseRepository leaseRepository, 
            ICustomerRepository customerRepository)
        {
            _leaseRepository = leaseRepository;
            _customerRepository = customerRepository;
        }

        public async Task On(LeaseCreatedEvent @event)
        {
            var lease = new LeaseEntity()
            {
                LeaseId = @event.Id,
                FloorPlan = @event.FloorPlan,
                ApartmentNumber = @event.ApartmentNumber,
                ParkingSpace = string.IsNullOrWhiteSpace(@event.ParkingSpace) ? String.Empty : @event.ParkingSpace,
                Cancelled = false,
                LeaseStartDate = @event.LeaseStartDate,
                LeaseTermLengthInMonths = @event.LeaseTermLengthInMonths,
                RentAmount = @event.RentAmount
            };
            await _leaseRepository.CreateAsync(lease);

            foreach (var customerEmail in @event.CustomerEmails)
            {
                var resident = new CustomerEntity()
                {
                    CustomerId = Guid.NewGuid(),
                    EmailAddress = customerEmail,
                    LeaseId = @event.Id
                };
                await _customerRepository.CreateAsync(resident);
            }
        }

        // Since we are not tracking anything when lease sent just returning task.completed
        public async Task On(LeaseSentEvent @event)
        {
            await Task.CompletedTask.ConfigureAwait(false) ;
        }

        public async Task On(LeaseSignedEvent @event)
        {
            var residents = await _customerRepository.GetCustomersByLeaseIdAsync(@event.Id);

            if (residents == null || !residents.Any(x => x.EmailAddress.Equals(@event.EmailAddress)))
                return;

            var resident = residents.First(x => x.EmailAddress.Equals(@event.EmailAddress));
            resident.LeaseSignedDate = @event.LeaseSignedDate;
            await _customerRepository.UpdateAsync(resident);
        }

        public async Task On(LeaseEditedEvent @event)
        {
            var lease = await _leaseRepository.GetByIdAsync(@event.Id);

            if (lease == null)
                return;

            lease.ApartmentNumber = @event.ApartmentNumber;
            lease.ParkingSpace = string.IsNullOrWhiteSpace(@event.ParkingSpace) ? String.Empty : @event.ParkingSpace;
            await _leaseRepository.UpdateAsync(lease);
        }

        public async Task On(LeaseCancelledEvent @event)
        {
            var lease = await _leaseRepository.GetByIdAsync(@event.Id);

            if (lease == null)
                return;

            lease.Cancelled = true;
            await _leaseRepository.UpdateAsync(lease);
        }
    }
}
