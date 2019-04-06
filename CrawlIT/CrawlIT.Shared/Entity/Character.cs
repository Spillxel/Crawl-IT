using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrawlIT.Shared.Entity
{
    public abstract class Character : Entity
    {
        public Animation.Animation StandUp;
        public Animation.Animation StandDown;
        public Animation.Animation StandLeft;
        public Animation.Animation StandRight;

        public int FrameWidth;
        public int FrameHeight; 
    }
}
