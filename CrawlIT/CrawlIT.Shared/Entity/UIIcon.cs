using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrawlIT.Shared.Entity
{
    public class UiIcon : Entity
    {
        protected readonly Vector2 Position;
        protected Matrix Scale;

        public UiIcon(Texture2D texture, Matrix scale, float posX, float posY)
        {
            TextureSheet = texture;
            Scale = scale;
            PosX = posX;
            PosY = posY;
            Position = new Vector2(PosX, PosY);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureSheet, Position, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
