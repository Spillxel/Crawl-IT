using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

using ResolutionBuddy;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
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
        private Point _virtualResolution;
        private Point _realResolution;
        private Matrix _transform;

        private InputManager _inputManager;

        private float _scale;
        private Rectangle? _gameTouchRectangle;
        private Rectangle? _mapTouchRectangle;

        private Song _backgroundSong;

        private SplashScreen _splashScreen;
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

        public List<Entity> Entities { get; private set; }
        private Player _player;
        private Texture2D _playerTexture;
        private Tutor _tutor;
        private Enemy _firstAssistant;
        private Enemy _secondAssistant;
        private Enemy _thirdAssistant;
        private Boss _mathBoss;

        private Rectangle _startButton;
        private Point _startButtonSize;
        private Texture2D _startButtonTexture;

        // combat flags
        private bool _hasAnswered;
        private bool _hasUsedCrystal;

        private SpriteFont _font;

        private bool _fightTrigger;
        private double _fightTransitionTimer = 0;

        private float _timer = 5;
        private const float Timer = 5;

        // TODO: make do better be
        private bool _menuState = false;

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
                                                           true, false, null);

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

            LoadMap();
            LoadEntities();
            LoadCollision();
            LoadUi();

            _inputManager = new InputManager(_playerCamera);

            _backgroundSong = Content.Load<Song>("Audio/Investigations");
        }

        /// <summary>
        /// Allows the game to initialize components it may use for various reasons.
        /// <para/>This function should be used to initialize anything that does not really
        /// belong inside <see cref="LoadContent"/>.
        /// </summary>
        private void InitializeComponents()
        {
            _realResolution = new Point(_graphics.PreferredBackBufferWidth,
                                        _graphics.PreferredBackBufferHeight);
            // Set _virtualResolution to actual drawn screen size
            _virtualResolution = _resolutionComponent.ScreenArea.Size;

            _transform = _resolutionComponent.TransformationMatrix();
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Define scale for virtual rendering
            var scaleX = Math.Max(_realResolution.X / (float)_virtualResolution.X, 3);
            var scaleY = Math.Max(_realResolution.Y / (float)_virtualResolution.Y, 3);
            _scale = Math.Min(scaleX, scaleY);

            Entities = new List<Entity>();

            // Repeat _backgroundSong on end
            XnaMediaPlayer.IsRepeating = true;
            _staticCamera = new Camera(0, 0, 1);
        }

        /// <summary>
        /// Load entity related stuff.
        /// </summary>
        private void LoadEntities()
        {
            // TODO: keep list of entities for game, rather than separate instances for each...
            var entityData = _map.GetLayer<TiledMapObjectLayer>("Entity")
                .Objects
                .ToDictionary(obj => obj.Name,  obj => (position: obj.Position, frame: obj.Size.ToPoint()));

            // TODO: unify content loading into content manager class or smth...
            _playerTexture = Content.Load<Texture2D>("Sprites/playerspritesheet");
            _player = new Player(
                _playerTexture,
                _transform,
                entityData["Player"].position, entityData["Player"].frame);

            _playerCamera = new Camera(_virtualResolution.X,
                                       _virtualResolution.Y,
                                       _scale);

            // TODO: probably possible to simplify this even further.
            _tutor = new Tutor(
                Content, "Sprites/tutorspritesheet", "Sprites/tutorcloseup",
                entityData["Tutor"].position, entityData["Tutor"].frame,
                100, 1);
            _firstAssistant = new Enemy(
                Content, "Sprites/assistantspritesheet1", "Sprites/assistant1closeup",
                entityData["FirstAssistant"].position, entityData["FirstAssistant"].frame,
                10, 2);
            _secondAssistant = new Enemy(
                Content, "Sprites/assistantspritesheet2", "Sprites/assistant2closeup",
                entityData["SecondAssistant"].position, entityData["SecondAssistant"].frame, 
                10, 2);
            _thirdAssistant = new Enemy(
                Content, "Sprites/assistantspritesheet3", "Sprites/assistant3closeup",
                entityData["ThirdAssistant"].position, entityData["ThirdAssistant"].frame,
                10, 2);
            _mathBoss = new Boss(
                Content, "Sprites/mathsteacherspritesheet", "Sprites/mathsteachercloseup",
                entityData["MathBoss"].position, entityData["MathBoss"].frame,
                100, 3);

            _player.Enemies.AddRange(new List<Enemy>
            {
                _tutor,
                _firstAssistant,
                _secondAssistant,
                _thirdAssistant,
                _mathBoss
            });

            Entities.Add(_player);
        }

        /// <summary>
        /// Load collisions.
        /// </summary>
        private void LoadCollision()
        {
            // Add a rectangle with position & size of each collision object in the map to the CollisionObjects list
            _map.GetLayer<TiledMapObjectLayer>("Collision")
                .Objects
                .ToList()
                .ForEach(collisionObject => _player.CollisionObjects.Add(collisionObject.ToRectangle()));

            // Same for each enemy
            _player.Enemies.ForEach(enemy => _player.CollisionObjects.Add(enemy.CollisionRectangle));
        }

        /// <summary>
        /// Load map related stuff.
        /// </summary>
        private void LoadMap()
        {
            _map = Content.Load<TiledMap>("Maps/test");

            _mapRenderer = new TiledMapRenderer(GraphicsDevice, _map);

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
            _splashScreen = new SplashScreen(GraphicsDevice, _virtualResolution, _transform, _scale);
            _menu = new Menu(GraphicsDevice, _virtualResolution, _transform, _scale);
            _level = new Level(GraphicsDevice);
            _fight = new Fight(GraphicsDevice, _virtualResolution, _transform, _player);

            _explorationUi = new ExplorationUi(_transform, _scale, _virtualResolution, Content, _player);
            _explorationUi.Load();

            // Initialize GameStateManager to be able to use it
            GameStateManager.Instance.Init(GraphicsDevice, Content, _virtualResolution, _transform);
            // Initialize by adding the Menu screen into the game
            GameStateManager.Instance.AddScreen(_splashScreen);
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
            if (_touchCollection.Count > 0 && _touchCollection.First().State == TouchLocationState.Released)
            {
                ScreenToGameTouchTransform(out _gameTouchRectangle);
                ScreenToMapTouchTransform(out _mapTouchRectangle);
            }
            else
            {
                _gameTouchRectangle = null;
                _mapTouchRectangle = null;
            }

            // TODO: simplify stuff inside of here
            switch (GameStateManager.Instance.State)
            {
                case GameState.StateType.Splash:
                    if (gameTime.TotalGameTime.Seconds > 3)
                    {
                        GameStateManager.Instance.ChangeScreen(_menu);
                    }

                    _splashScreen.Update(gameTime);
                    break;
                case GameState.StateType.Menu:
                    if (!_menuState)
                    {
                        _menuState = true;
                        XnaMediaPlayer.Play(_backgroundSong);
                    }

                    // TODO: replace with InputManager, able to handle touch within levels then
                    if (_gameTouchRectangle != null)
                        if (_gameTouchRectangle.Value.Intersects(_menu.NewGameRectangle))
                            GameStateManager.Instance.ChangeScreen(_level);
                    break;
                case GameState.StateType.Playing:
                    if (IsFightTriggered(gameTime))
                    {
                        UpdateMap(gameTime);
                        UpdateCharacters(gameTime);
                    }
                    else
                    {
                        UpdateMap(gameTime);
                        UpdatePlayerMovement(gameTime);
                        UpdateCharacters(gameTime);
                        UpdateUi(gameTime);
                        UpdateFightTrigger(gameTime);
                    }
                    break;
                case GameState.StateType.Fighting:
                    if (_hasAnswered)
                    {
                        var elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
                        _timer -= elapsed;
                        if (_timer < 0)
                        {
                            GameStateManager.Instance.RemoveScreen();
                            _hasAnswered = false;
                            _hasUsedCrystal = false;
                            _timer = Timer;
                        }
                    }
                    else if (_gameTouchRectangle is Rectangle gameTouchRectangle)
                    {
                        if (gameTouchRectangle.Intersects(_fight.CrystalRectangle))
                        {
                            _hasUsedCrystal = true;
                            _player.SetCrystalCount(--_player.CrystalCount);
                        }
                        else if (gameTouchRectangle.Intersects(_fight.AnswerRectangle))
                        {
                            _fight.CheckAnswer(gameTouchRectangle);
                            _hasAnswered = true;
                        }
                    }

                    _fight.Update(gameTime);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            base.Update(gameTime);
        }

        private bool IsFightTriggered(GameTime gameTime)
        {
            if (!_fightTrigger) return false; // Fight not triggered
            // Update fight timer
            _fightTransitionTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (!(_fightTransitionTimer > 1000)) return true; // Fight timer not finished
            _fightTrigger = false;
            _fightTransitionTimer = 0;
            // TODO: For loop with enemy.QuestionPerFight
            _fight.QuestionCurrentAnimation = _fight.NoAnswer;
            GameStateManager.Instance.AddScreen(_fight);
            // TODO: move line below to Fight.cs somehow
            _fight.Enemy.Fights--;
            return true;
        }

        private void UpdateMap(GameTime gameTime)
        {
            _mapRenderer.Update(gameTime);
        }

        private void UpdateCharacters(GameTime gameTime)
        {
            // HACK: temp fix for updating every enemy
            _player.Enemies.ForEach(enemy => enemy.Update(gameTime));

            Entities.ForEach(entity => entity.Update(gameTime));

            _playerCamera.Follow(_player);
            _staticCamera.Follow(null);
        }

        private void UpdateUi(GameTime gameTime)
        {
            _explorationUi.Update(gameTime);
        }

        private void UpdatePlayerMovement(GameTime gameTime)
        {
            _inputManager.Update(gameTime);
            _player.UpdateMovement(gameTime, _inputManager.CurrentInputState);
        }

        private void UpdateFightTrigger(GameTime gameTime)
        {
            if (_mapTouchRectangle == null) return; // No touch, no check
            foreach (var enemy in _player.Enemies)
            {
                if (_mapTouchRectangle.Value.Intersects(enemy.FightRectangle)
                    && enemy.Fights > 0
                    && _player.Collides(enemy.FightZoneRectangle))
                {
                    _fight.Enemy = enemy;
                    _fightTrigger = true;
                }
                else
                {
                    // TODO: Display TextBox "I have no more questions for you!"
                }
            }
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
                case GameState.StateType.Splash:
                    break;
                case GameState.StateType.Menu:
                    // TODO: Will need to redraw map here once we animate stuff in there
                    break;
                case GameState.StateType.Playing:
                    DrawMap();
                    DrawCharacters();
                    DrawUi();
                    break;
                case GameState.StateType.Fighting:
                    DrawFight();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

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

            _mapRenderer.Draw(_playerCamera.Transform);

            GraphicsDevice.SetRenderTarget(null);
            _spriteBatch.Begin(transformMatrix: _transform, samplerState: SamplerState.PointClamp);
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
            
            // HACK: temp fix to make enemies dissapear once their fights are completed
            _player.Enemies.ForEach(enemy => enemy.Draw(_spriteBatch));
            
            Entities.ForEach(entity => entity.Draw(_spriteBatch));

            // TODO: this can surely be simplified, too...
            _player.Enemies.ForEach(
                enemy => enemy.DrawActionIcons(
                    _spriteBatch,
                    enemy.FightZoneRectangle.Intersects(_player.CollisionRectangle)));
           
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

        private void DrawFight()
        {
            if (_hasUsedCrystal)
                _fight.Help(_spriteBatch);

            if (!_hasAnswered) return;

            _fight.ChangeColour(_spriteBatch);

            if (!(_timer < 2)) return;

            GameStateManager.Instance.FadeBackBufferToBlack(_spriteBatch);
            _fight.PopUp(_spriteBatch);
        }

        /// <summary>
        /// If there is a touch input, produces a Rectangle with the scaled touch location.
        /// Otherwise null.
        /// </summary>
        /// <param name="gameTouchRectangle"></param>
        private void ScreenToGameTouchTransform(out Rectangle? gameTouchRectangle)
        {
            var touchPoint = _resolutionComponent.ScreenToGameCoord(_touchCollection[0].Position)
                                                 .ToPoint();
            gameTouchRectangle = new Rectangle(touchPoint, new Point(5, 5));
        }

        /// <summary>
        /// If there is a touch input, produces a Rectangle with the map coordinates.
        /// Otherwise null.
        /// </summary>
        /// <param name="mapTouchRectangle"></param>
        private void ScreenToMapTouchTransform(out Rectangle? mapTouchRectangle)
        {
            var touchVector = _resolutionComponent.ScreenToGameCoord(_touchCollection[0].Position);
            var mapVector = Vector2.Transform(touchVector, Matrix.Invert(_playerCamera.Transform));

            mapTouchRectangle = new Rectangle(mapVector.ToPoint(), new Point(5, 5));
        }
    }
}
