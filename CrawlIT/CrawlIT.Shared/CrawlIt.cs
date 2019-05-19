#region Using Statements

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

#endregion

namespace CrawlIT.Shared
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
        private Matrix _transform;
        private readonly Point _virtualResolution;
        private readonly Point _realResolution;

        private InputManager _inputManager;

        private float _scale;
        private Rectangle _touch;

        private enum State
        {
            Menu,
            Playing,
            Fighting
        }

        private Camera _playerCamera;
        private Camera _staticCamera;
        private Song _backgroundSong;

        private TiledMap _map;
        private TiledMapRenderer _mapRenderer;
        private RenderTarget2D _mapRenderTarget;

        private Player _player;
        private Enemy _tutor;

        private Texture2D _playerTexture;
        private Texture2D _tutorTexture;

        private Menu _menu;
        private Level _level;
        private Fight _fight;

        private Point _startSize;
        private Texture2D _startButton;
        private Point _exitSize;
        private Texture2D _exitButton;
        private Point _pauseSize;
        private Texture2D _pauseButton;
        private Point _answerSize;
        private Texture2D _answerButton;
        private Point _surgeCrystalSize;
        private Texture2D _surgeCrystalTexture;

        private bool _win;
        private bool _played;

        private SpriteFont _font;

        private List<Enemy> _enemies;

        private ExplorationUi _explorationUi;

        public CrawlIt()
        {
            _graphics = new GraphicsDeviceManager(this)
            {
                // Force orientation to be fullscreen portrait
                SupportedOrientations = DisplayOrientation.Portrait,
                IsFullScreen = true,
            };

            _realResolution = new Point(_graphics.PreferredBackBufferWidth,
                                        _graphics.PreferredBackBufferHeight);
            _resolution = new ResolutionComponent(this, _graphics,
                                                  new Point(720, 1280),
                                                  new Point(720, 1280),
                                                  true, true);
            _virtualResolution = _resolution.VirtualResolution;

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
            _transform = _resolution.TransformationMatrix();

            // TODO: comment this scaling stuff
            var scaleX = Math.Max(_realResolution.X / (float) _virtualResolution.X, 3);
            var scaleY = Math.Max(_realResolution.Y / (float) _virtualResolution.Y, 3);
            _scale = Math.Min(scaleX, scaleY);

            _menu = new Menu(GraphicsDevice, _virtualResolution, _transform);
            _menu.SetState(State.Menu);

            _staticCamera = new Camera(0, 0, 1);

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _level = new Level(GraphicsDevice);
            _level.SetState(State.Playing);

            _win = false;
            _played = false;

            // TODO: make this work
            // _gameStateManager.GameState = GameState.StartMenu;

            _mapRenderer = new TiledMapRenderer(GraphicsDevice);

            // Repeat _backgroundSong on end
            XnaMediaPlayer.IsRepeating = true;


            var pp = _graphics.GraphicsDevice.PresentationParameters;
            _mapRenderTarget = new RenderTarget2D(GraphicsDevice,
                _virtualResolution.X,
                _virtualResolution.Y,
                false, SurfaceFormat.Color,
                DepthFormat.None,
                pp.MultiSampleCount,
                RenderTargetUsage.DiscardContents);

            //_spriteBatch = new _spriteBatch(_resolution);

            _backgroundSong = Content.Load<Song>("Audio/Investigations");
            XnaMediaPlayer.Play(_backgroundSong);

            _map = Content.Load<TiledMap>("Maps/test");

            _playerTexture = Content.Load<Texture2D>("Sprites/playerspritesheet");
            _player = new Player(_playerTexture, _transform);
            _playerCamera = new Camera(_virtualResolution.X,
                                       _virtualResolution.Y,
                                       _scale);
            
            _explorationUi = new ExplorationUi(_transform, _scale, _virtualResolution, Content, _player);

            _fight = new Fight(GraphicsDevice, _player);
            _fight.SetState(State.Fighting);

            _tutorTexture = Content.Load<Texture2D>("Sprites/tutorspritesheet");
            _tutor = new Enemy(_tutorTexture, _transform, 600, 80, 10);

            _startButton = Content.Load<Texture2D>("Buttons/start");
            _exitButton = Content.Load<Texture2D>("Buttons/exit");
            _pauseButton = Content.Load<Texture2D>("Buttons/pause");
            _answerButton = Content.Load<Texture2D>("Sprites/newscreentexture");

            _explorationUi.Load();

            _surgeCrystalTexture = Content.Load<Texture2D>("Sprites/surgecrystal");

            _startSize = new Point(_startButton.Width, _startButton.Height);
            _exitSize = new Point(_exitButton.Width, _exitButton.Height);
            _pauseSize = new Point(_pauseButton.Width, _pauseButton.Height);

            _answerSize = new Point(GraphicsDevice.Viewport.Width,
                                    GraphicsDevice.Viewport.Height / 10 * 4);
            _surgeCrystalSize = new Point(_surgeCrystalTexture.Width * GraphicsDevice.Viewport.Width / 200,
                                          _surgeCrystalTexture.Height * GraphicsDevice.Viewport.Width / 200);

            _font = Content.Load<SpriteFont>("Fonts/File");

            //_staticCamera = new Camera(0, 0, 1.0f);
            
            // Fetching list of collision objects in the map to check for collision
            _player.CollisionObjects = _map.ObjectLayers[0].Objects
                                                       .Select(o => new Rectangle(
                                                                    (int)o.Position.X, (int)o.Position.Y,
                                                                    (int)o.Size.Width, (int)o.Size.Height))
                                                       .ToList();

            // Making list of collision enemies to check for combat
            _enemies = new List<Enemy> {_tutor};
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
            {
                var (touchX, touchY) = _resolution.ScreenToGameCoord(_touchCollection[0].Position);
                Console.WriteLine($"startPos: {_menu.StartButtonPoint} screen: {_touchCollection[0].Position} game: {touchX}, {touchY}");
                _touch = new Rectangle((int) touchX, (int) touchY, 5, 5);
            }

            if (GameStateManager.Instance.IsState(State.Menu))
            {
                var start = new Rectangle(_menu.StartButtonPoint, _startSize);
                if (_touch.Intersects(start))
                    GameStateManager.Instance.ChangeScreen(_level);

                // TODO: remove exit button altogether (bad practice for mobile development)
                var exit = new Rectangle(_menu.ExitButtonPoint, _exitSize);
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

                foreach (var enemy in _enemies)
                {
                    if (_player.Collides(enemy.FightRectangle) && enemy.Rounds > 0)
                    {
                        Thread.Sleep(2000);
                        GameStateManager.Instance.AddScreen(_fight);
                        enemy.Rounds--;
                    }
                    else
                    {
                        // Display textbox "I have no more questions for you!"
                    }
                }

                _explorationUi.Update(gameTime);
                _fight.Update(gameTime);
            }

            if (GameStateManager.Instance.IsState(State.Fighting))
            {
                if (_played)
                {
                    //if(_win)
                    _played = false;
                    Thread.Sleep(5000);
                    GameStateManager.Instance.RemoveScreen();
                    _player.MoveBack(_tutor);
                }
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

                _mapRenderer.Draw(_map, _playerCamera.Transform);

                GraphicsDevice.SetRenderTarget(null);
                _spriteBatch.Begin(transformMatrix:_transform, samplerState: SamplerState.PointClamp);
                var destinationRectangle = new Rectangle(0, 0, _virtualResolution.X, _virtualResolution.Y);
                _spriteBatch.Draw(_mapRenderTarget, destinationRectangle, Color.White);
                _spriteBatch.End();

                #endregion

                #region Drawing Characters

                _spriteBatch.Begin(transformMatrix: _playerCamera.Transform * _transform, samplerState: SamplerState.PointClamp);
                _tutor.Draw(_spriteBatch);
                _player.Draw(_spriteBatch);
                _spriteBatch.End();

                #endregion

                #region Drawing UI

                _explorationUi.Draw(_spriteBatch);

                #endregion 
            }

            if (GameStateManager.Instance.IsState(State.Fighting))
            {
                var crystal = new Rectangle(_fight.GetPosition(_surgeCrystalTexture), _surgeCrystalSize);
                var answer = new Rectangle(_fight.GetPosition(_answerButton), _answerSize);
                if (_touch.Intersects(crystal))
                    _fight.Help(_spriteBatch);
                else if (_touch.Intersects(answer) && !_touch.Intersects(crystal))
                {
                    _win |= _fight.GetAnswer(_touch);
                    _fight.ChangeColour(_spriteBatch);
                    _played = true;
                }
            }

            //#region FPS Counter

            //var fps = 1 / (float)gameTime.ElapsedGameTime.TotalSeconds;
            //var fpsString = "FPS: " + Math.Ceiling(fps);
            //var (stringDimensionX, stringDimensionY) = _font.MeasureString(fpsString);

            //var stringPosX = (_graphics.PreferredBackBufferWidth - stringDimensionX) / 2;
            //var stringPosY = _graphics.PreferredBackBufferHeight - stringDimensionY;

            //if (!GameStateManager.Instance.IsState(State.Fighting))
            //{
            //    _spriteBatch.Begin();
            //    _spriteBatch.DrawString(_font, fpsString, new Vector2(stringPosX, stringPosY), Color.Red);
            //    _spriteBatch.End();
            //}

            //#endregion

            base.Draw(gameTime);
        }
    }
}
