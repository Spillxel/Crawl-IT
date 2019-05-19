using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace CrawlIT.Shared
{
    public class InputManager
    {
        public enum InputState
        {
            Idle,
            Pinch,
        }

        public InputState CurrentInputState { get; private set; }

        private readonly Camera _camera;

        public InputManager(Camera camera)
        {
            _camera = camera;
            CurrentInputState = InputState.Idle;
        }

        public void Update(GameTime gameTime)
        {   // Thanks Corey @ StackOverflow!
            while (TouchPanel.IsGestureAvailable)
            {
                var gesture = TouchPanel.ReadGesture();

                if (gesture.GestureType == GestureType.Pinch)
                {
                    CurrentInputState = InputState.Pinch;

                    var currentPosOne = gesture.Position;
                    var currentPosTwo = gesture.Position2;
                    var currentDistance = Vector2.Distance(currentPosOne, currentPosTwo);

                    var oldPosOne = gesture.Position - gesture.Delta;
                    var oldPosTwo = gesture.Position2 - gesture.Delta2;
                    var oldDistance = Vector2.Distance(oldPosOne, oldPosTwo);

                    // set new zoom level and clamp it to normal values
                    var newScaleFactor = _camera.Zoom - (oldDistance - currentDistance) * 0.01f;
                    _camera.Zoom = Math.Min(8f, Math.Max(newScaleFactor, 3f));
                }
                else if (gesture.GestureType == GestureType.PinchComplete)
                {
                    CurrentInputState = InputState.Idle;
                }
            }
        }
    }
}
