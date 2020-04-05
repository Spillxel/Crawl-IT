using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrawlIT.Shared
{
    public abstract class Entity
    {
        protected Texture2D TextureSheet;

        public Vector2 Position { get; protected set; }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
