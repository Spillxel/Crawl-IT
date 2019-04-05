using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace CrawlIT.Shared.Entity
{
    public class Player : Entity
    {
        private readonly Animation.Animation _walkUp;
        private readonly Animation.Animation _walkDown;
        private readonly Animation.Animation _walkLeft;
        private readonly Animation.Animation _walkRight;

        private readonly Animation.Animation _standUp;
        private readonly Animation.Animation _standDown;
        private readonly Animation.Animation _standLeft;
        private readonly Animation.Animation _standRight;

        private Animation.Animation _currentAnimation;

        private readonly Matrix _scale;

        private readonly int _frameWidth;
        private readonly int _frameHeight;

        private const float Speed = 170;

        public List<Rectangle> CollisionObjects { get; set; }

        public Vector2 CurrentVelocity { get; set; }

        // For collision
        public Rectangle Rectangle => new Rectangle((int)PosX, (int)PosY, _frameWidth, _frameHeight);

        public Player(Texture2D texture, Matrix scale, float posx = 50, float posy = 70)
        {
            TextureSheet = texture;
            PosX = posx;
            PosY = posy;
            _scale = scale;
            _frameWidth = 23;
            _frameHeight = 45;

            // TODO: rethink this animation frame setup, probably better ways to set this up
            _walkDown = new Animation.Animation();
            _walkDown.AddFrame(new Rectangle(0, 0, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkDown.AddFrame(new Rectangle(_frameWidth, 0, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkDown.AddFrame(new Rectangle(_frameWidth * 2, 0, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkDown.AddFrame(new Rectangle(_frameWidth, 0, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkDown.AddFrame(new Rectangle(0, 0, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkDown.AddFrame(new Rectangle(_frameWidth * 3, 0, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkDown.AddFrame(new Rectangle(_frameWidth * 4, 0, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkDown.AddFrame(new Rectangle(_frameWidth * 3, 0, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));

            _walkUp = new Animation.Animation();
            _walkUp.AddFrame(new Rectangle(0, _frameHeight * 3, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkUp.AddFrame(new Rectangle(_frameWidth, _frameHeight * 3, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkUp.AddFrame(new Rectangle(_frameWidth * 2, _frameHeight * 3, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkUp.AddFrame(new Rectangle(_frameWidth, _frameHeight * 3, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkUp.AddFrame(new Rectangle(0, _frameHeight * 3, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkUp.AddFrame(new Rectangle(_frameWidth * 3, _frameHeight * 3, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkUp.AddFrame(new Rectangle(_frameWidth * 4, _frameHeight * 3, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkUp.AddFrame(new Rectangle(_frameWidth * 3, _frameHeight * 3, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));

            _walkLeft = new Animation.Animation();
            _walkLeft.AddFrame(new Rectangle(0, _frameHeight, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkLeft.AddFrame(new Rectangle(_frameWidth, _frameHeight, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkLeft.AddFrame(new Rectangle(_frameWidth * 2, _frameHeight, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkLeft.AddFrame(new Rectangle(_frameWidth, _frameHeight, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkLeft.AddFrame(new Rectangle(0, _frameHeight, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkLeft.AddFrame(new Rectangle(_frameWidth * 3, _frameHeight, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkLeft.AddFrame(new Rectangle(_frameWidth * 4, _frameHeight, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkLeft.AddFrame(new Rectangle(_frameWidth * 3, _frameHeight, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));

            _walkRight = new Animation.Animation();
            _walkRight.AddFrame(new Rectangle(0, _frameHeight * 2, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkRight.AddFrame(new Rectangle(_frameWidth, _frameHeight * 2, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkRight.AddFrame(new Rectangle(_frameWidth * 2, _frameHeight * 2, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkRight.AddFrame(new Rectangle(_frameWidth, _frameHeight * 2, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkRight.AddFrame(new Rectangle(0, _frameHeight * 2, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkRight.AddFrame(new Rectangle(_frameWidth * 3, _frameHeight * 2, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkRight.AddFrame(new Rectangle(_frameWidth * 4, _frameHeight * 2, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
            _walkRight.AddFrame(new Rectangle(_frameWidth * 3, _frameHeight * 2, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));

            _standUp = new Animation.Animation();
            _standUp.AddFrame(new Rectangle(0, _frameHeight * 3, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));

            _standDown = new Animation.Animation();
            _standDown.AddFrame(new Rectangle(0, 0, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));

            _standLeft = new Animation.Animation();
            _standLeft.AddFrame(new Rectangle(0, _frameHeight, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));

            _standRight = new Animation.Animation();
            _standRight.AddFrame(new Rectangle(0, _frameHeight * 2, _frameWidth, _frameHeight), TimeSpan.FromSeconds(.125));
        }

        public override void Update(GameTime gameTime)
        {
            var velocity = GetVelocity();

            SetAnimaton(velocity);

            foreach (var rect in CollisionObjects)
            {
                if ((velocity.Y > 0 && CollidesTop(rect)) || (velocity.Y < 0 && CollidesBottom(rect)))
                    velocity.Y = 0;
                if ((velocity.X > 0 && CollidesLeft(rect)) || (velocity.X < 0 && CollidesRight(rect)))
                    velocity.X = 0;
            }

            PosX += velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
            PosY += velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;

            _currentAnimation.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var position = new Vector2(PosX, PosY);
            var sourceRectangle = _currentAnimation.CurrentRectangle;
            spriteBatch.Draw(TextureSheet, position, sourceRectangle, Color.White);
        }

        private Vector2 GetVelocity()
        {
            var velocity = Vector2.Zero;

            var touchCollection = TouchPanel.GetState();
            if (touchCollection.Count <= 0)
                return velocity;    // no input

            var (touchPointX, touchPointY) = ScaleInput(touchCollection[0].Position);
            velocity.X = touchPointX - (720 * 0.5f);
            velocity.Y = touchPointY - (1280 * 0.5f);

            if (Math.Abs(velocity.X) < 1 && Math.Abs(velocity.Y) < 1)
                return velocity;    // no movement

            // otherwise get new velocity
            velocity.Normalize();
            return velocity * Speed;
        }

        private void SetAnimaton(Vector2 velocity)
        {
            if (velocity != Vector2.Zero)
            {
                var movingHorizontally = Math.Abs(velocity.X) > Math.Abs(velocity.Y);
                _currentAnimation = movingHorizontally ? (velocity.X > 0 ? _walkRight : _walkLeft)
                                                       : (velocity.Y > 0 ? _walkDown : _walkUp);
            }
            else
            {
                if (_currentAnimation == _walkUp)
                    _currentAnimation = _standUp;
                else if (_currentAnimation == _walkDown)
                    _currentAnimation = _standDown;
                else if (_currentAnimation == _walkLeft)
                    _currentAnimation = _standLeft;
                else if (_currentAnimation == _walkRight)
                    _currentAnimation = _standRight;
                else if (_currentAnimation == null)
                    _currentAnimation = _standDown;
            }
        }

        private Vector2 ScaleInput(Vector2 vector)
        {
            return Vector2.Transform(vector, Matrix.Invert(_scale));
        }

        public bool CollidesTop(Rectangle rect)
        {
            return Rectangle.Bottom + CurrentVelocity.Y > rect.Top &&
                   Rectangle.Top < rect.Top &&
                   Rectangle.Right > rect.Left &&
                   Rectangle.Left < rect.Right;
        }

        public bool CollidesBottom(Rectangle rect)
        {
            return Rectangle.Top + CurrentVelocity.Y < rect.Bottom &&
                   Rectangle.Bottom > rect.Bottom &&
                   Rectangle.Right > rect.Left &&
                   Rectangle.Left < rect.Right;
        }

        public bool CollidesRight(Rectangle rect)
        {
            return Rectangle.Left + CurrentVelocity.X < rect.Right &&
                   Rectangle.Right > rect.Right &&
                   Rectangle.Bottom > rect.Top &&
                   Rectangle.Top < rect.Bottom;
        }

        public bool CollidesLeft(Rectangle rect)
        {
            return Rectangle.Right + CurrentVelocity.X > rect.Left &&
                   Rectangle.Left < rect.Left &&
                   Rectangle.Bottom > rect.Top &&
                   Rectangle.Top < rect.Bottom;
        }
    }
}
