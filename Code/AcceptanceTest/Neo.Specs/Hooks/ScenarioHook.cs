using Autofac;
using BoDi;
using SpecFlow.Autofac;
using TechTalk.SpecFlow;

namespace Neo.Specs.Hooks;

[Binding]
public class ScenarioHook
{
    private readonly IObjectContainer _container;

    public ScenarioHook(IObjectContainer container)
    {
        _container = container;
    }

    [ScenarioDependencies]
    public static ContainerBuilder CreateContainerBuilder()
    {
        var builder = Dependencies.CreateContainerBuilder();

        builder.RegisterTypes(typeof(ScenarioHook).Assembly.GetTypes()
            .Where(t => Attribute.IsDefined(t, typeof(BindingAttribute)))
            .ToArray()).InstancePerLifetimeScope();
        return builder;
    }
}