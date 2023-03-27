using Autofac;
using Xena.HttpClient.Generator.Services.ModelGenerator;

namespace Xena.HttpClient.Generator;

public class IocModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterType<ModelGeneratorService>();
        builder.RegisterType<PropertyGenerator>();
        builder.RegisterType<ApiClientGeneratorService>();
    }
}