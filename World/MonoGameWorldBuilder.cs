using MonoGame.Extended.ECS;
using MonoGame.Extended.ECS.Systems;
using Reoria.Engine.MonoGame.World.Interfaces;
using Reoria.Engine.Signals.Interfaces;
using MonoGameWorld = MonoGame.Extended.ECS.World;

namespace Reoria.Engine.MonoGame.World;

/// <summary>
/// Builds a MonoGame ECS world by registering systems and emitting build events via the signal bus.
/// </summary>
/// <param name="signalBus">Signal bus used for event signaling during the build process.</param>
/// <param name="systems">Collection of ECS systems to add to the world.</param>
public class MonoGameWorldBuilder(ISignalBus signalBus, IEnumerable<ISystem> systems) : IMonoGameWorldBuilder
{
    /// <summary>
    /// Provides a mechanism to emit events/signals during the build process.
    /// </summary>
    protected readonly ISignalBus SignalBus = signalBus;

    /// <summary>
    /// Collection of systems to be added to the ECS world.
    /// </summary>
    protected readonly IEnumerable<ISystem> Systems = systems;

    /// <summary>
    /// Internal world builder instance used to compose and build the ECS world.
    /// </summary>
    protected readonly WorldBuilder WorldBuilder = new();

    /// <summary>
    /// Constructs the ECS world by adding systems and emitting lifecycle signals.
    /// </summary>
    /// <returns>A fully built <see cref="MonoGameWorld"/> instance with all registered systems.</returns>
    public virtual MonoGameWorld Build()
    {
        foreach (ISystem system in this.Systems)
        {
            this.SignalBus.Emit("MonoGameWorldBuilder.OnAddSystem", system);
            _ = this.WorldBuilder.AddSystem(system);
        }

        this.SignalBus.Emit("MonoGameWorldBuilder.OnBuildWorld");
        return this.WorldBuilder.Build();
    }
}
