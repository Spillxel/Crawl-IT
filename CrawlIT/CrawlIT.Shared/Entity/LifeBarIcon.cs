using CrawlIT.Shared.Animation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CrawlIT.Shared.Entity
{
    public class LifeBarIcon : AnimatedUIIcon
    {
        private Animation.Animation _lifeBar0;
        private Animation.Animation _lifeBar1;
        private Animation.Animation _lifeBar2;
        private Animation.Animation _lifeBar3;

        private Player _player;

        public LifeBarIcon(Texture2D texture, float zoom, float posx, float posy, Player player)
            : base (texture, zoom, posx, posy)
        {
            _player = player;

            _lifeBar0 = new Animation.Animation();
            _lifeBar0.AddFrame(new Rectangle(0, 0, _iconFrameWidth, _iconFrameHeight), TimeSpan.FromSeconds(1));

            _lifeBar1 = new Animation.Animation();
            _lifeBar1.AddFrame(new Rectangle(0, _iconFrameHeight, _iconFrameWidth, _iconFrameHeight), TimeSpan.FromSeconds(1));

            _lifeBar2 = new Animation.Animation();
            _lifeBar2.AddFrame(new Rectangle(0, _iconFrameHeight * 2, _iconFrameWidth, _iconFrameHeight), TimeSpan.FromSeconds(1));

            _lifeBar3 = new Animation.Animation();
            _lifeBar3.AddFrame(new Rectangle(0, _iconFrameHeight * 3, _iconFrameWidth, _iconFrameHeight), TimeSpan.FromSeconds(1));

            CurrentAnimation = _lifeBar3;
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
            switch (_player.lifeCount)
            {
                case 0:
                    CurrentAnimation = _lifeBar0;
                    break;
                case 1:
                    CurrentAnimation = _lifeBar1;
                    break;
                case 2:
                    CurrentAnimation = _lifeBar2;
                    break;
                case 3:
                    CurrentAnimation = _lifeBar3;
                    break;
                // TODO: Implement a default that throws an error
            }
        }
    }
}
