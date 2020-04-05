using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;

namespace CrawlIT.Shared
{

    /// <summary>
    /// <para>
    /// Extension class that holds extension methods.
    /// </para>
    /// <para> 
    /// For example: <see cref="ToPoint"/>, adds a <see cref="Size2"/> -> <see cref="Point"/> conversion method to
    /// <see cref="Size2"/>.
    /// </para>
    /// </summary>
    internal static class Extension
    {

        /// <summary>
        /// Converts <paramref name="size"/> to <see cref="Point"/>. Useful for simple conversion of
        /// <see cref="TiledMapObjectLayer"/> collision sizes.
        /// </summary>
        /// <param name="size">The <see cref="Size2"/> to convert.</param>
        /// <returns><see cref="Point"/> that defines the size of... <paramref name="size"/>.</returns>
        public static Point ToPoint(this Size2 size)
        {
            return new Point((int) size.Width, (int) size.Height);
        }

        /// <summary>
        /// Converts <paramref name="mapLayerObject"/> to <see cref="Rectangle"/>. Useful for simple conversion of a
        /// <see cref="TiledMapObject"/> collision to a <see cref="Rectangle"/> for... collision detection.
        /// </summary>
        /// <param name="mapLayerObject">The <see cref="TiledMapObject"/> to convert.</param>
        /// <returns><see cref="Rectangle"/> that defines the corresponding rectangle with proper position and size on
        /// the map.</returns>
        public static Rectangle ToRectangle(this TiledMapObject mapLayerObject)
        {
            return new Rectangle(mapLayerObject.Position.ToPoint(), mapLayerObject.Size.ToPoint());
        }
    }
}
