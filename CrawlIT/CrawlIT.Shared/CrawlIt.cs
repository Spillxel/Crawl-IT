using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

using ResolutionBuddy;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Graphics;

using Color = Microsoft.Xna.Framework.Color;
using XnaMediaPlayer = Microsoft.Xna.Framework.Media.MediaPlayer;
using Point = Microsoft.Xna.Framework.Point;

namespace CrawlIT.Shared
{
    /// <summary>
    /// This is the main type for Crawl IT
    /// </summary>
    public class CrawlIt : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private TouchCollection _touchCollection;

        private readonly IResolution _resolutionComponent;
        private readonly Point _virtualResolution;
        private readonly Point _realResolution;
        private Matrix _transform;

        private InputManager _inputManager;

        private float _scale;
        private Rectangle _touch;

        private Song _backgroundSong;
        private SpriteFont _font;

        private Menu _menu;
        private Level _level;
        private Fight _fight;
        private ExplorationUi _explorationUi;

        private TiledMap _map;
        private TiledMapRenderer _mapRenderer;
        private RenderTarget2D _mapRenderTarget;
        private Rectangle _mapRectangle;

        private Camera _playerCamera;
        private Camera _staticCamera;

        private Player _player;
        private Texture2D _playerTexture;
        private Enemy _tutor;
        private Texture2D _tutorTexture;

        private Rectangle _startButton;
        private Point _startButtonSize;
        private Texture2D _startButtonTexture;
        
        private bool _played;

