#region Using Statements

using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using XnaMediaPlayer = Microsoft.Xna.Framework.Media.MediaPlayer;

using ResolutionBuddy;
using Microsoft.Xna.Framework.Input.Touch;

using CrawlIT.Shared.Entity;
using CrawlIT.Shared.Camera;

using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Graphics;
using MonoGame.Extended.ViewportAdapters;

#endregion

namespace CrawlIT
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private readonly IResolution _resolution;

        private Song _backgroundSong;

        private Texture2D _playerTexture;

        private Texture2D _startButton;

        private Texture2D _exitButton;

        private Texture2D _pauseButton;

        private Texture2D _resumeButton;

        private Player _player;

        private TiledMap _map;

        private TiledMapRenderer _mapRenderer;

        //private Camera _camera;

        private Vector2 _startButtonPosition;

        private Vector2 _exitButtonPosition;

        private Vector2 _resumeButtonPosition;

        private Vector2 _pauseButtonPosition;

        TouchCollection touchCollection;

        private GameState gameState;

        enum GameState
        {
            StartMenu,
            Loading,
            Playing,
            Paused
        }

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                SupportedOrientations = DisplayOrientation.Portrait,
                IsFullScreen = true
            };

            _resolution = new ResolutionComponent(this, graphics, new Point(640, 360), new Point(640, 360), true, false);

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
            XnaMediaPlayer.IsRepeating = true;

            ////set the position of the buttons
            _startButtonPosition = new Vector2(60, 200);
            _exitButtonPosition = new Vector2(60, 250);
            _resumeButtonPosition = new Vector2(60, 200);
            _pauseButtonPosition = new Vector2(0, 0);

            //set the gamestate to start menu
            gameState = GameState.StartMenu;

            //get the touch state
            touchCollection = TouchPanel.GetState();

            _mapRenderer = new TiledMapRenderer(GraphicsDevice);

            //_camera = new Camera(_resolution.VirtualResolution.X, _resolution.VirtualResolution.Y);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //TODO: use this.Content to load your game content here
            _backgroundSong = Content.Load<Song>("Audio/Investigations");
            _playerTexture = Content.Load<Texture2D>("Sprites/charactersheet");
            _map = Content.Load<TiledMap>("test2");

            _startButton = Content.Load<Texture2D>(@"start");
            _exitButton = Content.Load<Texture2D>(@"exit");
            _resumeButton = Content.Load<Texture2D>(@"resume");
            _pauseButton = Content.Load<Texture2D>(@"pause");

            _player = new Player(_playerTexture, _resolution.TransformationMatrix());

            XnaMediaPlayer.Play(_backgroundSong);
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
#if !__IOS__
                Game.Activity.MoveTaskToBack(true);
#endif
            }
            // TODO: Add your update logic here  
       
            //Create the player if the game is playing
            if (gameState == GameState.Playing)
            { 
                _mapRenderer.Update(_map, gameTime);
                _player.Update(gameTime);
            }

            //wait for touch interaction
            touchCollection = TouchPanel.GetState();

            if (touchCollection.Count > 0)
            {
                Vector2 touchVector= Vector2.Transform(touchCollection[0].Position, Matrix.Invert(_resolution.TransformationMatrix()));
                //To add proper touch interaction
                TouchedScreen((int)touchVector.X, (int)touchVector.Y);
            }

            //_camera.Follow(_player);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateGray);

            //TODO: Add your drawing code here

            spriteBatch.Begin(transformMatrix: _resolution.TransformationMatrix());

            if (gameState==GameState.StartMenu)
            {
                //spriteBatch.Begin(transformMatrix: _resolution.TransformationMatrix());
                spriteBatch.Draw(_startButton, _startButtonPosition, Color.White);
                spriteBatch.Draw(_exitButton, _exitButtonPosition, Color.White);
                //spriteBatch.End();
            }

            //spriteBatch.Begin(transformMatrix: _camera.Transform / 2);

            if (gameState == GameState.Playing)
            {
                Vector2 scaleVector = new Vector2(0.5f, 0.5f);
                _mapRenderer.Draw(_map);
                _player.Draw(spriteBatch);
                spriteBatch.Draw(texture: _pauseButton, position: _pauseButtonPosition, color: Color.White, scale: scaleVector);
            }

            if(gameState==GameState.Paused)
            {
                spriteBatch.Draw(_resumeButton, _resumeButtonPosition, Color.White);
                spriteBatch.Draw(_exitButton, _exitButtonPosition, Color.White);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        void TouchedScreen(int x, int y)
        {
            //creates a rectangle of 5x5 around the place where the screen was touched
            Rectangle touchRect = new Rectangle(x, y, 5, 5);

            //check the startmenu
            if (gameState == GameState.StartMenu)
            {
                Rectangle startButtonRect = new Rectangle((int)_startButtonPosition.X, (int)_startButtonPosition.Y, 100, 20);
                Rectangle exitButtonRect = new Rectangle((int)_exitButtonPosition.X, (int)_exitButtonPosition.Y, 100, 20);

                if (touchRect.Intersects(startButtonRect)) //player touched start button
                {
                    gameState = GameState.Playing;
                }
                else if (touchRect.Intersects(exitButtonRect)) //player clicked exit button
                {
                    Exit();
                }

            }

            //check the pausebutton
            if (gameState == GameState.Playing)
            {
                Rectangle pauseButtonRect = new Rectangle((int)_pauseButtonPosition.X, (int)_pauseButtonPosition.Y, 35, 35);

                if (touchRect.Intersects(pauseButtonRect))
                {
                    gameState = GameState.Paused;
                }

            }

            //check the resumebutton
            if (gameState == GameState.Paused)
            {
                Rectangle resumeButtonRect = new Rectangle((int)_resumeButtonPosition.X, (int)_resumeButtonPosition.Y, 100, 20);
                Rectangle exitButtonRect = new Rectangle((int)_exitButtonPosition.X, (int)_exitButtonPosition.Y, 100, 20);

                if (touchRect.Intersects(resumeButtonRect))
                {
                    gameState = GameState.Playing;
                }
                else if (touchRect.Intersects(exitButtonRect)) //player clicked exit button
                {
                    Exit();
                }

            }

        }

    }
}
