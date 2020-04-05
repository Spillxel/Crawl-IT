using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrawlIT.Shared
{
    public abstract class AnimatedUiIcon : UiIcon
    {
        protected int IconFrameWidth;
        protected int IconFrameHeight;

        public Animation CurrentAnimation;

        protected AnimatedUiIcon(Texture2D texture, float scale, Vector2 position)
            : base(texture, scale, position)
        {
            IconFrameWidth = 32;
            IconFrameHeight = 32;
        }

        public abstract void SetAnimation();
    }
}
