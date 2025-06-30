using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Reoria.Engine.MonoGame.Core.Interfaces;

/// <summary>
/// Defines a contract for rendering game visuals.
/// Implementers should use this interface to draw visual elements to the screen.
/// </summary>
public interface IMonoGameDrawer
{
    /// <summary>
    /// Draws game visuals using the provided <see cref="GameTime"/> and <see cref="SpriteBatch"/>.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    /// <param name="spriteBatch">The SpriteBatch used for drawing textures.</param>
    void Draw(GameTime gameTime, SpriteBatch spriteBatch);
}
