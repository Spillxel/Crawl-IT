#region Using Statements

using System;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

using ResolutionBuddy;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Graphics;

using CrawlIT.Shared.Entity;
using CrawlIT.Shared.GameStates;

using Camera = CrawlIT.Shared.Camera.Camera;
using XnaMediaPlayer = Microsoft.Xna.Framework.Media.MediaPlayer;

#endregion

namespace CrawlIT
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private TouchCollection _touchCollection;

        private readonly IResolution _resolution;

        private Camera _staticCamera;
        private Song _backgroundSong;

        private Player _player;
        private Camera _playerCamera;
        private Texture2D _playerTexture;

        private GameState _menu;
        private GameState _level;
        private GameState _fight;

        private Texture2D _startButton;
        private Texture2D _exitButton;
        private Texture2D _pauseButton;
        private Point _startSize;
        private Point _exitSize;
        private Point _pauseSize;

        private Rectangle _touch;

        private enum GameState
        {
            Menu,
            Playing,
            Fighting
        }

        private SpriteFont _font;

        private TiledMapRenderer _mapRenderer;
        private RenderTarget2D _mapRenderTarget;
        private TiledMap _map;

        private float _zoom;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this)
            {   // Force orientation to be fullscreen portrait
                SupportedOrientations = DisplayOrientation.Portrait,
                IsFullScreen = true
            };

            _resolution = new ResolutionComponent(this, _graphics,
                                                  new Point(720, 1280), new Point(720, 1280),
                                                  true, false);

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
            // TODO: think up a better way to zoom for different resolutions
            _zoom = _graphics.PreferredBackBufferHeight > 1280
                                                        ? 6.0f
                                                        : 3.0f;

            _menu = new Menu(GraphicsDevice, _zoom);
            _menu.SetState(GameState.Menu);

            _level = new Level(GraphicsDevice);
            _level.SetState(_gameStates.Playing);

            _fight = new Fight(GraphicsDevice);
            _fight.SetState(_gameStates.Fighting);

            // TODO: make this work
            // _gameStateManager.GameState = GameState.StartMenu;

            _mapRenderer = new TiledMapRenderer(GraphicsDevice);

            // Repeat _backgroundSong on end
            XnaMediaPlayer.IsRepeating = true;

            base.Initialize();

            var pp = _graphics.GraphicsDevice.PresentationParameters;
            _mapRenderTarget = new RenderTarget2D(GraphicsDevice,
                                                  _graphics.PreferredBackBufferWidth,
                                                  _graphics.PreferredBackBufferHeight,
                                                  false, SurfaceFormat.Color,
                                                  DepthFormat.None,
                                                  pp.MultiSampleCount,
                                                  RenderTargetUsage.DiscardContents);
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

            _map = Content.Load<TiledMap>("Maps/test");

            _playerTexture = Content.Load<Texture2D>("Sprites/playerspritesheet");
            _player = new Player(_playerTexture, _resolution.TransformationMatrix());
            _playerCamera = new Camera(_graphics.PreferredBackBufferWidth,
                                       _graphics.PreferredBackBufferHeight,
                                       _zoom);

            _startButton = Content.Load<Texture2D>(@"start");
            _exitButton = Content.Load<Texture2D>(@"exit");
            _pauseButton = Content.Load<Texture2D>(@"pause");

            _startSize = new Point(_startButton.Width * (int)_zoom,
                                   _startButton.Height * (int)_zoom);

            _exitSize = new Point(_exitButton.Width * (int)_zoom,
                                  _exitButton.Height * (int)_zoom);

            _pauseSize = new Point(_pauseButton.Width, _pauseButton.Height);

            _font = Content.Load<SpriteFont>("Fonts/File");

            _staticCamera = new Camera(0, 0, 1.0f);

            // Fetching list of collision objects in the map to check for collision
            var collisionObjects = _map.ObjectLayers[0].Objects
                                                       .Select(o => new Rectangle((int) o.Position.X, (int) o.Position.Y,
                                                                                  (int) o.Size.Width, (int) o.Size.Height))
                                                       .ToList();
            _player.CollisionObjects = collisionObjects;

            // Set the content to the GameStateManager to be able to use it
            GameStateManager.Instance.SetContent(Content);

            // Initialize by adding the Menu screen into the game
            GameStateManager.Instance.AddScreen(_menu);
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

            if (_touchCollection.Count > 0)
                _touch = new Rectangle((int)_touchCollection[0].Position.X,
                                       (int)_touchCollection[0].Position.Y,
                                       5, 5);

            if (GameStateManager.Instance.IsState(_gameStates.Menu))
            {
                Rectangle _start = new Rectangle(_menu.GetPosition(_startButton), _startSize);

                Rectangle _exit = new Rectangle(_menu.GetPosition(_exitButton), _exitSize);

                if (_touch.Intersects(_start))
                    GameStateManager.Instance.ChangeScreen(_level);

                // TODO: remove exit button altogether (bad practice for mobile development)
                if (_touch.Intersects(_exit))
                    Game.Activity.MoveTaskToBack(true);
            }

            if (GameStateManager.Instance.IsState(GameState.Playing))
            {
                _mapRenderer.Update(_map, gameTime);

                _player.Update(gameTime);

                _playerCamera.Follow(_player);
                _staticCamera.Follow(null);

                Rectangle _pause = new Rectangle(_level.GetPosition(_pauseButton), _pauseSize);

                if (_touch.Intersects(_pause))
                    GameStateManager.Instance.AddScreen(_fight);
            }

            if (GameStateManager.Instance.IsState(GameState.Fighting))
            {
                Rectangle _pause = new Rectangle(_fight.GetPosition(_pauseButton), _pauseSize);

                if (_touch.Intersects(_pause))
                    GameStateManager.Instance.RemoveScreen();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>j
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateGray);

            GameStateManager.Instance.Draw(_spriteBatch);

            if (GameStateManager.Instance.IsState(GameState.Playing))
            {
                //Little trick to show the tiled map as PointClamp even though we don't use spritebatch to draw it
                GraphicsDevice.SetRenderTarget(_mapRenderTarget);
                GraphicsDevice.BlendState = BlendState.AlphaBlend;
                GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
                GraphicsDevice.RasterizerState = RasterizerState.CullNone;

                _mapRenderer.Draw(_map, viewMatrix: _playerCamera.Transform);

                GraphicsDevice.SetRenderTarget(null);
                _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                _spriteBatch.Draw(_mapRenderTarget,
                                  destinationRectangle: new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight),
                                  color: Color.White);
                _spriteBatch.End();
                //End of little trick

                _spriteBatch.Begin(SpriteSortMode.BackToFront,
                                   BlendState.AlphaBlend,
                                   SamplerState.PointClamp,
                                   null, null, null,
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
            var fpsString = "FPS: " + Math.Ceiling(fps);
            var (stringDimensionX, stringDimensionY) = _font.MeasureString(fpsString);

            var stringPosX = (_graphics.PreferredBackBufferWidth - stringDimensionX) / 2;
            var stringPosY = _graphics.PreferredBackBufferHeight - stringDimensionY;

            if (!GameStateManager.Instance.IsState(GameState.Fighting))
            {
                _spriteBatch.Begin();
                _spriteBatch.DrawString(_font, fpsString, new Vector2(stringPosX, stringPosY), Color.Black);
                _spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}
