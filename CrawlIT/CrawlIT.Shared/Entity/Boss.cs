using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrawlIT.Shared
{
    public class Boss : Enemy
    {
        public Animation Idle;

        public Boss(Texture2D texture, Texture2D closeUpTexture, float posx, float posy, int fightsLeft, int questionPerFight)
            : base(texture, closeUpTexture, posx, posy, fightsLeft, questionPerFight)
        {
            FrameWidth = 39;
            FrameHeight = 53;

            CollisionRectangle = new Rectangle((int)PosX, (int)PosY, FrameWidth, (int)(FrameHeight / 1.5));
            FightRectangle = new Rectangle((int)PosX - 1, (int)PosY - 1, FrameWidth + 2, (int)(FrameHeight / 1.5) + 2);
                        
            Idle = new Animation();
            Idle.AddFrame(new Rectangle(0, 0, FrameWidth, FrameHeight), TimeSpan.FromSeconds(0.25));
            Idle.AddFrame(new Rectangle(0, FrameHeight, FrameWidth, FrameHeight), TimeSpan.FromSeconds(0.25));
            Idle.AddFrame(new Rectangle(0, FrameHeight * 2, FrameWidth, FrameHeight), TimeSpan.FromSeconds(0.25));
            Idle.AddFrame(new Rectangle(0, FrameHeight * 3, FrameWidth, FrameHeight), TimeSpan.FromSeconds(0.25));

            StandDown = Idle;
            StandLeft = Idle;
            StandRight = Idle;
            StandUp = Idle;

            CurrentAnimation = Idle;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void BeatenBy(Player player)
        {
            this.FightsLeft = Math.Max(0, this.FightsLeft - 1);
            if (this.FightsLeft == 0)
            {
                player.Enemies.Remove(this);
                player.CollisionObjects.Remove(this.CollisionRectangle);
            }     
        }

        public override void Beat(Player player)
        {
            player.SetLifeCount(--player.LifeCount);
            // TODO: Insert dialogue when you lose against boss
        }
    }
}
