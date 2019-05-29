﻿using System;

using Microsoft.Xna.Framework;

namespace CrawlIT.Shared
{
    /// <summary>
    /// <para>Serves to define an <c>Camera</c> with a width, height and zoom level.</para>
    /// <para>The class then provides the <see cref="Follow"/> to either follow a
    /// <see cref="Player"/> or null (static camera).</para>
    /// <para>This method updates the camera's transform and position.
    /// The transform can be used to draw using the camera's perspective.</para>
    /// </summary>
    public class Camera
    {
        public Matrix Transform { get; private set; }
        public float Zoom;
        private readonly int _width;
        private readonly int _height;
        private float _posX;
        private float _posY;

        public Camera(int width, int height, float zoom)
        {
            _width = width;
            _height = height;
            Zoom = Math.Max(3, zoom);
        }

        public void Follow(Player target)
        {
            _posX = target == null ? 0 : MathHelper.Lerp(_posX, target.PosX, 0.08f);
            _posY = target == null ? 0 : MathHelper.Lerp(_posY, target.PosY, 0.08f);

            Transform = Matrix.CreateTranslation(new Vector3(-_posX - target?.FrameWidth * 0.5f ?? 0,
                                                             -_posY - target?.FrameHeight * 0.5f ?? 0,
                                                             0))
                        * Matrix.CreateRotationZ(0)
                        * Matrix.CreateScale(Zoom, Zoom, 1)
                        * Matrix.CreateTranslation(new Vector3(_width * 0.5f, _height * 0.5f, 0));
        }
    }
}
