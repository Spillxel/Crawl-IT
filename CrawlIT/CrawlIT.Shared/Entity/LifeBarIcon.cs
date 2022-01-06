using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CrawlIT.Shared
{
    public class LifeBarIcon : AnimatedUiIcon
    {
        private readonly Animation _lifeBar0;
        private readonly Animation _lifeBar1;
        private readonly Animation _lifeBar2;
        private readonly Animation _lifeBar3;

        private readonly Player _player;

        public LifeBarIcon(Texture2D texture, float scale, Vector2 position, Player player)
            : base(texture, scale, position)
        {
            _player = player;

            _lifeBar0 = new Animation();
            _lifeBar0.AddFrame(new Rectangle(0, 0, IconFrameWidth, IconFrameHeight),
                               TimeSpan.FromSeconds(1));

            _lifeBar1 = new Animation();
            _lifeBar1.AddFrame(new Rectangle(0, IconFrameHeight, IconFrameWidth, IconFrameHeight),
                               TimeSpan.FromSeconds(1));

            _lifeBar2 = new Animation();
            _lifeBar2.AddFrame(new Rectangle(0, IconFrameHeight * 2, IconFrameWidth, IconFrameHeight),
                               TimeSpan.FromSeconds(1));

            _lifeBar3 = new Animation();
            _lifeBar3.AddFrame(new Rectangle(0, IconFrameHeight * 3, IconFrameWidth, IconFrameHeight),
                               TimeSpan.FromSeconds(1));

            CurrentAnimation = _lifeBar3;
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
            switch (_player.LifeCount)
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
                case 4:
                    Console.WriteLine("You somehow have 4 lives???");
                    break;
                default:
                    break;
            }
        }
    }
}
