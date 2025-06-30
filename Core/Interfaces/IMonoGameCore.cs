using Microsoft.Xna.Framework;

namespace Reoria.Engine.MonoGame.Core.Interfaces;

/// <summary>
/// Represents the core interface for a MonoGame project.
/// Provides essential methods and properties for running and managing the game lifecycle.
/// </summary>
public interface IMonoGameCore : IDisposable
{
    /// <summary>
    /// Gets the container for game services.
    /// This allows shared services to be registered and retrieved throughout the application.
    /// </summary>
    GameServiceContainer Services { get; }

    /// <summary>
    /// Exits the game, triggering shutdown procedures and cleanup.
    /// </summary>
    void Exit();

    /// <summary>
    /// Starts and runs the game loop.
    /// This method typically blocks until the game exits.
    /// </summary>
    void Run();
}