        public CrawlIt()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                // Force orientation to be fullscreen portrait
                SupportedOrientations = DisplayOrientation.Portrait,
                IsFullScreen = true,
            };

            _resolutionComponent = new ResolutionComponent(this, _graphics,
                                                           new Point(720, 1280),
                                                           new Point(720, 1280),
                                                           true, true);

            _realResolution = new Point(_graphics.PreferredBackBufferWidth,
                                        _graphics.PreferredBackBufferHeight);
            _virtualResolution = _resolutionComponent.VirtualResolution;

            TouchPanel.EnabledGestures = GestureType.Pinch
                                         | GestureType.PinchComplete;

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            InitializeComponents();

            LoadPlayer();
            LoadEnemies();
            LoadMap();
            LoadCollision();
            LoadUi();

            _inputManager = new InputManager(_playerCamera);
            
            _backgroundSong = Content.Load<Song>("Audio/Investigations");
            XnaMediaPlayer.Play(_backgroundSong);
        }

        /// <summary>
        /// Allows the game to initialize components it may use for various reasons.
        /// <para/>This function should be used to initialize anything that does not really
        /// belong inside <see cref="LoadContent"/>.
        /// </summary>  
        private void InitializeComponents()
        {
            _transform = _resolutionComponent.TransformationMatrix();
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Define scale for virtual rendering
            var scaleX = Math.Max(_realResolution.X / (float) _virtualResolution.X, 3);
            var scaleY = Math.Max(_realResolution.Y / (float) _virtualResolution.Y, 3);
            _scale = Math.Min(scaleX, scaleY);

            // Repeat _backgroundSong on end
            XnaMediaPlayer.IsRepeating = true;

            _staticCamera = new Camera(0, 0, 1);
        }

        /// <summary>
        /// Load player related stuff.
        /// </summary>
        private void LoadPlayer()
        {
            _playerTexture = Content.Load<Texture2D>("Sprites/playerspritesheet");
            _player = new Player(_playerTexture, _transform);
            _playerCamera = new Camera(_virtualResolution.X,
                                       _virtualResolution.Y,
                                       _scale);
        }

        /// <summary>
        /// Load all the enemies.
        /// </summary>
        private void LoadEnemies()
        {
            _tutorTexture = Content.Load<Texture2D>("Sprites/tutorspritesheet");
            _tutor = new Enemy(_tutorTexture, 600, 80, 10);

            
            _player.Enemies = new List<Enemy>
            {
                _tutor,
            };
        }

        /// <summary>
        /// Load collisions.
        /// </summary>
        private void LoadCollision()
        {
            // List of collision objects in the map
            _player.CollisionObjects = _map.ObjectLayers[0]
                                           .Objects
                                           .Select(o => new Rectangle((int)o.Position.X, (int)o.Position.Y,
                                                                      (int)o.Size.Width, (int)o.Size.Height))
                                           .ToList();

            // Add enemies to CollisionObjects
            foreach (var enemy in _player.Enemies)
                _player.CollisionObjects.Add(enemy.CollisionRectangle);
        }

        /// <summary>
        /// Load map related stuff.
        /// </summary>
        private void LoadMap()
        {
            _map = Content.Load<TiledMap>("Maps/test");

            _mapRenderer = new TiledMapRenderer(GraphicsDevice);

            // Define render target to draw map to our virtual resolution
            // TODO: find out if this is necessary
            var pp = _graphics.GraphicsDevice.PresentationParameters;
            _mapRenderTarget = new RenderTarget2D(GraphicsDevice,
                                                  _virtualResolution.X,
                                                  _virtualResolution.Y,
                                                  false, SurfaceFormat.Color,
                                                  DepthFormat.None,
                                                  pp.MultiSampleCount,
                                                  RenderTargetUsage.DiscardContents);

            _mapRectangle = new Rectangle(0, 0, _virtualResolution.X, _virtualResolution.Y);
        }

        /// <summary>
        /// Load anything related to Menus/UI.
        /// </summary>
        private void LoadUi()
        {
            _menu = new Menu(GraphicsDevice, _virtualResolution, _transform);
            _menu.Initialize();
            _menu.LoadContent(Content);
            _level = new Level(GraphicsDevice);
            _level.LoadContent(Content);
            _level.Initialize();
            _fight = new Fight(GraphicsDevice, _virtualResolution, _transform,  _player);
            _fight.Initialize();
            _fight.LoadContent(Content);

            _explorationUi = new ExplorationUi(_transform, _scale, _virtualResolution, Content, _player);
            _explorationUi.Load();

            // Set the content to the GameStateManager to be able to use it
            GameStateManager.Instance.SetContent(Content);
            // Initialize by adding the Menu screen into the game
            GameStateManager.Instance.AddScreen(_menu);

            // Buttons
            _startButtonTexture = Content.Load<Texture2D>("Buttons/start");
            _startButtonSize = new Point(_startButtonTexture.Width, _startButtonTexture.Height);
            _startButton = new Rectangle(_menu.StartButtonPoint, _startButtonSize);
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
                UpdateTouch(out _touch);

            // TODO: simplify stuff inside of here
            switch (GameStateManager.Instance.State)
            {
                case GameState.StateType.Menu:
                    if (_touch.Intersects(_startButton))
                        GameStateManager.Instance.ChangeScreen(_level);
                    break;
                case GameState.StateType.Playing:
                    _mapRenderer.Update(_map, gameTime);

                    _inputManager.Update(gameTime);

                    _player.UpdateMovement(gameTime, _inputManager.CurrentInputState);
                    _player.Update(gameTime);

                    _tutor.Update(gameTime);

                    _playerCamera.Follow(_player);
                    _staticCamera.Follow(null);

                    foreach (var enemy in _player.Enemies)
                    {
                        if (_player.Collides(enemy.FightRectangle) && enemy.Rounds > 0)
                        {
                            Thread.Sleep(2000);
                            GameStateManager.Instance.AddScreen(_fight);
                            enemy.Rounds--;
                        }
                        else
                        {
                            // Display TextBox "I have no more questions for you!"
                        }
                    }

                    _explorationUi.Update(gameTime);
                    break;
                case GameState.StateType.Fighting:
                    if (_played)
                    {
                        _played = false;
                        Thread.Sleep(5000);
                        GameStateManager.Instance.RemoveScreen();
                        _player.MoveBack(_tutor);
                    }
                    _fight.Update(gameTime);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// If there is a touch input, produces a Rectangle with the scaled touch location.
        /// Otherwise null.
        /// </summary>
        /// <param name="touchRectangle"></param>
        private void UpdateTouch(out Rectangle touchRectangle)
        {
            var touchPoint = _resolutionComponent.ScreenToGameCoord(_touchCollection[0].Position)
                                                 .ToPoint();
            touchRectangle = new Rectangle(touchPoint, new Point(5, 5));
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateGray);
            GameStateManager.Instance.Draw(_spriteBatch);

            var gameState = GameStateManager.Instance.State;
            switch (gameState)
            {
                case GameState.StateType.Menu:
                    // TODO: Will need to redraw map here once we animate stuff in there
                    break;
                case GameState.StateType.Playing:
                    DrawMap();
                    DrawCharacters();
                    DrawUi();
                    break;
                case GameState.StateType.Fighting:
                    // TODO: make this better
                    // it's just straight up a weird way to do what it does
                    if (_touch.Intersects(_fight.CrystalRectangle))
                    {
                        _fight.Help(_spriteBatch);
                    }
                    else if (_touch.Intersects(_fight.AnswerRectangle))
                    {
                        _fight.CheckAnswer(_touch);
                        _fight.ChangeColour(_spriteBatch);
                        _played = true;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            // FPS
            //var fps = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
            //var fpsString = "FPS: " + Math.Ceiling(fps);
            //var (stringDimensionX, stringDimensionY) = _font.MeasureString(fpsString);

            //var stringPosX = (_graphics.PreferredBackBufferWidth - stringDimensionX) / 2;
            //var stringPosY = _graphics.PreferredBackBufferHeight - stringDimensionY;

            //if (!GameStateManager.Instance.IsState(GameState.StateType.Fighting))
            //{
            //    _spriteBatch.Begin();
            //    _spriteBatch.DrawString(_font, fpsString, new Vector2(stringPosX, stringPosY), Color.Red);
            //    _spriteBatch.End();
            //}

            base.Draw(gameTime);
        }

        /// <summary>
        /// Draw anything map related.
        /// </summary>
        private void DrawMap()
        {
            // Little trick to show the tiled map as PointClamp,
            // even though we don't use SpriteBatch to draw it
            GraphicsDevice.SetRenderTarget(_mapRenderTarget);
            GraphicsDevice.BlendState = BlendState.AlphaBlend;
            GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            GraphicsDevice.RasterizerState = RasterizerState.CullNone;

            _mapRenderer.Draw(_map, _playerCamera.Transform);

            GraphicsDevice.SetRenderTarget(null);
            _spriteBatch.Begin(transformMatrix:_transform, samplerState: SamplerState.PointClamp);
            _spriteBatch.Draw(_mapRenderTarget, _mapRectangle, Color.White);
            _spriteBatch.End();
        }

        /// <summary>
        /// Draw all characters.
        /// </summary>
        private void DrawCharacters()
        {
            // We draw characters in the playerCamera transform, scaled to our virtual transform
            _spriteBatch.Begin(transformMatrix: _playerCamera.Transform * _transform,
                               samplerState: SamplerState.PointClamp);
            _tutor.Draw(_spriteBatch);
            _player.Draw(_spriteBatch);
            _spriteBatch.End();
        }

        /// <summary>
        /// Draw UI elements.
        /// </summary>
        private void DrawUi()
        {
            // Might get more lines in the future, lol
            _explorationUi.Draw(_spriteBatch);
        }
    }
}
