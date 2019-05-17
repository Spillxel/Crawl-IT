using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrawlIT.Shared.Entity
{
    public abstract class AnimatedUiIcon : UiIcon
    {
        protected int IconFrameWidth;
        protected int IconFrameHeight;

        public Animation.Animation CurrentAnimation;

        protected AnimatedUiIcon(Texture2D texture, Matrix scale, float posX, float posY)
            : base(texture, scale, posX, posY)
        {
            IconFrameWidth = 32;
            IconFrameHeight = 32;
        }

        public abstract void SetAnimation();
    }
}
