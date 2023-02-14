using Neo.Domain.Contracts.ReferentialPointers;
using Neo.Infrastructure.Framework.Domain;

namespace Neo.Domain.Models.ReferentialPointers;

public interface IReferentialPointerRepository : IDomainRepository
{
    Task Add(ReferentialPointerId id, ReferentialPointer referentialPoint,
        CancellationToken cancellationToken);
    Task<ReferentialPointer> GetById(ReferentialPointerId id, CancellationToken cancellationToken);
}
