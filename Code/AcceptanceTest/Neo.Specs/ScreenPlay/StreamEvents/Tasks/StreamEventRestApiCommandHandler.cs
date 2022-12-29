﻿using Neo.Specs.Framework;
using Neo.Specs.ScreenPlay.StreamEvents.Commands;
using Suzianna.Core.Screenplay;

namespace Neo.Specs.ScreenPlay.StreamEvents.Tasks;

public class StreamEventRestApiCommandHandler :
    ICommandHandler<AppendStreamEventCommand>,
    ICommandHandler<RemoveStreamEventCommand>
{
    public ITask Handle(AppendStreamEventCommand command)
        => new AppendStreamEventByApiTask(command);

    public ITask Handle(RemoveStreamEventCommand command)
    {
        throw new NotImplementedException();
    }
}