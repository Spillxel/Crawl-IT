using Microsoft.Xna.Framework;

namespace CrawlIT.Shared
{
    public abstract class Character : Entity
    {
        public Animation StandUp;
        public Animation StandDown;
        public Animation StandLeft;
        public Animation StandRight;

        public Point Frame;
    }
}
