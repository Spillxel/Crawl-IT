using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace CrawlIT.Shared
{
    public class Player : Character
    {
        private readonly Animation _walkUp;
        private readonly Animation _walkDown;
        private readonly Animation _walkLeft;
        private readonly Animation _walkRight;

        private Animation _currentAnimation;

        private readonly Matrix _scale;
        
        private const float Speed = 170;

        public List<Rectangle> CollisionObjects { get; set; }
        public List<Enemy> Enemies { get; set; }

        public int LifeCount;
        public int CrystalCount;

        private Vector2 _currentVelocity;

        // For collision
        public Rectangle CollisionRectangle => new Rectangle((int)PosX, (int)PosY, FrameWidth, FrameHeight);

        public Player(Texture2D texture, Matrix scale, float posX = 50, float posY = 70)
        {
            TextureSheet = texture;
            PosX = posX;
            PosY = posY;
            _scale = scale;
            FrameWidth = 23;
            FrameHeight = 45;
            LifeCount = 3;
            CrystalCount = 0;

            // TODO: rethink this animation frame setup, probably better ways to set this up
            _walkDown = new Animation();
            _walkDown.AddFrame(new Rectangle(0, 0, FrameWidth, FrameHeight), TimeSpan.FromSeconds(.125));
            _walkDown.AddFrame(new Rectangle(FrameWidth, 0, FrameWidth, FrameHeight), TimeSpan.FromSeconds(.125));
            _walkDown.AddFrame(new Rectangle(FrameWidth * 2, 0, FrameWidth, FrameHeight), TimeSpan.FromSeconds(.125));
            _walkDown.AddFrame(new Rectangle(FrameWidth, 0, FrameWidth, FrameHeight), TimeSpan.FromSeconds(.125));
            _walkDown.AddFrame(new Rectangle(0, 0, FrameWidth, FrameHeight), TimeSpan.FromSeconds(.125));
            _walkDown.AddFrame(new Rectangle(FrameWidth * 3, 0, FrameWidth, FrameHeight), TimeSpan.FromSeconds(.125));
            _walkDown.AddFrame(new Rectangle(FrameWidth * 4, 0, FrameWidth, FrameHeight), TimeSpan.FromSeconds(.125));
            _walkDown.AddFrame(new Rectangle(FrameWidth * 3, 0, FrameWidth, FrameHeight), TimeSpan.FromSeconds(.125));

            _walkUp = new Animation();
            _walkUp.AddFrame(new Rectangle(0, FrameHeight * 3, FrameWidth, FrameHeight), TimeSpan.FromSeconds(.125));
            _walkUp.AddFrame(new Rectangle(FrameWidth, FrameHeight * 3, FrameWidth, FrameHeight), TimeSpan.FromSeconds(.125));
            _walkUp.AddFrame(new Rectangle(FrameWidth * 2, FrameHeight * 3, FrameWidth, FrameHeight), TimeSpan.FromSeconds(.125));
            _walkUp.AddFrame(new Rectangle(FrameWidth, FrameHeight * 3, FrameWidth, FrameHeight), TimeSpan.FromSeconds(.125));
            _walkUp.AddFrame(new Rectangle(0, FrameHeight * 3, FrameWidth, FrameHeight), TimeSpan.FromSeconds(.125));
            _walkUp.AddFrame(new Rectangle(FrameWidth * 3, FrameHeight * 3, FrameWidth, FrameHeight), TimeSpan.FromSeconds(.125));
            _walkUp.AddFrame(new Rectangle(FrameWidth * 4, FrameHeight * 3, FrameWidth, FrameHeight), TimeSpan.FromSeconds(.125));
            _walkUp.AddFrame(new Rectangle(FrameWidth * 3, FrameHeight * 3, FrameWidth, FrameHeight), TimeSpan.FromSeconds(.125));

            _walkLeft = new Animation();
            _walkLeft.AddFrame(new Rectangle(0, FrameHeight, FrameWidth, FrameHeight), TimeSpan.FromSeconds(.125));
            _walkLeft.AddFrame(new Rectangle(FrameWidth, FrameHeight, FrameWidth, FrameHeight), TimeSpan.FromSeconds(.125));
            _walkLeft.AddFrame(new Rectangle(FrameWidth * 2, FrameHeight, FrameWidth, FrameHeight), TimeSpan.FromSeconds(.125));
            _walkLeft.AddFrame(new Rectangle(FrameWidth, FrameHeight, FrameWidth, FrameHeight), TimeSpan.FromSeconds(.125));
            _walkLeft.AddFrame(new Rectangle(0, FrameHeight, FrameWidth, FrameHeight), TimeSpan.FromSeconds(.125));
            _walkLeft.AddFrame(new Rectangle(FrameWidth * 3, FrameHeight, FrameWidth, FrameHeight), TimeSpan.FromSeconds(.125));
            _walkLeft.AddFrame(new Rectangle(FrameWidth * 4, FrameHeight, FrameWidth, FrameHeight), TimeSpan.FromSeconds(.125));
            _walkLeft.AddFrame(new Rectangle(FrameWidth * 3, FrameHeight, FrameWidth, FrameHeight), TimeSpan.FromSeconds(.125));

            _walkRight = new Animation();
            _walkRight.AddFrame(new Rectangle(0, FrameHeight * 2, FrameWidth, FrameHeight), TimeSpan.FromSeconds(.125));
            _walkRight.AddFrame(new Rectangle(FrameWidth, FrameHeight * 2, FrameWidth, FrameHeight), TimeSpan.FromSeconds(.125));
            _walkRight.AddFrame(new Rectangle(FrameWidth * 2, FrameHeight * 2, FrameWidth, FrameHeight), TimeSpan.FromSeconds(.125));
            _walkRight.AddFrame(new Rectangle(FrameWidth, FrameHeight * 2, FrameWidth, FrameHeight), TimeSpan.FromSeconds(.125));
            _walkRight.AddFrame(new Rectangle(0, FrameHeight * 2, FrameWidth, FrameHeight), TimeSpan.FromSeconds(.125));
            _walkRight.AddFrame(new Rectangle(FrameWidth * 3, FrameHeight * 2, FrameWidth, FrameHeight), TimeSpan.FromSeconds(.125));
            _walkRight.AddFrame(new Rectangle(FrameWidth * 4, FrameHeight * 2, FrameWidth, FrameHeight), TimeSpan.FromSeconds(.125));
            _walkRight.AddFrame(new Rectangle(FrameWidth * 3, FrameHeight * 2, FrameWidth, FrameHeight), TimeSpan.FromSeconds(.125));

            StandUp = new Animation();
            StandUp.AddFrame(new Rectangle(0, FrameHeight * 3, FrameWidth, FrameHeight), TimeSpan.FromSeconds(.125));

            StandDown = new Animation();
            StandDown.AddFrame(new Rectangle(0, 0, FrameWidth, FrameHeight), TimeSpan.FromSeconds(.125));

            StandLeft = new Animation();
            StandLeft.AddFrame(new Rectangle(0, FrameHeight, FrameWidth, FrameHeight), TimeSpan.FromSeconds(.125));

            StandRight = new Animation();
            StandRight.AddFrame(new Rectangle(0, FrameHeight * 2, FrameWidth, FrameHeight), TimeSpan.FromSeconds(.125));
        }

        public override void Update(GameTime gameTime) => _currentAnimation.Update(gameTime);

        public void UpdateMovement(GameTime gameTime, InputManager.InputState inputState)
        {
            _currentVelocity = inputState != InputManager.InputState.Idle ? Vector2.Zero
                                                             : GetVelocity(gameTime);
            SetAnimation(_currentVelocity);

            if (_currentVelocity == Vector2.Zero)
                return;

            foreach (var enemy in Enemies)
            {
                if (_currentVelocity.Y > 0 && CollidesTop(enemy.CollisionRectangle))
                    enemy.CurrentAnimation = enemy.StandUp;
                else if (_currentVelocity.Y < 0 && CollidesBottom(enemy.CollisionRectangle))
                    enemy.CurrentAnimation = enemy.StandDown;
                else if (_currentVelocity.X > 0 && CollidesLeft(enemy.CollisionRectangle))
                    enemy.CurrentAnimation = enemy.StandLeft;
                else if (_currentVelocity.X < 0 && CollidesRight(enemy.CollisionRectangle))
                    enemy.CurrentAnimation = enemy.StandRight;
            }

            foreach (var rect in CollisionObjects)
            {
                if (_currentVelocity.Y > 0 && CollidesTop(rect) || _currentVelocity.Y < 0 && CollidesBottom(rect))
                    _currentVelocity.Y = 0;
                if (_currentVelocity.X > 0 && CollidesLeft(rect) || _currentVelocity.X < 0 && CollidesRight(rect))
                    _currentVelocity.X = 0;
            }

            PosX += _currentVelocity.X;
            PosY += _currentVelocity.Y;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var position = new Vector2(PosX, PosY);
            var sourceRectangle = _currentAnimation.CurrentRectangle;
            spriteBatch.Draw(TextureSheet, position, sourceRectangle, Color.White);
        }

        private Vector2 GetVelocity(GameTime gameTime)
        {
            var velocity = Vector2.Zero;

            var touchCollection = TouchPanel.GetState();
            if (touchCollection.Count <= 0)
                return velocity;    // no input

            // TODO: replace with actual virtual res
            var (touchPointX, touchPointY) = ScaleInput(touchCollection[0].Position);
            velocity.X = touchPointX - 720 * 0.5f;
            velocity.Y = touchPointY - 1280 * 0.5f;

            if (Math.Abs(velocity.X) < 1 && Math.Abs(velocity.Y) < 1)
                return velocity;    // no movement

            // otherwise get new velocity
            velocity.Normalize();
            return velocity * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        private void SetAnimation(Vector2 velocity)
        {
            if (velocity != Vector2.Zero)
            {
                var movingHorizontally = Math.Abs(velocity.X) > Math.Abs(velocity.Y);
                _currentAnimation = movingHorizontally ? velocity.X > 0 ? _walkRight : _walkLeft
                                                       : velocity.Y > 0 ? _walkDown : _walkUp;
            }
            else
            {
                if (_currentAnimation == _walkUp)
                    _currentAnimation = StandUp;
                else if (_currentAnimation == _walkDown)
                    _currentAnimation = StandDown;
                else if (_currentAnimation == _walkLeft)
                    _currentAnimation = StandLeft;
                else if (_currentAnimation == _walkRight)
                    _currentAnimation = StandRight;
                else if (_currentAnimation == null)
                    _currentAnimation = StandDown;
            }
        }

        private Vector2 ScaleInput(Vector2 vector) => Vector2.Transform(vector, Matrix.Invert(_scale));

        public bool Collides(Rectangle rectangle)
        {
            return CollidesLeft(rectangle)
                   || CollidesRight(rectangle)
                   || CollidesTop(rectangle)
                   || CollidesBottom(rectangle);
        }

        public bool CollidesTop(Rectangle rect)
        {
            return CollisionRectangle.Bottom + _currentVelocity.Y > rect.Top
                   && CollisionRectangle.Top < rect.Top
                   && CollisionRectangle.Right > rect.Left
                   && CollisionRectangle.Left < rect.Right;
        }

        public bool CollidesBottom(Rectangle rect)
        {
            return CollisionRectangle.Top + _currentVelocity.Y < rect.Bottom
                   && CollisionRectangle.Bottom > rect.Bottom
                   && CollisionRectangle.Right > rect.Left
                   && CollisionRectangle.Left < rect.Right;
        }

        public bool CollidesRight(Rectangle rect)
        {
            return CollisionRectangle.Left + _currentVelocity.X < rect.Right
                   && CollisionRectangle.Right > rect.Right
                   && CollisionRectangle.Bottom > rect.Top
                   && CollisionRectangle.Top < rect.Bottom;
        }

        public bool CollidesLeft(Rectangle rect)
        {
            return CollisionRectangle.Right + _currentVelocity.X > rect.Left
                   && CollisionRectangle.Left < rect.Left
                   && CollisionRectangle.Bottom > rect.Top
                   && CollisionRectangle.Top < rect.Bottom;
        }

        public void MoveBack(Enemy currentEnemy)
        {
            if (currentEnemy.CurrentAnimation == currentEnemy.StandDown)
                PosY += 15;
            else if (currentEnemy.CurrentAnimation == currentEnemy.StandUp)
                PosY -= 15;
            else if (currentEnemy.CurrentAnimation == currentEnemy.StandRight)
                PosX += 15;
            else if (currentEnemy.CurrentAnimation == currentEnemy.StandLeft)
                PosX -= 15;
        }

        public void SetLifeCount(int count)
        {
            lifeCount = Math.Min(Math.Max(0, count), 3);
        }
    }
}
