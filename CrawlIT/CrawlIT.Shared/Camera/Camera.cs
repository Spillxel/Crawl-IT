using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using CrawlIT.Shared.Entity;

namespace CrawlIT.Shared.Camera
{
    public class Camera
    {
        public Matrix Transform { get; set; }
        private int _width;
        private int _height;
        private float _zoom;

        public Camera(int width, int height, float zoom)
        {
            _width = width;
            _height = height;
            _zoom = zoom;
        }

        public void Follow(Player target)
        {
            var posx = target?.PosX ?? 0;
            var posy = target?.PosY ?? 0;

            Transform = 
                Matrix.CreateTranslation(new Vector3(- posx, - posy, 0))
                                         * Matrix.CreateRotationZ(0)
                                         * Matrix.CreateScale(new Vector3(_zoom, _zoom, 1))
                                         * Matrix.CreateTranslation(new Vector3(_width * 0.5f, _height * 0.5f, 0));
        }
    }
}
