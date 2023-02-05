﻿using Neo.Infrastructure.Framework.Domain;

namespace Neo.Infrastructure.Framework.ReferentialPointers;

public class ReferentialPointerRemoved : DomainEvent
{
    public ReferentialPointerId Id { get; }
    public string PointerType { get; set; }
    
    public ReferentialPointerRemoved(ReferentialPointerId id,
        string pointerType)
    {
        Id = id;
        PointerType = pointerType;
    }
}