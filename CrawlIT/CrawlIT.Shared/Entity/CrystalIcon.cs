using CrawlIT.Shared.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CrawlIT.Shared.Entity
{
    public class CrystalIcon : AnimatedUIIcon
    {
        private Animation.Animation _crystal0;
        private Animation.Animation _crystal1;
        private Animation.Animation _crystal2;
        private Animation.Animation _crystal3;

        private Player _player;

        public CrystalIcon(Texture2D texture, float zoom, float posx, float posy, Player player)
            : base(texture, zoom, posx, posy)
        {
            _player = player;

            _crystal0 = new Animation.Animation();
            _crystal0.AddFrame(new Rectangle(0, 0, _iconFrameWidth, _iconFrameHeight), TimeSpan.FromSeconds(1));

            _crystal1 = new Animation.Animation();
            _crystal1.AddFrame(new Rectangle(0, _iconFrameHeight, _iconFrameWidth, _iconFrameHeight), TimeSpan.FromSeconds(1));

            _crystal2 = new Animation.Animation();
            _crystal2.AddFrame(new Rectangle(0, _iconFrameHeight * 2, _iconFrameWidth, _iconFrameHeight), TimeSpan.FromSeconds(1));

            _crystal2 = new Animation.Animation();
            _crystal2.AddFrame(new Rectangle(0, _iconFrameHeight * 3, _iconFrameWidth, _iconFrameHeight), TimeSpan.FromSeconds(1));

            CurrentAnimation = _crystal0;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var sourceRectangle = CurrentAnimation.CurrentRectangle;

            spriteBatch.Draw(TextureSheet, _position, sourceRectangle,
                             Color.White, 0, Vector2.Zero,
                             _scale, SpriteEffects.None, 0);
        }

        public override void Update(GameTime gameTime)
        {
            SetAnimaton();
            CurrentAnimation.Update(gameTime);
        }

        public override void SetAnimaton()
        {
            switch (_player.crystalCount)
            {
                case 0:
                    CurrentAnimation = _crystal0;
                    break;
                case 1:
                    CurrentAnimation = _crystal1;
                    break;
                case 2:
                    CurrentAnimation = _crystal2;
                    break;
                case 3:
                    CurrentAnimation = _crystal3;
                    break;
                    // TODO: Implement a default that throws an error
            }
        }
    }
}
