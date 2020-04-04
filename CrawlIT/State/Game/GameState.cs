using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CrawlIT.Shared
{
    public abstract class GameState
    {
        protected GraphicsDevice GraphicsDevice;

        protected Player Player;

        protected GameState(GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;
        }

        public abstract GameStateType State { get; }

        public abstract void Initialize();
        public abstract void LoadContent(ContentManager content);
        public abstract void Dispose();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
