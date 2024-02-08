using CQRS.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lease.Query.Domain.Entities
{
    [Table("Lease")]
    public class LeaseEntity : BaseEntity
    {
        [Key]
        public Guid LeaseId { get; set; }

        public string FloorPlan { get; set; }

        public DateTime LeaseStartDate { get; set; }

        public int LeaseTermLengthInMonths { get; set; }

        public double RentAmount { get; set; }

        public string ParkingSpace { get; set; }

        public string ApartmentNumber { get; set; }

        public bool Cancelled { get; set; }

        public virtual ICollection<CustomerEntity> Residents { get; set; }
    }
}
