using System;
using System.Collections.Generic;
using Neo.Infrastructure.Framework.Application;

namespace Neo.Application.Contracts.StreamEventTypes;

public class ModifyStreamEventTypeCommand : ICommand
{
    private ModifyStreamEventTypeCommand()
    {
        Metada = new List<StreamEventTypeMetadaCommandItem>();
    }

    public Guid CorrelationId { get; set; }
    public Guid Id { get; set; }
    public string Title { get; set; }
    public IReadOnlyList<StreamEventTypeMetadaCommandItem> Metada { get; set; }
    public long Version { get; set; }
}