#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using XnaMediaPlayer = Microsoft.Xna.Framework.Media.MediaPlayer;

using ResolutionBuddy;
using Microsoft.Xna.Framework.Input.Touch;

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
        TouchCollection touchCollection;

        private Texture2D _background;
        readonly IResolution _resolution;
        private Rectangle target = new Rectangle(0,0,100,100);
        private Song _backgroundSong;
        private Song _backgroundSong2;
        private SpriteFont font;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this)
            {
                SupportedOrientations = DisplayOrientation.Portrait,
                IsFullScreen = true
            };

            _resolution = new ResolutionComponent(this, graphics, new Point(1280, 720), new Point(1280, 720), true, false);

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
            // TODO: Add your initialization logic here
            XnaMediaPlayer.IsRepeating = true;
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
            _background = Content.Load<Texture2D>("mulli");
            _backgroundSong = Content.Load<Song>("sneaky_snitch");
            _backgroundSong2 = Content.Load<Song>("investigations");
            font = Content.Load<SpriteFont>("test");

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
                Exit();
            #endif
            }
            // TODO: Add your update logic here  
            touchCollection = TouchPanel.GetState();

            foreach (TouchLocation tl in touchCollection)
            {
                if (tl.State == TouchLocationState.Pressed &&
                    tl.Position.X > target.Left &&
                    tl.Position.X < target.Right &&
                    tl.Position.Y > target.Top &&
                    tl.Position.Y < target.Bottom)
                {
                    XnaMediaPlayer.Play(_backgroundSong2);
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //TODO: Add your drawing code here

            spriteBatch.Begin();

            spriteBatch.Draw(_background, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
            spriteBatch.DrawString(font, "touch me", new Vector2(25, 50), Color.Black);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
