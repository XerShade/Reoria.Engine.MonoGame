using MonoGameWorld = MonoGame.Extended.ECS.World;

namespace Reoria.Engine.MonoGame.World.Interfaces;

/// <summary>
/// Provides a contract for building a MonoGame ECS world.
/// </summary>
public interface IMonoGameWorldBuilder
{
    /// <summary>
    /// Builds and returns a configured <see cref="MonoGameWorld"/> instance.
    /// </summary>
    /// <returns>A fully constructed <see cref="MonoGameWorld"/>.</returns>
    MonoGameWorld Build();
}
