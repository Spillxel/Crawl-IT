using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrawlIT.Shared.Entity
{
    public abstract class Entity
    {
        // point of this being static is to avoid loading the character sheet
        // multiple times, not sure whether it makes sense in the current
        // inheritance model... TODO
        protected static Texture2D TextureSheet;

        public float X { get; protected set; }
        public float Y { get; protected set; }


        public float PosX { get; protected set; }
        public float PosY { get; protected set; }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
