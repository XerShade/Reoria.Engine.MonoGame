using Microsoft.Xna.Framework.Content;

namespace Reoria.Engine.MonoGame.Content.Interfaces;

/// <summary>
/// Defines a contract for unloading game content using a <see cref="ContentManager"/>.
/// Implementers should use this to dispose or release content resources cleanly.
/// </summary>
public interface IContentUnloader
{
    /// <summary>
    /// Unloads game content using the provided content manager.
    /// </summary>
    /// <param name="content">The content manager used to unload game assets.</param>
    void UnloadContent(ContentManager content);
}
