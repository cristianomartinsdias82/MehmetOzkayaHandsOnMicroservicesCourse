using System;

namespace Ordering.Domain.Common
{
    public abstract class Entity<TKey>
    {
        public TKey Id { get; protected set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }

    public abstract class Entity : Entity<Guid> { }
}
