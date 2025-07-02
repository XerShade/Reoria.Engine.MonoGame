using Microsoft.Xna.Framework.Graphics;

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
    /// <param name="graphicsDevice">The <see cref="GraphicsDevice"/> instance on this MonoGame class.</param>
    /// <param name="spriteBatch">The <see cref="SpriteBatch"/> instance on this MonoGame class.</param>
    void Initialize(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch);
}
