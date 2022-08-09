using Autofac;
using Microsoft.Extensions.Configuration;

namespace Neo.Specs.Configs.Neo
{
    public static class Extension
    {
        public static ContainerBuilder RegisterNeo(this ContainerBuilder container)
        {
            container.Register(x =>
            {
                var configuration = x.Resolve<IConfigurationRoot>();
                var option = configuration.GetOptions<NeoOptions>("Neo");
                return option;
            });
            return container;
        }
    }
}