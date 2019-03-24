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

        private Player _player;

        private TiledMap _map;

        private TiledMapRenderer _mapRenderer;

        //private Camera2D _camera;

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
            base.Initialize();
            XnaMediaPlayer.IsRepeating = true;
            _mapRenderer = new TiledMapRenderer(GraphicsDevice);
            //var viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, 800, 480);
            //_camera = new Camera2D(viewportAdapter);
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
            _mapRenderer.Update(_map, gameTime);
            _player.Update(gameTime);
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
            
            //the camera produces a view matrix that can be applied to any sprite batch
            //var transformMatrix = _camera.GetViewMatrix();
            //spriteBatch.Begin(transformMatrix: transformMatrix);

            _player.Draw(spriteBatch);

            _mapRenderer.Draw(_map);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
