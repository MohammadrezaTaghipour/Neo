﻿namespace Neo.Infrastructure.Framework.Projections;

public interface IDominEventProjectorDispatcher
{
    Task Dispatch<T>(T eventToProject, CancellationToken cancellationToken);
}