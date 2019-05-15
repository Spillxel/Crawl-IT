using System;
using System.Runtime.InteropServices;
using Android.Views.Animations;
using Java.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrawlIT.Shared.Entity
{
    public class UIIcon : Entity
    {
        protected readonly Vector2 _position;
        protected readonly Vector2 _scale;

        public UIIcon(Texture2D texture, float zoom, float posx, float posy)
        {
            TextureSheet = texture;
            _scale = new Vector2(zoom);
            PosX = posx;
            PosY = posy;
            _position = new Vector2(PosX, PosY);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(TextureSheet, _position, null,
                             Color.White, 0, Vector2.Zero,
                            _scale, SpriteEffects.None, 0);
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
