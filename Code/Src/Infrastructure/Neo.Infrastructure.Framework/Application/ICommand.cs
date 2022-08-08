
using System;

namespace Neo.Infrastructure.Framework.Application;

public interface ICommand
{
    Guid CorrelationId { get; }
}
