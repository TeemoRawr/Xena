using Autofac;
using Xena.HttpClient.Generator.Services;

namespace Xena.HttpClient.Generator;

public class IocModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<ModelGenerator>();
        builder.RegisterType<PropertyGenerator>();
    }
}