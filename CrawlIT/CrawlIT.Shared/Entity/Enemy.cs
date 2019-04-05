using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrawlIT.Shared.Entity
{

    public class Enemy : Character
    {
        private int _rounds;

        public Player Player { get; set; }

        public Enemy(Texture2D texture, float posx, float posy, int rounds)
        {
            TextureSheet = texture;
            PosX = posx;
            PosY = posy;
            FrameWidth = 23;
            FrameHeight = 45;
            _rounds = rounds;
        }

        public override void Update(GameTime gameTime)
        {

            if (Collides(Player.Rectangle) && _rounds > 0)
            {
                //pause game
                //launch fight screen
                _rounds--;
            }
            else
            {
                //show text panel saying I have no more questions for you
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var position = new Vector2(PosX, PosY);
            spriteBatch.Draw(TextureSheet, position, Rectangle, Color.White);
        }

        public bool Collides(Rectangle rectangle)
        {
            if (this.Rectangle.Intersects(rectangle))
                return true;
            return false;
        }
    }
}
