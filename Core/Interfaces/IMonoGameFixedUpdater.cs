using Microsoft.Xna.Framework;

namespace Reoria.Engine.MonoGame.Core.Interfaces;

/// <summary>
/// Defines a contract for game logic that is updated at a fixed timestep.
/// Useful for physics and other systems requiring consistent timing.
/// </summary>
public interface IMonoGameFixedUpdater
{
    /// <summary>
    /// Updates the fixed timestep game logic with the provided game time.
    /// This is called at a regular, fixed interval regardless of frame rate.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    void Update(GameTime gameTime);
}
