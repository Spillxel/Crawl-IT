using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CrawlIT.Shared.GameState
{
    public abstract class GameState
    {
        protected GraphicsDevice GraphicsDevice;

        protected GameState(GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;
        }

        public abstract void Initialize();

        public abstract void LoadContent(ContentManager content);

        public abstract void SetState(Enum gameState);

        public abstract Enum GetState();

        public abstract void UnloadContent();

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);

        public abstract Point GetPosition(Texture2D button);

        public abstract bool GetAnswer(Rectangle touch);

        public abstract void ChangeColour(SpriteBatch spriteBatch);

        public abstract void Help(SpriteBatch spriteBatch); 
    }
}
