using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Reoria.Engine.Common;
using Reoria.Engine.MonoGame.Core.Interfaces;
using System.Diagnostics;

namespace Reoria.Engine.MonoGame.Core;

/// <summary>
/// Represents a server-side core loop for MonoGame integration within the Reoria Engine.
/// Handles initialization, fixed and variable update loops, and clean disposal of components.
/// </summary>
public class MonoGameServer : Disposable, IMonoGameCore
{
    /// <summary>
    /// Logger instance for logging server-related events.
    /// </summary>
    protected readonly ILogger<IMonoGameCore> Logger;

    /// <summary>
    /// Initializers to run during the server initialization phase.
    /// </summary>
    protected readonly IEnumerable<IMonoGameInitializer> Initializers;

    /// <summary>
    /// Updaters that are invoked every frame.
    /// </summary>
    protected readonly IEnumerable<IMonoGameUpdater> Updaters;

    /// <summary>
    /// Fixed updaters that are invoked on a fixed timestep.
    /// </summary>
    protected readonly IEnumerable<IMonoGameFixedUpdater> FixedUpdaters;

    /// <summary>
    /// Disposers to clean up resources when shutting down.
    /// </summary>
    protected readonly IEnumerable<IMonoGameDisposer> Disposers;

    /// <summary>
    /// Accumulates time and triggers fixed updates at a stable interval.
    /// </summary>
    protected readonly TimeAccumulator FixedTimeAccumulator;

    /// <summary>
    /// Provides a container for registering and accessing game services.
    /// </summary>
    public GameServiceContainer Services { get; init; } = new();

    /// <summary>
    /// Holds a value that tells the engine if the server is currently running.
    /// </summary>
    protected bool isRunning = false;

    /// <summary>
    /// Holds a value that tells the engine if the server should continue to the next loop.
    /// </summary>
    protected bool tickGameLoop = false;

    /// <summary>
    /// Constructs a new MonoGameServer with the specified dependencies.
    /// </summary>
    /// <param name="logger">Logger for outputting runtime information.</param>
    /// <param name="initializers">Initializers to prepare the game state.</param>
    /// <param name="updaters">Updaters executed every frame.</param>
    /// <param name="fixedUpdaters">Updaters executed on a fixed timestep.</param>
    /// <param name="disposers">Disposers to clean up resources on shutdown.</param>
    public MonoGameServer(
        ILogger<IMonoGameCore> logger,
        IEnumerable<IMonoGameInitializer> initializers,
        IEnumerable<IMonoGameUpdater> updaters,
        IEnumerable<IMonoGameFixedUpdater> fixedUpdaters,
        IEnumerable<IMonoGameDisposer> disposers)
    {
        this.Logger = logger;
        this.Initializers = initializers;
        this.Updaters = updaters;
        this.FixedUpdaters = fixedUpdaters;
        this.Disposers = disposers;
        this.FixedTimeAccumulator = new TimeAccumulator(1 / 20.0, 5);
    }

    /// <summary>
    /// Starts the game loop and runs until <see cref="Exit"/> is called.
    /// </summary>
    public void Run()
    {
        if (this.isRunning)
        {
            this.Logger.LogWarning("Something attempted to run the server thread while it is already running, aborting this attempt.");
            return;
        }

        this.isRunning = true;
        this.tickGameLoop = true;

        this.Initialize();

        Stopwatch stopwatch = Stopwatch.StartNew();
        double previousElapsedTime = 0.0;

        while (this.tickGameLoop)
        {
            double totalElapsedTime = stopwatch.Elapsed.TotalSeconds;
            double deltaTime = totalElapsedTime - previousElapsedTime;
            previousElapsedTime = totalElapsedTime;

            GameTime gameTime = new(
                TimeSpan.FromSeconds(totalElapsedTime),
                TimeSpan.FromSeconds(deltaTime),
                isRunningSlowly: false);

            this.FixedTimeAccumulator.Update(gameTime, this.FixedUpdate);
            this.Update(gameTime);

            Thread.Sleep(1);
        }

        stopwatch.Stop();
        this.isRunning = false;
    }

    /// <summary>
    /// Signals the server loop to stop on the next iteration.
    /// </summary>
    public void Exit()
        => this.tickGameLoop = false;

    /// <summary>
    /// Initializes the game and calls all registered initializers.
    /// </summary>
    protected virtual void Initialize()
    {
        foreach (IMonoGameInitializer initializer in this.Initializers)
        {
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            initializer.Initialize(null, null);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.
        }
    }

    /// <summary>
    /// Updates the game state, handles input for exit conditions,
    /// runs fixed timestep updates, and calls all regular update handlers.
    /// </summary>
    /// <param name="gameTime">Timing information for the current frame.</param>
    protected virtual void Update(GameTime gameTime)
    {
        this.FixedTimeAccumulator?.Update(gameTime, this.FixedUpdate);

        foreach (IMonoGameUpdater updater in this.Updaters)
        {
            updater.Update(gameTime);
        }
    }

    /// <summary>
    /// Executes all registered fixed update handlers.
    /// Called at a stable fixed timestep defined by <see cref="FixedTimeAccumulator"/>.
    /// </summary>
    /// <param name="gameTime">Timing information for the fixed update.</param>
    protected virtual void FixedUpdate(GameTime gameTime)
    {
        foreach (IMonoGameFixedUpdater fixedUpdater in this.FixedUpdaters)
        {
            fixedUpdater.Update(gameTime);
        }
    }

    /// <summary>
    /// Disposes all registered disposable components.
    /// </summary>
    /// <param name="disposing">Indicates if managed resources should be disposed.</param>
    protected override void Dispose(bool disposing)
    {
        foreach (IMonoGameDisposer disposer in this.Disposers)
        {
            disposer.Dispose(disposing);
        }

        base.Dispose(disposing);
    }
}
