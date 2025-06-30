namespace Reoria.Engine.MonoGame.Core.Interfaces;

/// <summary>
/// Defines a contract for performing custom disposal logic.
/// This is useful for controlled cleanup of unmanaged resources or subscriptions.
/// </summary>
public interface IMonoGameDisposer
{
    /// <summary>
    /// Performs resource cleanup based on the disposing flag.
    /// If <c>true</c>, dispose of both managed and unmanaged resources.
    /// If <c>false</c>, dispose of unmanaged resources only.
    /// </summary>
    /// <param name="disposing">Indicates whether managed resources should be disposed.</param>
    void Dispose(bool disposing);
}