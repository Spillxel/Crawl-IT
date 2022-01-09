using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrawlIT.Shared
{
    public class CrystalIcon : AnimatedUiIcon
    {
        private const uint MaxCrystals = 6;
        private readonly List<Animation> _crystal;

        private readonly Player _player;

        public CrystalIcon(Texture2D texture, float scale, Vector2 position, Player player)
            : base(texture, scale, position)
        {
            // need to keep track of crystal count to set the correct animation
            _player = player;

            _crystal = new List<Animation>();
            foreach (var i in Enumerable.Range(0, (int)MaxCrystals + 1))
            {
                var animation = new Animation();
                animation.AddFrame(new Rectangle(0, IconFrameHeight * i, IconFrameWidth, IconFrameHeight),
                                   TimeSpan.FromSeconds(1));
                _crystal.Add(animation);
            }

            CurrentAnimation = _crystal[_player.CrystalCount];
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
            CurrentAnimation = _crystal[_player.CrystalCount];
        }
    }
}
