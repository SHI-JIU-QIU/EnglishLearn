using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class BaseDbContext : DbContext
    {
        private IMediator? mediator;
        public BaseDbContext(DbContextOptions options, IMediator? mediator) : base(options)
        {
            this.mediator = mediator;
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            if (mediator != null)
            {
                await mediator.DispatchDomainEventsAsync(this);
            }

            var softDeletedEntities = this.ChangeTracker.Entries<ISoftDelete>()
               .Where(e => e.State == EntityState.Modified && e.Entity.IsDeleted)
               .Select(e => e.Entity).ToList();

            var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
            softDeletedEntities.ForEach(e => this.Entry(e).State = EntityState.Detached);

            return result;
        }
    }



}
