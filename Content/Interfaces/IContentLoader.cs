using Microsoft.Xna.Framework.Content;

namespace Reoria.Engine.MonoGame.Content.Interfaces;

/// <summary>
/// Defines a contract for loading game content using a <see cref="ContentManager"/>.
/// Typically used to load textures, fonts, sounds, and other assets.
/// </summary>
public interface IContentLoader
{
    /// <summary>
    /// Loads game content using the provided content manager.
    /// </summary>
    /// <param name="content">The content manager used to load game assets.</param>
    void LoadContent(ContentManager content);
}
