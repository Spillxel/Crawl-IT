using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrawlIT.Shared.Entity
{
    public abstract class AnimatedUIIcon : UIIcon
    {
        protected int _iconFrameWidth;
        protected int _iconFrameHeight;

        public Animation.Animation CurrentAnimation;

        public AnimatedUIIcon(Texture2D texture, float zoom, float posx, float posy)
            : base(texture, zoom, posx, posy)
        {
            _iconFrameWidth = 32;
            _iconFrameHeight = 32;
        }

        public abstract void SetAnimaton();
    }
}
