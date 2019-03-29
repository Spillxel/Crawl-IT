using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;


namespace CrawlIT.Shared.Entity
{
    public class Player : Entity
    {
        private Animation.Animation _walkUp;
        private Animation.Animation _walkDown;
        private Animation.Animation _walkLeft;
        private Animation.Animation _walkRight;

        private Animation.Animation _standUp;
        private Animation.Animation _standDown;
        private Animation.Animation _standLeft;
        private Animation.Animation _standRight;

        private Animation.Animation _currentAnimation;

        private Matrix _scale;

        public Player(Texture2D texture, Matrix scale, float posx = 50, float posy = 50)
        {
            TextureSheet = texture;
            PosX = posx;
            PosY = posy;
            _scale = scale;

            // Rethink this animation frame setup, probably better ways to set this up
            _walkUp = new Animation.Animation();
            _walkUp.AddFrame(new Rectangle(144, 0, 16, 16), TimeSpan.FromSeconds(.25));
            _walkUp.AddFrame(new Rectangle(160, 0, 16, 16), TimeSpan.FromSeconds(.25));
            _walkUp.AddFrame(new Rectangle(144, 0, 16, 16), TimeSpan.FromSeconds(.25));
            _walkUp.AddFrame(new Rectangle(176, 0, 16, 16), TimeSpan.FromSeconds(.25));

            _walkDown = new Animation.Animation();
            _walkDown.AddFrame(new Rectangle(0, 0, 16, 16), TimeSpan.FromSeconds(.25));
            _walkDown.AddFrame(new Rectangle(16, 0, 16, 16), TimeSpan.FromSeconds(.25));
            _walkDown.AddFrame(new Rectangle(0, 0, 16, 16), TimeSpan.FromSeconds(.25));
            _walkDown.AddFrame(new Rectangle(32, 0, 16, 16), TimeSpan.FromSeconds(.25));

            _walkLeft = new Animation.Animation();
            _walkLeft.AddFrame(new Rectangle(48, 0, 16, 16), TimeSpan.FromSeconds(.25));
            _walkLeft.AddFrame(new Rectangle(64, 0, 16, 16), TimeSpan.FromSeconds(.25));
            _walkLeft.AddFrame(new Rectangle(48, 0, 16, 16), TimeSpan.FromSeconds(.25));
            _walkLeft.AddFrame(new Rectangle(80, 0, 16, 16), TimeSpan.FromSeconds(.25));

            _walkRight = new Animation.Animation();
            _walkRight.AddFrame(new Rectangle(96, 0, 16, 16), TimeSpan.FromSeconds(.25));
            _walkRight.AddFrame(new Rectangle(112, 0, 16, 16), TimeSpan.FromSeconds(.25));
            _walkRight.AddFrame(new Rectangle(96, 0, 16, 16), TimeSpan.FromSeconds(.25));
            _walkRight.AddFrame(new Rectangle(128, 0, 16, 16), TimeSpan.FromSeconds(.25));

            _standUp = new Animation.Animation();
            _standUp.AddFrame(new Rectangle(144, 0, 16, 16), TimeSpan.FromSeconds(.25));

            _standDown = new Animation.Animation();
            _standDown.AddFrame(new Rectangle(0, 0, 16, 16), TimeSpan.FromSeconds(.25));

            _standLeft = new Animation.Animation();
            _standLeft.AddFrame(new Rectangle(48, 0, 16, 16), TimeSpan.FromSeconds(.25));

            _standRight = new Animation.Animation();
            _standRight.AddFrame(new Rectangle(96, 0, 16, 16), TimeSpan.FromSeconds(.25));
        }

        public override void Update(GameTime gameTime)
        {
            var velocity = GetVelocity();

            PosX += velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
            PosY += velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;

            SetAnimaton(velocity);

            _currentAnimation.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 position = new Vector2(PosX, PosY);
            var sourceRectangle = _currentAnimation.CurrentRectangle;
            spriteBatch.Draw(TextureSheet, position, sourceRectangle, Color.White);
        }

        private Vector2 GetVelocity()
        {
            Vector2 velocity = new Vector2();

            TouchCollection touchCollection = TouchPanel.GetState();

            // if there is touch input
            if (touchCollection.Count > 0)
            {
                var touchPoint = ScaleInput(touchCollection[0].Position);
                velocity.X = touchPoint.X - (720 / 2);
                velocity.Y = touchPoint.Y - (1280 / 2);

                if (velocity.X != 0 || velocity.Y != 0)
                {
                    velocity.Normalize();
                    const float desiredSpeed = 170;
                    velocity *= desiredSpeed;
                }
            }

            return velocity;
        }

        private void SetAnimaton(Vector2 velocity)
        {
            if (velocity != Vector2.Zero)
            {
                var movingHorizontally = Math.Abs(velocity.X) > Math.Abs(velocity.Y);
                _currentAnimation =
                    movingHorizontally
                        ? (velocity.X > 0 ? _walkRight : _walkLeft)
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
    }
}
