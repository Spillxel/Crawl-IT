using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrawlIT.Shared
{
    public abstract class AnimatedUiIcon : UiIcon
    {
        protected int IconFrameWidth;
        protected int IconFrameHeight;

        public Animation CurrentAnimation;

        protected AnimatedUiIcon(Texture2D texture, Matrix scale, float posX, float posY)
            : base(texture, scale, posX, posY)
        {
            IconFrameWidth = texture.Width;
            IconFrameHeight = texture.Height;
        }

        public abstract void SetAnimation();
    }
}
