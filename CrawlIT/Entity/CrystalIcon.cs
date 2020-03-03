using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrawlIT.Shared
{
    public class CrystalIcon : AnimatedUiIcon
    {
        private readonly Animation _crystal0;
        private readonly Animation _crystal1;
        private readonly Animation _crystal2;
        private readonly Animation _crystal3;

        private readonly Player _player;

        public CrystalIcon(Texture2D texture, float scale, float posx, float posy, Player player)
            : base(texture, scale, posx, posy)
        {
            _player = player;

            _crystal0 = new Animation();
            _crystal0.AddFrame(new Rectangle(0, 0, IconFrameWidth, IconFrameHeight),
                               TimeSpan.FromSeconds(1));

            _crystal1 = new Animation();
            _crystal1.AddFrame(new Rectangle(0, IconFrameHeight, IconFrameWidth, IconFrameHeight),
                                TimeSpan.FromSeconds(1));

            _crystal2 = new Animation();
            _crystal2.AddFrame(new Rectangle(0, IconFrameHeight * 2, IconFrameWidth, IconFrameHeight),
                                TimeSpan.FromSeconds(1));

            _crystal3 = new Animation();
            _crystal3.AddFrame(new Rectangle(0, IconFrameHeight * 3, IconFrameWidth, IconFrameHeight),
                                TimeSpan.FromSeconds(1));

            CurrentAnimation = _crystal0;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var sourceRectangle = CurrentAnimation.CurrentRectangle;
            spriteBatch.Draw(TextureSheet, Position, sourceRectangle, Color.White, 0,
                             Vector2.Zero, Scale, SpriteEffects.None, 0);
        }

        public override void Update(GameTime gameTime)
        {
            SetAnimation();
            CurrentAnimation.Update(gameTime);
        }

        public override void SetAnimation()
        {
            switch (_player.CrystalCount)
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
                default:
                    break;

                // TODO: Implement a default that throws an error
            }
        }
    }
}
