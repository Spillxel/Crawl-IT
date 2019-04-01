using System;
using System.Collections.Generic;
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

        private Rectangle currentCollision;

        public List<Rectangle> CollisionObjects { get; set; }

        public Vector2 CurrentVelocity { get; set; }

        //For collision
        public Rectangle Rectangle
        {
	        get
	        {
		        return new Rectangle((int)PosX, (int)PosY, 16, 16); //store frame size in separate + create rectangle outside
            }
        }

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

            var oldPosX = PosX;
            var oldPosY = PosY;

            PosX += velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
            PosY += velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (Collides())
            {
                PosX = oldPosX;
                PosY = oldPosY;

                //if (_collision.IsCollisionTile(_player.Rectangle))
                //{
                //    if ((_player.CurrentVelocity.Y > 0 && _collision.HitsFromTheTop(_player)) ||
                //        (_player.CurrentVelocity.Y < 0 && _collision.HitsFromTheBottom(_player)))
                //    {
                //        _player.CurrentVelocity = new Vector2(_player.CurrentVelocity.X, 0);
                //    }

                //    if ((_player.CurrentVelocity.X < 0 && _collision.HitsFromTheRight(_player) ||
                //        _player.CurrentVelocity.X > 0 && _collision.HitsFromTheLeft(_player)))
                //    {
                //        _player.CurrentVelocity = new Vector2(0, _player.CurrentVelocity.Y);
                //    }
                //} 

            }

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

        public bool Collides()
        {
            foreach (Rectangle rect in CollisionObjects)
                if (rect.Intersects(this.Rectangle))
                {
                    currentCollision = rect;
                    return true;
                }
            return false;
        }

            //Player hits object from the top
            //public bool HitsFromTheTop()
            //{
            //    return Rectangle.Bottom + CurrentVelocity.Y > this.CurrentCollision.Top &&
            //           Rectangle.Top < .Right &&
            //           Rectangle.Right > .Left &&
            //           Rectangle.Left < .Right;
            //}
            //Player hits object from the bottom
            //public bool HitsFromTheBottom()
            //{
            //    return player.Rectangle.Top + player.CurrentVelocity.Y < this.CurrentCollision.Bottom &&
            //            player.Rectangle.Bottom > this.CurrentCollision.Bottom &&
            //            player.Rectangle.Right > this.CurrentCollision.Left &&
            //            player.Rectangle.Left < this.CurrentCollision.Right;
            //}
            ////Player hits object from the right
            //public bool HitsFromTheRight()
            //{
            //    return player.Rectangle.Left + player.CurrentVelocity.X < this.CurrentCollision.Right &&
            //            player.Rectangle.Right > this.CurrentCollision.Right &&
            //            player.Rectangle.Bottom > this.CurrentCollision.Top &&
            //            player.Rectangle.Top < this.CurrentCollision.Bottom;
            //}
            ////Player hits object from the left
            //public bool HitsFromTheLeft()
            //{
            //    return player.Rectangle.Right + player.CurrentVelocity.X > this.CurrentCollision.Left &&
            //           player.Rectangle.Left < this.CurrentCollision.Left &&
            //           player.Rectangle.Bottom > this.CurrentCollision.Top &&
            //           player.Rectangle.Top < this.CurrentCollision.Bottom;
            //}

        }
}
