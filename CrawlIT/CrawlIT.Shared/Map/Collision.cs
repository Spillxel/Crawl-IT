using System;
using System.Collections.Generic;
using System.Text;

using MonoGame.Extended.Tiled;

using Microsoft.Xna.Framework;

using CrawlIT.Shared.Entity;

namespace CrawlIT.Shared.Map
{
    class Collision
    {

        private TiledMap _map;
        public List<Rectangle> CollisionObjects { get; protected set; }
        public Rectangle CurrentCollision { get; protected set; }

        public Collision(TiledMap map)
        {
            _map = map;
            CollisionObjects = new List<Rectangle>();
            foreach (var o in _map.ObjectLayers[0].Objects)
                CollisionObjects.Add(new Rectangle((int)o.Position.X, (int)o.Position.Y, (int)o.Size.Width, (int)o.Size.Height));
        }

        public bool IsCollisionTile(Rectangle playerRectangle)
        {
            foreach (Rectangle rect in CollisionObjects)
                if (rect.Intersects(playerRectangle))
                {
                    CurrentCollision = rect;
                    return true;
                }  
            return false;
        }

        public bool HitsFromTheTop(Player player)
        {
            return  player.Rectangle.Bottom + player.CurrentVelocity.Y > this.CurrentCollision.Top &&
                    player.Rectangle.Top < this.CurrentCollision.Right &&
                    player.Rectangle.Right > this.CurrentCollision.Left &&
                    player.Rectangle.Left < this.CurrentCollision.Right;
        }

        public bool HitsFromTheBottom(Player player)
        {
            return  player.Rectangle.Top + player.CurrentVelocity.Y < this.CurrentCollision.Bottom &&
                    player.Rectangle.Bottom > this.CurrentCollision.Bottom &&
                    player.Rectangle.Right > this.CurrentCollision.Left &&
                    player.Rectangle.Left < this.CurrentCollision.Right;
        }

        public bool HitsFromTheRight(Player player)
        {
            return  player.Rectangle.Left + player.CurrentVelocity.X < this.CurrentCollision.Right &&
                    player.Rectangle.Right > this.CurrentCollision.Right &&
                    player.Rectangle.Bottom > this.CurrentCollision.Top &&
                    player.Rectangle.Top < this.CurrentCollision.Bottom;
        }
        
        public bool HitsFromTheLeft(Player player)
        {
            return player.Rectangle.Right + player.CurrentVelocity.X > this.CurrentCollision.Left &&
                   player.Rectangle.Left < this.CurrentCollision.Left &&
                   player.Rectangle.Bottom > this.CurrentCollision.Top &&
                   player.Rectangle.Top < this.CurrentCollision.Bottom;
        }
    }
}
