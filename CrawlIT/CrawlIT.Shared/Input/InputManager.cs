using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace CrawlIT.Shared.Input
{
    internal class InputManager
    {
        public bool InPinch { get; private set; }

        private readonly Camera.Camera _camera;
        private float _pinchDistance;

        public InputManager(Camera.Camera camera)
        {
            _camera = camera;
        }

        public void Update(GameTime gameTime)
        {   // Thanks Corey @ StackOverflow!
            while (TouchPanel.IsGestureAvailable)
            {
                var gesture = TouchPanel.ReadGesture();

                if (gesture.GestureType == GestureType.Pinch)
                {
                    var currentPosOne = gesture.Position;
                    var currentPosTwo = gesture.Position2;
                    var currentDistance = Vector2.Distance(currentPosOne, currentPosTwo);

                    var oldPosOne = gesture.Position - gesture.Delta;
                    var oldPosTwo = gesture.Position2 - gesture.Delta2;
                    var oldDistance = Vector2.Distance(oldPosOne, oldPosTwo);

                    if (!InPinch)
                    {
                        InPinch = true;
                        _pinchDistance = oldDistance;
                    }

                    // set new zoom level and clamp it to normal values
                    _camera.Zoom -= (oldDistance - currentDistance) * 0.01f;
                    _camera.Zoom = Math.Min(12f, Math.Max(_camera.Zoom, 3f));
                }
                else if (gesture.GestureType == GestureType.PinchComplete)
                {
                    InPinch = false;
                }
            }
        }
    }
}
