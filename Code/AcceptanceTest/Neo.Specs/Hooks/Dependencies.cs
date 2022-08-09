using Autofac;
using Neo.Specs.Configs.Neo;
using Neo.Specs.Utils;
using Suzianna.Core.Screenplay;
using Suzianna.Rest.Screenplay.Abilities;

namespace Neo.Specs.Hooks
{
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
                stage.ShineSpotlightOn("Ashly");
                return stage;
            }).InstancePerLifetimeScope();

            // builder.RegisterType(typeof(CommandBus))
            //     .As<ICommandBus>()
            //     .InstancePerLifetimeScope();

            builder.RegisterStreamEventTypeHandlers();


            return builder;
        }

        static void RegisterStreamEventTypeHandlers(this ContainerBuilder builder)
        {
            // builder.RegisterType<StreamEventTypeRestApiCommandHandler>()
            //     .As<ICommandHandler<DefineStreamEventTypeCommand>>()
            //     .InstancePerLifetimeScope();
            //
            // builder.RegisterType<StreamEventTypeRestApiCommandHandler>()
            //     .As<ICommandHandler<ModifyStreamEventTypeCommand>>()
            //     .InstancePerLifetimeScope();
            //
            // builder.RegisterType<StreamEventTypeRestApiCommandHandler>()
            //     .As<ICommandHandler<RemoveStreamEventTypeCommand>>()
            //     .InstancePerLifetimeScope();
        }
    }
}