using Microsoft.Extensions.DependencyInjection;
using Reoria.Engine.Container.Registrars;
using Reoria.Engine.Container.Services.Interfaces;
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
    /// <param name="registryGuard">
    /// A service registry guard used to prevent duplicate registrations and ensure safe registration.
    /// </param>
    public void RegisterServices(IServiceRegistryGuard registryGuard)
        => registryGuard.TryRegister<IMonoGameWorldBuilder, MonoGameWorldBuilder>(ServiceLifetime.Transient);
}
