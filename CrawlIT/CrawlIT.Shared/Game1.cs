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

        enum _gameStates
        {
            Playing,
            Fighting,
            Menu
        }

        private SpriteFont _font;

        private TiledMap _map;
        private TiledMapRenderer _mapRenderer;

        private float _zoom;

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
            _zoom = _graphics.PreferredBackBufferHeight > 1280
                ? 6.0f
                : 3.0f;

            _menu = new Menu(GraphicsDevice, _zoom);
            _menu.SetState(_gameStates.Menu);

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
                                       8.0f);

            _startButton = Content.Load<Texture2D>(@"start");
            _exitButton = Content.Load<Texture2D>(@"exit");
            _pauseButton = Content.Load<Texture2D>(@"pause");

            _startSize = new Point((int)_startButton.Width * (int)_zoom, 
                                   (int)_startButton.Height * (int)_zoom);

            _exitSize = new Point((int)_exitButton.Width * (int)_zoom,
                                  (int)_exitButton.Height * (int)_zoom);

            _pauseSize = new Point((int)_pauseButton.Width,
                                   (int)_pauseButton.Height);

            _font = Content.Load<SpriteFont>("Fonts/File");

            _staticCamera = new Camera(0, 0, 1.0f);

            //Set the content to the GameStateManager to be able to use it
            GameStateManager.Instance.SetContent(Content);

            //Initialize by adding the Menu screen into the game
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
           
            if (GameStateManager.Instance.IsState(_gameStates.Menu) && _touchCollection.Count > 0)
            {
                Rectangle _start = new Rectangle(_menu.GetPosition(_startButton), _startSize);

                Rectangle _exit = new Rectangle(_menu.GetPosition(_exitButton), _exitSize);

                Rectangle _touch = new Rectangle((int)_touchCollection[0].Position.X,
                                                 (int)_touchCollection[0].Position.Y, 5, 5);

                if(_touch.Intersects(_start))
                {
                    GameStateManager.Instance.ChangeScreen(_level);
                }

                if(_touch.Intersects(_exit))
                {
                    Game.Activity.MoveTaskToBack(true);
                }
            }

            if (GameStateManager.Instance.IsState(_gameStates.Playing) && _touchCollection.Count > 0)
            {
                _mapRenderer.Update(_map, gameTime);
                _player.Update(gameTime);

                _playerCamera.Follow(_player);
                _staticCamera.Follow(null);

                Rectangle _pause = new Rectangle(_level.GetPosition(_pauseButton), _pauseSize);

                Rectangle _touch = new Rectangle((int)_touchCollection[0].Position.X,
                                                 (int)_touchCollection[0].Position.Y, 5, 5);

                if (_touch.Intersects(_pause))
                {
                    GameStateManager.Instance.ChangeScreen(_fight);
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

            if (GameStateManager.Instance.IsState(_gameStates.Playing))
            {
                _mapRenderer.Draw(_map, viewMatrix: _playerCamera.Transform);
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
            var stringDimensions = _font.MeasureString(fpsString);

            var stringPosX = _graphics.PreferredBackBufferWidth / 2 - stringDimensions.X / 2;
            var stringPosY = _graphics.PreferredBackBufferHeight - stringDimensions.Y;

            _spriteBatch.Begin();
            _spriteBatch.DrawString(_font, fpsString, new Vector2(stringPosX, stringPosY), Color.Black);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
