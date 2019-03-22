using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrawlIT.Shared.IEntity
{
    public interface IEntity
    {
        static Texture2D textureSheet;

        public float X { get; protected set; }
        public float Y { get; protected set; }
    }
}
