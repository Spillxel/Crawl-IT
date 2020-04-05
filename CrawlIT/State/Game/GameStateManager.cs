using System;
using System.Collections.Generic;
using Android.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CrawlIT.Shared
{
    public enum GameStateType
    {
        Splash,
        Menu,
        Playing,
        Fighting,
    }

    /// <summary>
    /// <para>GameStateManager class. A class to manage game state.</para>
    /// <para>Implemented using the 'Singleton' pattern as described
    /// <see href="https://stackoverflow.com/questions/52173966/whats-the-correct-way-to-have-singleton-class-in-c-sharp">
    /// in this StackOverflow thread</see>.</para>
    /// </summary>
    public sealed class GameStateManager
    {
        // Lazy instantiation of the manager
        private static readonly Lazy<GameStateManager> LazyInstance =
            new Lazy<GameStateManager>(() => new GameStateManager());
        // Outside access to the manager
        public static GameStateManager Instance => LazyInstance.Value;

        // Stack of states/screens (think map -> menu, and then back to map)
        private readonly Stack<GameState> _stack;
        public GameStateType State => _stack.Peek().State;

        private GraphicsDevice _graphicsDevice;
        private ContentManager _content;
        private Rectangle _resolutionRectangle;
        
        // Translucent overlay background
        private readonly Color _overlayColor;
        private Texture2D _overlayTexture;

        // Prevent public constructor generation
        private GameStateManager()
        {
            _stack = new Stack<GameState>();
            _overlayColor = new Color(Color.Black, 0.7f);
        }

        /// <summary>
        /// Initializes <see cref="GameStateManager"/>.
        /// </summary>
        /// <param name="graphicsDevice">Game's graphics device.</param>
        /// <param name="content">Game's content manager.</param>
        public void Initialize(GraphicsDevice graphicsDevice, ContentManager content)
        {
            _graphicsDevice = graphicsDevice;
            _content = content;
            var resolutionSize = new Point(_graphicsDevice.Viewport.Width, _graphicsDevice.Viewport.Height);
            _resolutionRectangle = new Rectangle(Point.Zero, resolutionSize);
            _overlayTexture = new Texture2D(_graphicsDevice, 1, 1);
            _overlayTexture.SetData(new [] { Color.White });
        }

        /// <summary>
        /// Initializes and pushes <paramref name="state"/> onto <see cref="_stack"/>.
        /// </summary>
        /// <param name="state"></param>
        public void Push(GameState state)
        {
            state.Initialize();
            state.LoadContent(_content);
            _stack.Push(state);
        }

        /// <summary>
        /// Tries to pop from <see cref="_stack"/>.
        /// </summary>
        public void Pop()
        {
            if (!_stack.TryPop(out _))
                Log.Warn("GameStateManager::Pop", "Tried to pop on empty stack.");
        }

        /// <summary>
        /// Clears <see cref="_stack"/> and calls <see cref="GameState.Dispose"/> on each <see cref="GameState"/> from
        /// <see cref="_stack"/>.
        /// </summary>
        public void Clear()
        {
            while (_stack.TryPop(out var state))
                state.Dispose();
        }

        /// <summary>
        /// Clears all <see cref="GameState"/>s from <see cref="_stack"/> and <see cref="Push"/>es
        /// <paramref name="state"/> onto it.
        /// </summary>
        /// <param name="state">The <see cref="GameState"/> to push onto <see cref="_stack"/>.</param>
        public void Set(GameState state)
        {
            Clear();
            Push(state);
        }

        /// <summary>
        /// Calls <see cref="GameState.Update"/> on <see cref="GameState"/> on top of <see cref="GameStateManager"/>
        /// stack.
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            if (_stack.TryPeek(out var state))
                state.Update(gameTime);
            else
                Log.Warn("GameStateManager::Update", "Tried to peek on empty stack.");
        }

        /// <summary>
        /// Calls draw on <see cref="GameState"/> on top of <see cref="GameStateManager"/> stack.
        /// </summary>
        /// <param name="spriteBatch"><see cref="SpriteBatch"/> to draw to.</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (_stack.TryPeek(out var state))
                state.Draw(spriteBatch);
            else
                Log.Warn("GameStateManager::Draw", "Tried to peek on empty stack.");
        }
        
        /// <summary>
        /// Disposes of <see cref="GameStateManager"/>. Also calls <see cref="Clear"/>.
        /// </summary>
        public void Dispose()
        {
            // TODO: clean up the GameStateManager here too
            Clear();
        }

        /// <summary>
        /// Draws a translucent overlay onto the current <see cref="GameState"/> to 'dim' the screen. Used for
        /// highlighting an extra overlay (think dialogue).
        /// </summary>
        /// <param name="spriteBatch"><see cref="SpriteBatch"/> to draw to.</param>
        public void DrawTranslucentOverlay(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(_overlayTexture, _resolutionRectangle, _overlayColor);
            spriteBatch.End();
        }
    }
}
