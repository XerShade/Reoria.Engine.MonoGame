using Microsoft.Xna.Framework;

namespace Reoria.Engine.MonoGame.Core.Interfaces;

/// <summary>
/// Defines a contract for game logic that needs to be updated each frame.
/// Typically implemented by systems or components that require regular updates.
/// </summary>
public interface IMonoGameUpdater
{
    /// <summary>
    /// Updates the game logic with the provided game time.
    /// This method is called once per frame and is responsible for all time-based logic.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    void Update(GameTime gameTime);
}
