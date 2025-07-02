using MonoGame.Extended.ECS;

namespace Reoria.Engine.MonoGame.World.Entities;

/// <summary>
/// Base class for all MonoGame entities. Wraps an ECS entity and assigns a unique identifier.
/// </summary>
/// <param name="entity">The underlying ECS entity.</param>
/// <param name="id">The unique identifier of the entity.</param>
public abstract class MonoGameEntity(Entity entity, Guid id)
{
    /// <summary>
    /// The underlying ECS entity this class wraps.
    /// </summary>
    protected readonly Entity Entity = entity;

    /// <summary>
    /// The unique identifier of this MonoGame entity.
    /// </summary>
    public readonly Guid Id = id;

    /// <summary>
    /// Destroys the ECS entity.
    /// </summary>
    public virtual void Destroy()
        => this.Entity.Destroy();
}
