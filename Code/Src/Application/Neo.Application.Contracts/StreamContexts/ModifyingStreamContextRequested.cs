﻿using Neo.Infrastructure.Framework.Application;

namespace Neo.Application.Contracts.StreamContexts;

public class ModifyingStreamContextRequested : BaseRequest
{
    public ModifyingStreamContextRequested()
    {
        StreamEventTypes = new List<StreamEventTypeRequestItem>();
    }
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public IReadOnlyCollection<StreamEventTypeRequestItem> StreamEventTypes { get; set; }
    public long Version { get; set; }
}
