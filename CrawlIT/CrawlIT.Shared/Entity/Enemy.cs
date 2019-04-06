using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Timers;

namespace CrawlIT.Shared.Entity
{

    public class Enemy : Character
    {
        private Animation.Animation _currentAnimation;

        private readonly Matrix _scale;

        private readonly Vector2 _position;

        public int Rounds;

        // For collision
        public Rectangle Rectangle;

        public Enemy(Texture2D texture, Matrix scale, float posx, float posy, int rounds)
        {
            TextureSheet = texture;
            PosX = posx;
            PosY = posy;
            _position = new Vector2(PosX, PosY);
            FrameWidth = 23;
            FrameHeight = 45;
            _scale = scale;
            Rounds = rounds;

            Rectangle = new Rectangle((int)PosX, (int)PosY, FrameWidth, (int)(FrameHeight / 1.5));

            StandDown = new Animation.Animation();
            StandDown.AddFrame(new Rectangle(0, 0, FrameWidth, FrameHeight), TimeSpan.FromSeconds(1));

            _currentAnimation = StandDown;
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //var position = new Vector2(PosX, PosY);
           
            var sourceRectangle = _currentAnimation.CurrentRectangle;
            spriteBatch.Draw(TextureSheet, _position, sourceRectangle, Color.White);
        }
    }
}
