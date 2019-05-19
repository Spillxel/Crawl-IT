using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrawlIT.Shared
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
        {   // TODO: reformulate scale/matrix/transform stuff for icons (_transform to draw, _scaleVector? to scale, test it!
            spriteBatch.Draw(TextureSheet, Position, null, Color.White, 0, Vector2.Zero, new Vector2(3), SpriteEffects.None, 0);
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
