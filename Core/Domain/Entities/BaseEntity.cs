using System;

namespace Domain.Entities
{
    public class BaseEntity
    {
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public BaseEntity()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
