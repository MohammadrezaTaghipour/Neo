﻿using Neo.Specs.ScreenPlay.LifeStreams.Commands;
using Neo.Specs.ScreenPlay.LifeStreams.Questions;
using Suzianna.Core.Screenplay;
using Suzianna.Core.Screenplay.Actors;
using Suzianna.Rest.Screenplay.Interactions;
using Suzianna.Rest.Screenplay.Questions;

namespace Neo.Specs.ScreenPlay.LifeStreams.Tasks;

public class ModifyLifeStreamByApiTask : ITask
{
    private readonly ModifyLifeStreamCommand _command;

    public ModifyLifeStreamByApiTask(ModifyLifeStreamCommand command)
    {
        _command = command;
    }

    public void PerformAs<T>(T actor) where T : Actor
    {
        actor.AttemptsTo(Put.DataAsJson(_command)
            .To($"/api/LifeStreams/{_command.Id}"));

        if (!actor.Recall<LastResponseException>().HasException())
        {
            var status = actor.AsksFor(
                new GetLifeStreamByIdQuestion(_command.Id)).Status;
            if (status.Completed)
                return;
        }
    }
}