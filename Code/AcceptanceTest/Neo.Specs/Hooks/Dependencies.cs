using Autofac;
using Neo.Specs.Configurations.Neo;
using Neo.Specs.Framework;
using Neo.Specs.ScreenPlay.LifeStreams.Commands;
using Neo.Specs.ScreenPlay.LifeStreams.Tasks;
using Neo.Specs.ScreenPlay.StreamContexts.Commands;
using Neo.Specs.ScreenPlay.StreamContexts.Tasks;
using Neo.Specs.ScreenPlay.StreamEventTypes.Commands;
using Neo.Specs.ScreenPlay.StreamEventTypes.Tasks;
using Neo.Specs.Utils;
using Suzianna.Core.Screenplay;
using Suzianna.Rest.Screenplay.Abilities;

namespace Neo.Specs.Hooks;

public static class Dependencies
{
    public static ContainerBuilder CreateContainerBuilder()
    {
        var builder = new ContainerBuilder();

        var configuration = SettingsHelper.GetConfiguration();
        builder.RegisterInstance(configuration);

        builder.RegisterNeo();
        // builder.RegisterNeo4j();

        builder.Register(a =>
        {
            var options = a.Resolve<NeoOptions>();
            var cast = Cast.WhereEveryoneCan(new List<IAbility>
                {
                    CallAnApi.At(options.ApiUrl).With(new CustomHttpRequestSender())
                });

            var stage = new Stage(cast);
            stage.ShineSpotlightOn("Dave");
            return stage;
        }).InstancePerLifetimeScope();

        builder.RegisterType(typeof(CommandBus))
            .As<ICommandBus>()
            .InstancePerLifetimeScope();

        builder.RegisterStreamEventTypeCommandHandlers();
        builder.RegisterLifeStreamCommandHandlers();
        builder.RegisterStreamContextCommandHandlers();


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
}