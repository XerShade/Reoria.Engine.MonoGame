using Reoria.Engine.MonoGame.World.Entities;

namespace Reoria.Engine.MonoGame.World.Interfaces;

/// <summary>
/// Interface for managing the lifecycle and retrieval of MonoGame entities.
/// </summary>
public interface IMonoGameEntityManager
{
    /// <summary>
    /// Destroys an entity by its GUID.
    /// </summary>
    /// <param name="guid">The GUID of the entity to destroy.</param>
    void DestroyEntity(Guid guid);

    /// <summary>
    /// Destroys the given entity instance.
    /// </summary>
    /// <typeparam name="TEntity">Type of the MonoGameEntity.</typeparam>
    /// <param name="entity">The entity instance to destroy.</param>
    void DestroyEntity<TEntity>(TEntity entity) where TEntity : MonoGameEntity;

    /// <summary>
    /// Retrieves an entity by GUID or spawns one if it does not exist.
    /// </summary>
    /// <typeparam name="TEntity">Type of the MonoGameEntity.</typeparam>
    /// <param name="guid">The GUID of the entity to retrieve.</param>
    /// <returns>The requested entity instance.</returns>
    TEntity GetEntity<TEntity>(Guid guid) where TEntity : MonoGameEntity;

    /// <summary>
    /// Spawns a new entity with a new GUID.
    /// </summary>
    /// <typeparam name="TEntity">Type of the MonoGameEntity to spawn.</typeparam>
    /// <returns>The spawned entity instance.</returns>
    TEntity SpawnEntity<TEntity>() where TEntity : MonoGameEntity;

    /// <summary>
    /// Spawns a new entity using a specified GUID.
    /// </summary>
    /// <typeparam name="TEntity">Type of the MonoGameEntity to spawn.</typeparam>
    /// <param name="guid">The GUID to assign to the spawned entity.</param>
    /// <returns>The spawned entity instance.</returns>
    TEntity SpawnEntity<TEntity>(Guid guid) where TEntity : MonoGameEntity;
}
