using Neo.Domain.Contracts.ReferentialPointers;
using Neo.Domain.Models.ReferentialPointers;
using Neo.Infrastructure.Framework.Persistence;

namespace Neo.Infrastructure.Persistence.ES;

public class ReferentialPointerRepository : IReferentialPointerRepository
{
    private readonly IEventSourcedRepository<ReferentialPointer,
            ReferentialPointerState, ReferentialPointerId> _repository;

    public ReferentialPointerRepository(
        IEventSourcedRepository<ReferentialPointer,
            ReferentialPointerState, ReferentialPointerId> repository)
    {
        _repository = repository;
    }

    public async Task Add(ReferentialPointerId id,
        ReferentialPointer referentialPointer,
        CancellationToken cancellationToken)
          => await _repository.Add(GetStreamName(id),
                referentialPointer, cancellationToken)
            .ConfigureAwait(false);

    public async Task<ReferentialPointer> GetById(ReferentialPointerId id,
        CancellationToken cancellationToken)
        => await _repository.GetById(GetStreamName(id), id, cancellationToken)
            .ConfigureAwait(false);

    static StreamName GetStreamName(ReferentialPointerId id) => StreamName
        .For<ReferentialPointer, ReferentialPointerState, ReferentialPointerId>(id);
}