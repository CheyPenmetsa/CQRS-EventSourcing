using CQRS.Core.Domain;
using Lease.Common.Events;

namespace Lease.Cmd.Domain.Aggregates
{
    public class LeaseAggregate : AggregateRoot
    {
        private bool _isCancelled;

        private readonly Dictionary<string, bool> _customerSignatures = new();

        public bool IsCancelled
        {
            get => _isCancelled; 
            set
            {
                _isCancelled = value;
            }
        }

        public LeaseAggregate()
        {

        }

        // Id is leaseId 
        public LeaseAggregate(Guid id, 
            List<string> customerEmails, 
            string floorPlanName, 
            DateTime leaseStartDate, 
            int leaseTermLengthInMonths,
            double rentAmount,
            string apartmentNumber,
            string parkingSpace)
        {
            if(customerEmails == null || !customerEmails.Any())
            {
                throw new ArgumentNullException($"New to provide atleast one customer email");
            }

            if (string.IsNullOrWhiteSpace(floorPlanName))
            {
                throw new ArgumentNullException("Need to provide a floor plan name");
            }

            if(leaseStartDate <= DateTime.UtcNow)
            {
                throw new ArgumentException("Leasestartdate should be in future");
            }

            if(leaseTermLengthInMonths <= 0)
            {
                throw new ArgumentException("Provide a valid lease term length");
            }

            if (rentAmount.Equals(default))
            {
                throw new ArgumentException("Provide a valid rent amount length");
            }

            if (string.IsNullOrWhiteSpace(apartmentNumber))
            {
                throw new ArgumentNullException("Need to provide a apartment number");
            }

            foreach (var customer in customerEmails)
            {
                _customerSignatures.Add(customer, false);
            }

            RaiseEvent(new LeaseCreatedEvent()
            {
                Id = id,
                CustomerEmails = customerEmails,
                FloorPlan = floorPlanName,
                LeaseStartDate = leaseStartDate,
                LeaseTermLengthInMonths = leaseTermLengthInMonths,
                RentAmount = rentAmount,
                ApartmentNumber = apartmentNumber,
                ParkingSpace = parkingSpace,
                LeaseCreatedDate = DateTime.UtcNow
            });
        }

        public void Apply(LeaseCreatedEvent @event)
        {
            _id = @event.Id;
            _isCancelled = false;
        }

        public void SendLease()
        {
            // Make sure lease is not cancelled, in case of cancelled lease we wont send the lease to the customers
            if (_isCancelled)
            {
                throw new InvalidOperationException("You cannot send a cancelled lease.");
            }

            RaiseEvent(new LeaseSentEvent()
            {
                Id = _id,
                LeaseSentDate = DateTime.UtcNow
            });
        }

        public void Apply(LeaseSentEvent @event)
        {
            _id = @event.Id;
        }

        public void SignLease(string signerEmail)
        {
            // Make sure lease is not cancelled, in case of cancelled lease we wont send the lease to the customers
            if (_isCancelled)
            {
                throw new InvalidOperationException("You cannot sign a cancelled lease.");
            }

            if (string.IsNullOrWhiteSpace(signerEmail))
            {
                throw new InvalidOperationException($"Signing customer email cannot be null.");
            }

            if (_customerSignatures.TryGetValue(signerEmail, out bool customerSignedLease))
            {
                throw new ArgumentOutOfRangeException($"{signerEmail} is not present in the list of customers associated to lease.");
            }

            if (customerSignedLease)
            {
                throw new InvalidOperationException($"{signerEmail} signature already recieved for this lease.");
            }

            // Setting status of the lease signed to true
            _customerSignatures[signerEmail] = true;

            RaiseEvent(new LeaseSignedEvent()
            {
                Id = _id,
                EmailAddress = signerEmail,
                LeaseSignedDate = DateTime.UtcNow
            });
        }

        public void Apply(LeaseSignedEvent @event)
        {
            _id = @event.Id;
        }

        public void CancelLease(string userCancelled)
        {
            // Make sure lease is not cancelled, in case of cancelled lease we wont send the lease to the customers
            if (_isCancelled)
            {
                throw new InvalidOperationException("You cannot cancel a already cancelled lease.");
            }

            if (string.IsNullOrWhiteSpace(userCancelled))
            {
                throw new InvalidOperationException($"Lease cancellation user is required for cancelling a lease.");
            }

            RaiseEvent(new LeaseCancelledEvent()
            {
                Id = _id,
                UserName = userCancelled,
                LeaseCancelledDate = DateTime.UtcNow
            });
        }

        public void Apply(LeaseCancelledEvent @event)
        {
            _id = @event.Id;
            _isCancelled = true;
        }

        public void EditLease(string apartmentNumber, string parkingSpace)
        {
            // Make sure lease is not cancelled, in case of cancelled lease we wont send the lease to the customers
            if (_isCancelled)
            {
                throw new InvalidOperationException("You cannot edit a cancelled lease.");
            }

            if (string.IsNullOrWhiteSpace(apartmentNumber))
            {
                throw new ArgumentNullException("Need to provide a apartment number");
            }

            RaiseEvent(new LeaseEditedEvent()
            {
                Id = _id,
                ApartmentNumber = apartmentNumber,
                ParkingSpace = parkingSpace,
                LeaseEditedDate = DateTime.UtcNow
            });
        }

        public void Apply(LeaseEditedEvent @event)
        {
            _id = @event.Id;
        }
    }
}
