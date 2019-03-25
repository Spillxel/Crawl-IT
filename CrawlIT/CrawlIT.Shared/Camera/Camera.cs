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
        private Matrix _scale;

        public Camera(int width, int height, Matrix scale)
        {
            _width = width;
            _height = height;
            _scale = scale;
        }

        public void Follow(Player target)
        {
            // For Laurence: if target is null set camera pos to 0, 0 to make the static stuff not move >:)
            var posx = target == null ? 0 : target.PosX;
            var posy = target == null ? 0 : target.PosY;

            Transform = 
                Matrix.CreateTranslation(new Vector3(- posx, - posy, 0))
                                         * Matrix.CreateRotationZ(0)
                                         * Matrix.CreateScale(new Vector3(3, 3, 1))
                                         * Matrix.CreateTranslation(new Vector3(_width * 0.5f, _height * 0.5f, 0));
        }
    }
}
