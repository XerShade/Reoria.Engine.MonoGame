using Microsoft.Extensions.Logging;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Reoria.Engine.Common;
using Reoria.Engine.MonoGame.Content.Interfaces;
using Reoria.Engine.MonoGame.Core.Interfaces;
using MonoGameBase = Microsoft.Xna.Framework.Game;

namespace Reoria.Engine.MonoGame.Core;

/// <summary>
/// The core MonoGame client for the Reoria engine, managing lifecycle, rendering, and update logic.
/// Handles initialization, content loading/unloading, updating, fixed timestep updates, drawing, and disposal.
/// </summary>
public class MonoGameClient : MonoGameBase, IMonoGameCore
{
    /// <summary>
    /// Logger for recording informational, warning, and error messages during runtime.
    /// </summary>
    protected readonly ILogger<IMonoGameCore> Logger;

    /// <summary>
    /// Collection of initializers executed during game initialization.
    /// </summary>
    protected readonly IEnumerable<IMonoGameInitializer> Initializers;

    /// <summary>
    /// Collection of content loaders responsible for loading game assets.
    /// </summary>
    protected readonly IEnumerable<IContentLoader> ContentLoaders;

    /// <summary>
    /// Collection of update handlers called every frame.
    /// </summary>
    protected readonly IEnumerable<IMonoGameUpdater> Updaters;

    /// <summary>
    /// Collection of fixed update handlers called at a fixed timestep.
    /// </summary>
    protected readonly IEnumerable<IMonoGameFixedUpdater> FixedUpdaters;

    /// <summary>
    /// Collection of drawer handlers responsible for rendering each frame.
    /// </summary>
    protected readonly IEnumerable<IMonoGameDrawer> Drawers;

    /// <summary>
    /// Collection of content unloaders responsible for releasing loaded content.
    /// </summary>
    protected readonly IEnumerable<IContentUnloader> ContentUnloaders;

    /// <summary>
    /// Collection of disposers responsible for cleaning up resources.
    /// </summary>
    protected readonly IEnumerable<IMonoGameDisposer> Disposers;

    /// <summary>
    /// Manages graphics device settings such as resolution and fullscreen.
    /// </summary>
    protected readonly GraphicsDeviceManager GraphicsDeviceManager;

    /// <summary>
    /// Accumulates elapsed time to trigger fixed timestep updates reliably.
    /// </summary>
    protected readonly TimeAccumulator FixedTimeAccumulator;

    /// <summary>
    /// SpriteBatch used for batching 2D rendering operations.
    /// Initialized during <see cref="Initialize"/>.
    /// </summary>
    protected SpriteBatch? SpriteBatch;

    /// <summary>
    /// Constructs a new instance of <see cref="MonoGameClient"/> with injected dependencies.
    /// </summary>
    /// <param name="logger">Logger for outputting runtime information.</param>
    /// <param name="initializers">Initializers to prepare the game state.</param>
    /// <param name="contentLoaders">Loaders to load game assets.</param>
    /// <param name="updaters">Updaters executed every frame.</param>
    /// <param name="fixedUpdaters">Updaters executed on a fixed timestep.</param>
    /// <param name="drawers">Drawers responsible for rendering content.</param>
    /// <param name="contentUnloaders">Unloaders to release loaded assets.</param>
    /// <param name="disposers">Disposers to clean up resources on shutdown.</param>
    public MonoGameClient(ILogger<IMonoGameCore> logger,
        IEnumerable<IMonoGameInitializer> initializers,
        IEnumerable<IContentLoader> contentLoaders,
        IEnumerable<IMonoGameUpdater> updaters,
        IEnumerable<IMonoGameFixedUpdater> fixedUpdaters,
        IEnumerable<IMonoGameDrawer> drawers,
        IEnumerable<IContentUnloader> contentUnloaders,
        IEnumerable<IMonoGameDisposer> disposers)
    {
        this.Logger = logger;
        this.Initializers = initializers;
        this.ContentLoaders = contentLoaders;
        this.Updaters = updaters;
        this.FixedUpdaters = fixedUpdaters;
        this.Drawers = drawers;
        this.ContentUnloaders = contentUnloaders;
        this.Disposers = disposers;

        this.GraphicsDeviceManager = new GraphicsDeviceManager(this);
        this.FixedTimeAccumulator = new TimeAccumulator(1 / 20.0, 5); // 20 FPS fixed timestep
        this.Content.RootDirectory = "Assets/";
    }

    /// <summary>
    /// Initializes game components and executes all registered initializers.
    /// </summary>
    protected override void Initialize()
    {
        this.SpriteBatch = new SpriteBatch(this.GraphicsDevice);

        foreach (IMonoGameInitializer initializer in this.Initializers)
        {
            initializer.Initialize(this);
        }

        base.Initialize();
    }

    /// <summary>
    /// Loads game content by invoking all registered content loaders.
    /// </summary>
    protected override void LoadContent()
    {
        foreach (IContentLoader contentLoader in this.ContentLoaders)
        {
            contentLoader.LoadContent(this.Content);
        }

        base.LoadContent();
    }

    /// <summary>
    /// Updates the game state, handles input for exit conditions,
    /// runs fixed timestep updates, and calls all regular update handlers.
    /// </summary>
    /// <param name="gameTime">Timing information for the current frame.</param>
    protected override void Update(GameTime gameTime)
    {
        // Exit if Back button (GamePad) or Escape key (Keyboard) is pressed
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
            || Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            this.Exit();
        }

        this.FixedTimeAccumulator?.Update(gameTime, this.FixedUpdate);

        foreach (IMonoGameUpdater updater in this.Updaters)
        {
            updater.Update(gameTime);
        }

        base.Update(gameTime);
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
    /// Renders the game screen by clearing the graphics device and drawing all registered drawers.
    /// Uses <see cref="SpriteBatch"/> for batched rendering with point clamp filtering.
    /// </summary>
    /// <param name="gameTime">Timing information for the current frame.</param>
    protected override void Draw(GameTime gameTime)
    {
        this.GraphicsDevice.Clear(Color.CornflowerBlue);

        if (this.SpriteBatch is not null)
        {
            this.SpriteBatch.Begin(
                samplerState: SamplerState.PointClamp,
                sortMode: SpriteSortMode.Deferred);

            foreach (IMonoGameDrawer drawer in this.Drawers)
            {
                drawer.Draw(gameTime, this.SpriteBatch);
            }

            this.SpriteBatch.End();
        }

        base.Draw(gameTime);
    }

    /// <summary>
    /// Unloads all game content by invoking registered content unloaders.
    /// </summary>
    protected override void UnloadContent()
    {
        foreach (IContentUnloader contentUnloader in this.ContentUnloaders)
        {
            contentUnloader.UnloadContent(this.Content);
        }

        base.UnloadContent();
    }

    /// <summary>
    /// Disposes all registered disposers and releases resources.
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
