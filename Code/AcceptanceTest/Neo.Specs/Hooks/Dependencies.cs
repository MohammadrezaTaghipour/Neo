using Autofac;
using Neo.Specs.Configurations.Neo;
using Neo.Specs.Framework;
using Neo.Specs.ScreenPlay.LifeStreams.Commands;
using Neo.Specs.ScreenPlay.LifeStreams.Tasks;
using Neo.Specs.ScreenPlay.StreamContexts.Commands;
using Neo.Specs.ScreenPlay.StreamContexts.Tasks;
using Neo.Specs.ScreenPlay.StreamEvents.Commands;
using Neo.Specs.ScreenPlay.StreamEvents.Tasks;
using Neo.Specs.ScreenPlay.StreamEventTypes.Commands;
using Neo.Specs.ScreenPlay.StreamEventTypes.Tasks;
using Neo.Specs.Utils;
using Suzianna.Core.Screenplay;
using Suzianna.Core.Screenplay.Actors;
using Suzianna.Rest.Screenplay.Abilities;
using Suzianna.Rest.Screenplay.Questions;

namespace Neo.Specs.Hooks;

public static class Dependencies
{
    public static ContainerBuilder CreateContainerBuilder()
    {
        var builder = new ContainerBuilder();

        var configuration = SettingsHelper.GetConfiguration();
        builder.RegisterInstance(configuration);

        builder.RegisterNeo();

        builder.Register(a =>
        {
            var options = a.Resolve<NeoOptions>();
            var lastResponseException = new LastRequestResponse();
            var cast = Cast.WhereEveryoneCan(new List<IAbility>
                {
                    CallAnApi.At(options.ApiUrl)
                    .With(new CustomHttpRequestSender(lastResponseException))
                });

            var stage = new Stage(cast);
            stage.ShineSpotlightOn("NeoTestRunner");
            stage.ActorInTheSpotlight.Remember(lastResponseException);
            return stage;
        }).InstancePerLifetimeScope();

        builder.RegisterType(typeof(CommandBus))
            .As<ICommandBus>()
            .InstancePerLifetimeScope();

        builder.RegisterStreamEventTypeCommandHandlers();
        builder.RegisterLifeStreamCommandHandlers();
        builder.RegisterStreamContextCommandHandlers();
        builder.RegisterStreamEventCommandHandlers();

        return builder;
    }

    static void RegisterStreamEventTypeCommandHandlers(this ContainerBuilder builder)
    {
        builder.RegisterType<StreamEventTypeRestApiCommandHandler>()
            .As<ICommandHandler<DefineStreamEventTypeCommand>>()
            .InstancePerLifetimeScope();

        builder.RegisterType<StreamEventTypeRestApiCommandHandler>()
            .As<ICommandHandler<ModifyStreamEventTypeCommand>>()
            .InstancePerLifetimeScope();

        builder.RegisterType<StreamEventTypeRestApiCommandHandler>()
            .As<ICommandHandler<RemoveStreamEventTypeCommand>>()
            .InstancePerLifetimeScope();
    }

    static void RegisterLifeStreamCommandHandlers(this ContainerBuilder builder)
    {
        builder.RegisterType<LifeStreamRestApiCommandHandler>()
            .As<ICommandHandler<DefineLifeStreamCommand>>()
            .InstancePerLifetimeScope();

        builder.RegisterType<LifeStreamRestApiCommandHandler>()
            .As<ICommandHandler<ModifyLifeStreamCommand>>()
            .InstancePerLifetimeScope();

        builder.RegisterType<LifeStreamRestApiCommandHandler>()
            .As<ICommandHandler<RemoveLifeStreamCommand>>()
            .InstancePerLifetimeScope();
    }

    static void RegisterStreamContextCommandHandlers(this ContainerBuilder builder)
    {
        builder.RegisterType<StreamContextRestApiCommandHandler>()
            .As<ICommandHandler<DefineStreamContextCommand>>()
            .InstancePerLifetimeScope();

        builder.RegisterType<StreamContextRestApiCommandHandler>()
            .As<ICommandHandler<ModifyStreamContextCommand>>()
            .InstancePerLifetimeScope();

        builder.RegisterType<StreamContextRestApiCommandHandler>()
            .As<ICommandHandler<RemoveStreamContextCommand>>()
            .InstancePerLifetimeScope();
    }

    static void RegisterStreamEventCommandHandlers(this ContainerBuilder builder)
    {
        builder.RegisterType<StreamEventRestApiCommandHandler>()
            .As<ICommandHandler<AppendStreamEventCommand>>()
            .InstancePerLifetimeScope();

        builder.RegisterType<StreamEventRestApiCommandHandler>()
            .As<ICommandHandler<RemoveStreamEventCommand>>()
            .InstancePerLifetimeScope();
    }
}