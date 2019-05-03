#region Using Statements

using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

using ResolutionBuddy;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Graphics;

using CrawlIT.Shared.Entity;
using CrawlIT.Shared.GameState;

using Camera = CrawlIT.Shared.Camera.Camera;
using Color = Microsoft.Xna.Framework.Color;
using XnaMediaPlayer = Microsoft.Xna.Framework.Media.MediaPlayer;
using InputManager = CrawlIT.Shared.Input.InputManager;
using Point = Microsoft.Xna.Framework.Point;

#endregion

namespace CrawlIT
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class CrawlIt : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private TouchCollection _touchCollection;

        private readonly IResolution _resolution;

        private InputManager _inputManager;

        private float _zoom;
        private Rectangle _touch;

        private enum State
        {
            Menu,
            Playing,
            Fighting
        }

        private Camera _staticCamera;
        private Song _backgroundSong;

        private TiledMap _map;
        private TiledMapRenderer _mapRenderer;
        private RenderTarget2D _mapRenderTarget;

        private Player _player;
        private Enemy _tutor;
        private Camera _playerCamera;
        private Texture2D _playerTexture;
        private Texture2D _tutorTexture;

        private GameState _menu;
        private GameState _level;
        private GameState _fight;

        private Point _startSize;
        private Texture2D _startButton;
        private Point _exitSize;
        private Texture2D _exitButton;
        private Point _pauseSize;
        private Texture2D _pauseButton;

        private SpriteFont _font;

        private List<Enemy> _enemies;

        public CrawlIt()
        {
            _graphics = new GraphicsDeviceManager(this)
            {   // Force orientation to be fullscreen portrait
                SupportedOrientations = DisplayOrientation.Portrait,
                IsFullScreen = true,
            };

            _resolution = new ResolutionComponent(this, _graphics,
                                                  new Point(720, 1280), new Point(720, 1280),
                                                  true, false);

            TouchPanel.EnabledGestures = GestureType.Pinch
                                         | GestureType.PinchComplete;

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
            _zoom = _graphics.PreferredBackBufferHeight > 1280 ? 6.0f : 3.0f;

            _menu = new Menu(GraphicsDevice, _zoom);
            _menu.SetState(State.Menu);

            _level = new Level(GraphicsDevice);
            _level.SetState(State.Playing);

            _fight = new Fight(GraphicsDevice);
            _fight.SetState(State.Fighting);

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

            _tutorTexture = Content.Load<Texture2D>("Sprites/tutorspritesheet");
            _tutor = new Enemy(_tutorTexture, _resolution.TransformationMatrix(), 600, 80, 3);

            _startButton = Content.Load<Texture2D>("Buttons/start");
            _exitButton = Content.Load<Texture2D>("Buttons/exit");
            _pauseButton = Content.Load<Texture2D>("Buttons/pause");

            _startSize = new Point(_startButton.Width * (int)_zoom,
                                   _startButton.Height * (int)_zoom);
            _exitSize = new Point(_exitButton.Width * (int)_zoom,
                                  _exitButton.Height * (int)_zoom);
            _pauseSize = new Point(_pauseButton.Width, _pauseButton.Height);

            _font = Content.Load<SpriteFont>("Fonts/File");

            _staticCamera = new Camera(0, 0, 1.0f);

            // Fetching list of collision objects in the map to check for collision
            _player.CollisionObjects = _map.ObjectLayers[0].Objects
                                                       .Select(o => new Rectangle(
                                                                    (int) o.Position.X, (int) o.Position.Y,
                                                                    (int) o.Size.Width, (int) o.Size.Height))
                                                       .ToList();

            // Making list of collision enemies to check for combat
            _enemies = new List<Enemy>();
            _enemies.Add(_tutor);
            _player.Enemies = _enemies;
            foreach (var enemy in _enemies)
            {
                _player.CollisionObjects.Add(enemy.CollisionRectangle);
            }
                

            _inputManager = new InputManager(_playerCamera);

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
                Activity.MoveTaskToBack(true);
#endif

            }

            GameStateManager.Instance.Update(gameTime);

            _touchCollection = TouchPanel.GetState();

            if (_touchCollection.Count > 0)
                _touch = new Rectangle((int)_touchCollection[0].Position.X,
                                       (int)_touchCollection[0].Position.Y,
                                       5, 5);

            if (GameStateManager.Instance.IsState(State.Menu))
            {
                var start = new Rectangle(_menu.GetPosition(_startButton), _startSize);
                if (_touch.Intersects(start))
                    GameStateManager.Instance.ChangeScreen(_level);

                // TODO: remove exit button altogether (bad practice for mobile development)
                var exit = new Rectangle(_menu.GetPosition(_exitButton), _exitSize);
#if !__IOS__
                if (_touch.Intersects(exit))
                    Activity.MoveTaskToBack(true);
#endif
            }

            if (GameStateManager.Instance.IsState(State.Playing))
            {
                _mapRenderer.Update(_map, gameTime);

                _inputManager.Update(gameTime);
                
                _player.UpdateMovement(gameTime, _inputManager.CurrentInputState);
                _player.Update(gameTime);
                
                _tutor.Update(gameTime);

                _playerCamera.Follow(_player);
                _staticCamera.Follow(null);

                var pause = new Rectangle(_level.GetPosition(_pauseButton), _pauseSize);
                if (_touch.Intersects(pause))
                    GameStateManager.Instance.AddScreen(_fight);

                // Launch fight screen if player collides with ennemy
                foreach (var enemy in _player.Enemies)
                {
                    if (_player.Collides(enemy.CollisionRectangle) && enemy.Rounds > 0)
                    {
                        //GameStateManager.Instance.AddScreen(_fight);
                        //enemy.Rounds--;
                    }
                    else
                    {
                        //print I have no more questions for you!
                    }
                }
            }

            if (GameStateManager.Instance.IsState(State.Fighting))
            {
                var pause = new Rectangle(_fight.GetPosition(_pauseButton), _pauseSize);
                if (_touch.Intersects(pause))
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

            if (GameStateManager.Instance.IsState(State.Playing))
            {
                #region Drawing Map

                // Little trick to show the tiled map as PointClamp even though we don't use spritebatch to draw it
                GraphicsDevice.SetRenderTarget(_mapRenderTarget);
                GraphicsDevice.BlendState = BlendState.AlphaBlend;
                GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
                GraphicsDevice.RasterizerState = RasterizerState.CullNone;

                _mapRenderer.Draw(_map, viewMatrix: _playerCamera.Transform);

                GraphicsDevice.SetRenderTarget(null);
                _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                var destinationRectangle = new Rectangle(0, 0,
                                                         _graphics.PreferredBackBufferWidth,
                                                         _graphics.PreferredBackBufferHeight);
                _spriteBatch.Draw(_mapRenderTarget, destinationRectangle, Color.White);
                _spriteBatch.End();
                // End of little trick

                #endregion

                #region Drawing Player

                _spriteBatch.Begin(SpriteSortMode.BackToFront,
                                   BlendState.AlphaBlend,
                                   SamplerState.PointClamp,
                                   null, null, null,
                                   _playerCamera.Transform);
                _tutor.Draw(_spriteBatch);
                _player.Draw(_spriteBatch);
                _spriteBatch.End();

                #endregion

                #region Drawing Ennemies

                #endregion

                // Saving this here for future reference...
                /*_spriteBatch.Begin(SpriteSortMode.BackToFront,
                                  BlendState.AlphaBlend,
                                  null, null, null, null,
                                  _staticCamera.Transform);*/
            }

            #region FPS Counter

            var fps = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
            var fpsString = "FPS: " + Math.Ceiling(fps);
            var (stringDimensionX, stringDimensionY) = _font.MeasureString(fpsString);

            var stringPosX = (_graphics.PreferredBackBufferWidth - stringDimensionX) / 2;
            var stringPosY = _graphics.PreferredBackBufferHeight - stringDimensionY;

            if (!GameStateManager.Instance.IsState(State.Fighting))
            {
                _spriteBatch.Begin();
                _spriteBatch.DrawString(_font, fpsString, new Vector2(stringPosX, stringPosY), Color.Black);
                _spriteBatch.End();
            }
            
            #endregion

            base.Draw(gameTime);
        }
    }
}
