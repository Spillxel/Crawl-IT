using System;
using System.Collections.Generic;
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

        private ContentManager _content;

        public static GameStateManager Instance => _instance ?? (_instance = new GameStateManager());

        // Sets the content manager
        public void SetContent(ContentManager content)
        {
            _content = content;
        }

        // Get the state of the current screen
        public bool IsState(Enum state)
        {
            return _screens.Peek().GetState().Equals(state);
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
