using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Reoria.Engine.Container.Registrars;
using Reoria.Engine.MonoGame.World.Interfaces;

namespace Reoria.Engine.MonoGame.World.Registrars;

/// <summary>
/// Registers MonoGame world-related services into the dependency injection container.
/// </summary>
public class MonoGameWorldRegistrar : IServiceRegistrar
{
    /// <summary>
    /// Registers MonoGame world-related services into the dependency injection container.
    /// </summary>
    /// <inheritdoc/>
    public void RegisterServices(ContainerBuilder builder, IConfiguration configuration, ILoggerFactory loggerFactory)
    {
        // Register world builder with the container.
        _ = builder.RegisterType<MonoGameWorldBuilder>()
            .As<IMonoGameWorldBuilder>()
            .InstancePerDependency();

        // Register entity manager with the container.
        _ = builder.RegisterType<MonoGameEntityManager>()
            .As<IMonoGameEntityManager>()
            .AsImplementedInterfaces()
            .SingleInstance();
    }
}
