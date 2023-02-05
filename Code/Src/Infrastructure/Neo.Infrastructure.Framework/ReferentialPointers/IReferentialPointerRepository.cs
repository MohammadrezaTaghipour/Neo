using Neo.Infrastructure.Framework.Domain;

namespace Neo.Infrastructure.Framework.ReferentialPointers;

public interface IReferentialPointerRepository : IDomainRepository
{
    Task Add(ReferentialPointerId id,ReferentialPointer referentialPoint,
        CancellationToken cancellationToken);
    Task<ReferentialPointer> GetById(ReferentialPointerId id, CancellationToken cancellationToken);
}
