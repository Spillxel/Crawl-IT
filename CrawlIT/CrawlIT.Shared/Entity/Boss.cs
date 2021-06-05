using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CrawlIT.Shared
{
    public class Boss : Enemy
    {
        public Animation Idle;

        public Boss(ContentManager content, string texture, string closeUpTexture, float posx, float posy, int fights, int questions)
            : base(content, texture, closeUpTexture, posx, posy, fights, questions)
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

        public override void BeatenBy(Player player)
        {
            // TODO: implement multiple rounds/questions
            player.Enemies.Remove(this);
            player.CollisionObjects.Remove(CollisionRectangle);
        }

        public override void Beat(Player player)
        {
            Fights = Math.Max(0, Fights - 1);
            player.SetLifeCount(--player.LifeCount);
            // TODO: Insert dialogue when you lose against boss
        }
    }
}
