using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.ECS;
using Reoria.Engine.MonoGame.Core.Interfaces;
using Reoria.Engine.MonoGame.World.Entities;
using Reoria.Engine.MonoGame.World.Interfaces;
using Reoria.Engine.Signals.Interfaces;
using MonoGameWorld = MonoGame.Extended.ECS.World;

namespace Reoria.Engine.MonoGame.World;

/// <summary>
/// Manages MonoGame entities in the game world, including spawning, updating, drawing, and destroying them.
/// </summary>
public class MonoGameEntityManager : IMonoGameEntityManager, IMonoGameUpdater, IMonoGameDrawer
{
    /// <summary>
    /// The signal bus for dispatching and receiving events.
    /// </summary>
    protected readonly ISignalBus SignalBus;

    /// <summary>
    /// Responsible for building the ECS world instance.
    /// </summary>
    protected readonly IMonoGameWorldBuilder WorldBuilder;

    /// <summary>
    /// The ECS world that contains and manages all entities and systems.
    /// </summary>
    protected readonly MonoGameWorld GameWorld;

    /// <summary>
    /// A dictionary mapping entity GUIDs to their corresponding MonoGame entity instances.
    /// </summary>
    protected readonly Dictionary<Guid, MonoGameEntity> Entities;

    /// <summary>
    /// Initializes a new instance of the <see cref="MonoGameEntityManager"/> class.
    /// </summary>
    /// <param name="signalBus">The signal bus for event dispatching.</param>
    /// <param name="worldBuilder">Builder responsible for constructing the ECS world.</param>
    public MonoGameEntityManager(ISignalBus signalBus, IMonoGameWorldBuilder worldBuilder)
    {
        // Assign services and create the game world.
        this.SignalBus = signalBus;
        this.WorldBuilder = worldBuilder;
        this.GameWorld = this.WorldBuilder.Build();
        this.Entities = [];

        // Emit a signal that construction is done.
        this.SignalBus.Emit("MonoGameEntityManager.OnConstructed", this);
    }

    /// <inheritdoc/>
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        => this.GameWorld.Draw(gameTime);

    /// <inheritdoc/>
    public void Update(GameTime gameTime)
        => this.GameWorld.Update(gameTime);

    /// <inheritdoc/>
    public virtual TEntity SpawnEntity<TEntity>() where TEntity : MonoGameEntity
    {
        // Create a new guid for the entity.
        Guid guid = Guid.NewGuid();

        // Check to see if the guid is taken.
        while (this.Entities.ContainsKey(guid))
        {
            // It is taken, generate a new one and check again.
            guid = Guid.NewGuid();
        }

        // Spawn the entity using the new guid.
        return this.SpawnEntity<TEntity>(guid);
    }

    /// <inheritdoc/>
    public virtual TEntity SpawnEntity<TEntity>(Guid guid) where TEntity : MonoGameEntity
    {
        // Check to see if the guid is already taken.
        if (!this.Entities.ContainsKey(guid))
        {
            // Spawn an entity and create a new wrapper object using the entity and guid.
            TEntity entity = this.CreateEntity<TEntity>(this.GameWorld.CreateEntity(), Guid.NewGuid());

            // Try to add the new entity to the collection.
            if (this.Entities.TryAdd(guid, entity))
            {
                // Emit a signal that it was created and return it.
                this.SignalBus.Emit("MonoGameEntityManager.OnSpawnEntity", entity);
                return entity;
            }

            // Something went wrong adding the entity, throw an exception.
            throw new InvalidOperationException("Unable to add entity to collection.");
        }

        // A duplicate guid was passed in, throw an exception.
        throw new InvalidOperationException("Duplicate entity guid detected.");
    }

    /// <summary>
    /// Creates an instance of a MonoGame entity of type <typeparamref name="TEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity">Type of the MonoGameEntity to create.</typeparam>
    /// <param name="entity">The ECS entity.</param>
    /// <param name="guid">The unique identifier for the entity.</param>
    /// <returns>The created entity.</returns>
    protected virtual TEntity CreateEntity<TEntity>(Entity entity, Guid guid) where TEntity : MonoGameEntity
    {
        // Use the activator to create a new entity wrapper instance.
        return Activator.CreateInstance(typeof(TEntity), entity, guid) as TEntity
            ?? throw new NullReferenceException("Unable to create MonoGame entity.");
    }

    /// <inheritdoc/>
    public virtual void DestroyEntity<TEntity>(TEntity entity) where TEntity : MonoGameEntity
        => this.DestroyEntity(entity.Id);

    /// <inheritdoc/>
    public virtual void DestroyEntity(Guid guid)
    {
        // Attempt to remove the entity.
        if (this.Entities.Remove(guid, out MonoGameEntity? entity))
        {
            // It was removed from the collection, emit a signal and destroy it.
            this.SignalBus.Emit("MonoGameEntityManager.OnDestroyEntity", entity);
            entity.Destroy();
        }
    }

    /// <inheritdoc/>
    public virtual TEntity GetEntity<TEntity>(Guid guid) where TEntity : MonoGameEntity
    {
        // Attempt to return the entity, or spawn a new one.
        return this.Entities.TryGetValue(guid, out MonoGameEntity? entity)
            ? entity as TEntity ?? throw new NullReferenceException() : this.SpawnEntity<TEntity>(guid);
    }
}