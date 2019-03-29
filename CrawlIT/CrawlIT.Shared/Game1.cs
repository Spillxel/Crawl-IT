#region Using Statements

using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using XnaMediaPlayer = Microsoft.Xna.Framework.Media.MediaPlayer;

using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Graphics;

using ResolutionBuddy;

using CrawlIT.Shared.Entity;
using CrawlIT.Shared.Camera;
using Camera = CrawlIT.Shared.Camera.Camera;
using CrawlIT.Shared.GameStates;

#endregion

namespace CrawlIT
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private TouchCollection _touchCollection;

        private readonly IResolution _resolution;

        private GameStateManager _gameStateManager;

        private Camera _staticCamera;
        private Song _backgroundSong;

        private Player _player;
        private Camera _playerCamera;
        private Texture2D _playerTexture;

        private SpriteFont _font;

        private TiledMap _map;
        private TiledMapRenderer _mapRenderer;

        private int _level;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this)
            {   // Force orientation to be fullscreen portrait
                SupportedOrientations = DisplayOrientation.Portrait,
                IsFullScreen = true
            };

            _resolution =
                new ResolutionComponent(this, _graphics, new Point(720, 1280),
                                        new Point(720, 1280), true, false);

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            _level = 0;

            // TODO: make this work
            // _gameStateManager.GameState = GameState.StartMenu;

            _mapRenderer = new TiledMapRenderer(GraphicsDevice);

            // Repeat _backgroundSong on end
            XnaMediaPlayer.IsRepeating = true;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _backgroundSong = Content.Load<Song>("Audio/Investigations");
            XnaMediaPlayer.Play(_backgroundSong);

            _map = Content.Load<TiledMap>("Maps/test2");

            _playerTexture = Content.Load<Texture2D>("Sprites/charactersheet");
            _player = new Player(_playerTexture, _resolution.TransformationMatrix());
            _playerCamera = new Camera(_graphics.PreferredBackBufferWidth,
                                       _graphics.PreferredBackBufferHeight,
                                       6.0f);

            _font = Content.Load<SpriteFont>("Fonts/File");

            _staticCamera = new Camera(0, 0, 1.0f);

            GameStateManager.Instance.SetContent(Content);
            GameStateManager.Instance.AddScreen(new Menu(GraphicsDevice));
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // For Mobile devices, this logic will close the Game when the Back button is pressed
            // Exit() is obsolete on iOS
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
                || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
#if !__IOS__
                Game.Activity.MoveTaskToBack(true);
#endif
            }

            GameStateManager.Instance.Update(gameTime);

            _touchCollection = TouchPanel.GetState();
            if (_level == 0 && _touchCollection.Count > 0)
            {
                // TODO: re-implement Start/Exit button touch...
                GameStateManager.Instance.RemoveScreen();
                GameStateManager.Instance.AddScreen(new Level2(GraphicsDevice));

                _level = 1;
            }

            if (_level != 0)
            {
                _mapRenderer.Update(_map, gameTime);
                _player.Update(gameTime);

                _playerCamera.Follow(_player);
                _staticCamera.Follow(null);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateGray);
        
            GameStateManager.Instance.Draw(_spriteBatch);

            if (_level != 0)
            {
                _mapRenderer.Draw(_map, viewMatrix: _playerCamera.Transform);
                _spriteBatch.Begin(SpriteSortMode.BackToFront,
                                   BlendState.AlphaBlend,
                                   null, null, null, null,
                                   _playerCamera.Transform);
                _player.Draw(_spriteBatch);
                _spriteBatch.End();

                // Saving this here for future reference...
                /*_spriteBatch.Begin(SpriteSortMode.BackToFront,
                                  BlendState.AlphaBlend,
                                  null, null, null, null,
                                  _staticCamera.Transform);*/
            }

            // FPS counter
            var fps = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
            var fpsString = "FPS: " + fps;

            _spriteBatch.Begin();
            _spriteBatch.DrawString(_font, fpsString, new Vector2(0, _graphics.PreferredBackBufferHeight - 70), Color.Black);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
