using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntity
{
    public record BaseEntity: BaseDomainEvent
    {
        public Guid Id { get;  set; } = Guid.NewGuid();
    }
}
