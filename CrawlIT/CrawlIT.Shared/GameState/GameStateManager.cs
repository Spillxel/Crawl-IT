using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CrawlIT.Shared
{
    public class GameStateManager
    {
        // Instance of the game state manager     
        private static GameStateManager _instance;

        // Stack for the screens     
        private readonly Stack<GameState> _screens = new Stack<GameState>();

        private GraphicsDevice _graphicsDevice;
        private ContentManager _content;
        private Point _resolution;
        private Matrix _transform;

        public static GameStateManager Instance => _instance ?? (_instance = new GameStateManager());

        public GameState.StateType State => _screens.Peek().State;

        public Texture2D BlankTexture;

        // Init stuff
        public void Init(GraphicsDevice graphicsDevice, ContentManager content,
                         Point resolution, Matrix transform)
        {
            _graphicsDevice = graphicsDevice;
            _content = content;
            _resolution = resolution;
            _transform = transform;

            BlankTexture = new Texture2D(_graphicsDevice, _resolution.X, _resolution.Y);
            var data = new Color[_resolution.X * _resolution.Y];
            data = data.Select(i => Color.Black * 0.7f).ToArray();
            BlankTexture.SetData(data);
        }

        // Adds a new screen to the stack 
        public void AddScreen(GameState screen)
        {
            try
            {
                // Add the screen to the stack
                _screens.Push(screen);
                // Initialize the screen
                _screens.Peek().Initialize();
                // Call the LoadContent on the screen
                if (_content != null)
                {
                    _screens.Peek().LoadContent(_content);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        // Removes the top screen from the stack
        public void RemoveScreen()
        {
            if (_screens.Count <= 0) return;
            try
            {
                _screens.Pop();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }


        // Clears all the screen from the list
        public void ClearScreens()
        {
            while (_screens.Count > 0)
                _screens.Pop();
        }


        // Removes all screens from the stack and adds a new one 
        public void ChangeScreen(GameState screen)
        {
            try
            {
                ClearScreens();
                AddScreen(screen);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        // Updates the top screen. 
        public void Update(GameTime gameTime)
        {
            try
            {
                if (_screens.Count > 0)
                {
                    _screens.Peek().Update(gameTime);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        // Renders the top screen.
        public void Draw(SpriteBatch spriteBatch)
        {
            try
            {
                if (_screens.Count > 0)
                {
                    _screens.Peek().Draw(spriteBatch);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public void FadeBackBufferToBlack(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(transformMatrix: _transform, samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(BlankTexture, Vector2.Zero, Color.White);
            spriteBatch.End();
        }

        // Unloads the content from the screen
        public void UnloadContent()
        {
            foreach (var state in _screens)
            {
                state.UnloadContent();
            }
        }
    }
}
