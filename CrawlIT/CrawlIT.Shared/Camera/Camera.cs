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

        public Camera(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public void Follow(Player target)
        {
            var position = Matrix.CreateTranslation(
                -target.PosX - 8,
                -target.PosY - 8,
                0);
            var offset = Matrix.CreateTranslation(
                _width / 2,
                _height / 2,
                0);
            Transform = position * offset;
            
        }
    }
}
