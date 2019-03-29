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
using CrawlIT.Shared.GameStates;
using System.Collections.Generic;

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

        private GameState _menu;
        private GameState _level1;
        private GameState _level2;

        TouchCollection touchCollection;

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
            _menu = new Menu(GraphicsDevice);
            _menu.SetState("Menu");

            _level1 = new Level1(GraphicsDevice);
            _level1.SetState("Playing");

            _level2 = new Level2(GraphicsDevice);
            _level2.SetState("Playing");

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

            ////TODO: use this.Content to load your game content here
            _backgroundSong = Content.Load<Song>("Audio/Investigations");
            _playerTexture = Content.Load<Texture2D>("Sprites/charactersheet");
            _player = new Player(_playerTexture, _resolution.TransformationMatrix());

            XnaMediaPlayer.Play(_backgroundSong);

            //Set of the content for being used by GameStateManager
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
#if !__IOS__
                Game.Activity.MoveTaskToBack(true);
#endif
            }

            //wait for touch interaction
            touchCollection = TouchPanel.GetState();

            //Update of the GameStateManager
            GameStateManager.Instance.Update(gameTime);


            if (touchCollection.Count > 0)
            {
                GameStateManager.Instance.AddScreen(_level1);
            }

            _player.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>j
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            GameStateManager.Instance.Draw(spriteBatch);

            if (GameStateManager.Instance.GetCurrentState() == "Playing")
            {
                spriteBatch.Begin(transformMatrix: _resolution.TransformationMatrix());
                _player.Draw(spriteBatch);
                spriteBatch.End();
            }


            base.Draw(gameTime);
        }
    }
}
