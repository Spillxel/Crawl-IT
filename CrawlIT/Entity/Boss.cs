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

        // TODO: fight rectangle could be taken care off by Tiled, too...
        public Boss(ContentManager content, string texture, string closeUpTexture, Vector2 position, Point frame, int fights, int questions)
            : base(content, texture, closeUpTexture, position, frame, fights, questions)
        {
            Idle = new Animation();
            Idle.AddFrame(new Rectangle(0, 0, Frame.X, Frame.Y), TimeSpan.FromSeconds(0.25));
            Idle.AddFrame(new Rectangle(0, Frame.Y, Frame.X, Frame.Y), TimeSpan.FromSeconds(0.25));
            Idle.AddFrame(new Rectangle(0, Frame.Y * 2, Frame.X, Frame.Y), TimeSpan.FromSeconds(0.25));
            Idle.AddFrame(new Rectangle(0, Frame.Y * 3, Frame.X, Frame.Y), TimeSpan.FromSeconds(0.25));

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
