using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrawlIT.Shared.Entity
{
    public class UIIcon : Entity
    {
        private readonly Vector2 _position;
        static readonly Vector2 _scale = new Vector2(5, 5);

        public UIIcon(Texture2D texture, Matrix scale, float posx, float posy)
        {
            TextureSheet = texture;
            PosX = posx;
            PosY = posy;
            _position = new Vector2(PosX, PosY);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureSheet, _position, null, null, null, 0, _scale, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
