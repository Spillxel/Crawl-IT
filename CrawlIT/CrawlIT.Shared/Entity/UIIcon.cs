using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrawlIT.Shared
{
    public class UiIcon : Entity
    {
        protected readonly Vector2 Position;
        protected float Scale;

        public UiIcon(Texture2D texture, float scale, float posX, float posY)
        {
            TextureSheet = texture;
            Scale = scale;
            PosX = posX;
            PosY = posY;
            Position = new Vector2(PosX, PosY);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureSheet, Position, null, Color.White, 0,
                             Vector2.Zero, Scale, SpriteEffects.None, 0);
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
