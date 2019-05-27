using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CrawlIT.Shared.Entity
{

    public class Enemy : Character
    {
        public Animation.Animation CurrentAnimation;

        private readonly Matrix _scale;

        private readonly Vector2 _position;

        public int FightsLeft;
        public int QuestionPerFight;

        public Rectangle CollisionRectangle;
        public Rectangle FightRectangle;

        public Texture2D CloseUpTexture;

        public Enemy(Texture2D texture, Texture2D closeUpTexture, Matrix scale, float posx, float posy, int fightsLeft, int questionPerFight)
        {
            TextureSheet = texture;
            CloseUpTexture = closeUpTexture;
            PosX = posx;
            PosY = posy;
            _position = new Vector2(PosX, PosY);
            FrameWidth = 23;
            FrameHeight = 45;
            _scale = scale;
            FightsLeft = fightsLeft;
            QuestionPerFight = questionPerFight;

            CollisionRectangle = new Rectangle((int)PosX, (int)PosY, FrameWidth, (int)(FrameHeight / 1.5));
            FightRectangle = new Rectangle((int)PosX -1, (int)PosY -1, FrameWidth + 2, (int)(FrameHeight / 1.5) + 2);

            StandUp = new Animation.Animation();
            StandUp.AddFrame(new Rectangle(FrameWidth * 3, 0, FrameWidth, FrameHeight), TimeSpan.FromSeconds(1));

            StandDown = new Animation.Animation();
            StandDown.AddFrame(new Rectangle(0, 0, FrameWidth, FrameHeight), TimeSpan.FromSeconds(1));

            StandLeft = new Animation.Animation();
            StandLeft.AddFrame(new Rectangle(FrameWidth, 0, FrameWidth, FrameHeight), TimeSpan.FromSeconds(1));

            StandRight = new Animation.Animation();
            StandRight.AddFrame(new Rectangle(FrameWidth * 2, 0, FrameWidth, FrameHeight), TimeSpan.FromSeconds(1));

            CurrentAnimation = StandDown;
        }

        public override void Update(GameTime gameTime)
        {
            CurrentAnimation.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var sourceRectangle = CurrentAnimation.CurrentRectangle;
            spriteBatch.Draw(TextureSheet, _position, sourceRectangle, Color.White);
        }
    }
}
