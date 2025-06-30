namespace Reoria.Engine.MonoGame.Core.Interfaces;

/// <summary>
/// Defines a contract for initializing MonoGame-related systems and services.
/// Used to perform setup tasks before the game starts running.
/// </summary>
public interface IMonoGameInitializer
{
    /// <summary>
    /// Initializes the application with the provided MonoGame core instance.
    /// Use this method to register services, configure systems, and perform other startup tasks.
    /// </summary>
    /// <param name="monoGameCore">The MonoGame core instance to initialize with.</param>
    void Initialize(IMonoGameCore monoGameCore);
}
