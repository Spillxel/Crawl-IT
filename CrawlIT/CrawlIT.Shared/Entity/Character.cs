using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrawlIT.Shared.Entity
{
    public abstract class Character : Entity
    {
        public int FrameWidth;
        public int FrameHeight;

        // For collision
        public Rectangle Rectangle => new Rectangle((int)PosX, (int)PosY, FrameWidth, FrameHeight);
    }
}
