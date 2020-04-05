using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CrawlIT.Shared
{
    internal static class Helper
    {
        
        /// <summary>
        /// Wrapper to draw without using the ugly spriteBatch alternative.
        /// </summary>
        /// <param name="spriteBatch">Batch to draw to.</param>
        /// <param name="texture">Texture to draw.</param>
        /// <param name="position">Where to draw the texture.</param>
        /// <param name="source">Which part of the texture to draw (null = all of it).</param>
        /// <param name="scale">Scale the texture up or down.</param>
        public static void DrawWrapper(
            SpriteBatch spriteBatch,
            Texture2D texture,
            Vector2 position,
            Rectangle? source,
            float scale)
        {
            spriteBatch.Draw(
                texture,
                position,
                source,
                Color.White,
                0,
                Vector2.Zero,
                scale,
                SpriteEffects.None,
                0);
        }
    }
}