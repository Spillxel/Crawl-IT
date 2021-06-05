using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CrawlIT.Shared
{
    public abstract class GameState
    {
        protected GraphicsDevice GraphicsDevice;

        protected Player Player;

        public enum StateType
        {
            Splash,
            Menu,
            Playing,
            Fighting,
        }

        public abstract StateType State { get; }

        protected GameState(GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;
        }

        public abstract void Initialize();
        public abstract void LoadContent(ContentManager content);
        public abstract void UnloadContent();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
