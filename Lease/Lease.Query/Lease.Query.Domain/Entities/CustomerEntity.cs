using CQRS.Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Lease.Query.Domain.Entities
{
    [Table("Customer")]
    public class CustomerEntity : BaseEntity
    {
        [Key]
        public Guid CustomerId { get; set; }

        public string EmailAddress { get; set; }

        public DateTime LeaseSignedDate { get; set; }

        public Guid LeaseId { get; set; }

        [JsonIgnore]
        public virtual LeaseEntity Lease { get; set; }
    }
}
