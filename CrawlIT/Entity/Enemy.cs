using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CrawlIT.Shared
{
    public class Enemy : Character
    {
        public Animation CurrentAnimation;
        public Animation QuestionMarkAnimation;

        private readonly Vector2 _position;
        private readonly Vector2 _questionMarkPosition;

        public int Fights;
        public int Questions;

        public Rectangle CollisionRectangle;
        public Rectangle FightRectangle;
        public Rectangle FightZoneRectangle;

        public readonly Texture2D CloseUpTexture;
        public readonly Texture2D QuestionMarkTexture;

        public Enemy(ContentManager contentManager, string texture, string closeUpTexture, float posx, float posy, int fights, int questions)
        {
            TextureSheet = contentManager.Load<Texture2D>(texture);
            CloseUpTexture = contentManager.Load<Texture2D>(closeUpTexture);
            QuestionMarkTexture = contentManager.Load<Texture2D>("Sprites/placeholder_questionmark_spritesheet");
            PosX = posx;
            PosY = posy;
            _position = new Vector2(PosX, PosY);
            _questionMarkPosition = new Vector2(PosX, PosY - 32);
            FrameWidth = 23;
            FrameHeight = 47;
            Fights = fights;
            Questions = questions;

            CollisionRectangle = new Rectangle((int)PosX, (int)PosY, FrameWidth, (int)(FrameHeight / 1.5));
            FightRectangle = new Rectangle((int)_questionMarkPosition.X, (int)_questionMarkPosition.Y, QuestionMarkTexture.Width, QuestionMarkTexture.Height);

            // Action zone
            var (centerPosX, centerPosY) = (PosX + FrameWidth * 0.5, PosY + FrameHeight * 0.5);
            FightZoneRectangle = new Rectangle((int)centerPosX - FrameHeight, (int)centerPosY - FrameHeight, (int) (FrameHeight * 2), FrameHeight * 2);

            StandUp = new Animation();
            StandUp.AddFrame(new Rectangle(0, FrameHeight * 3, FrameWidth, FrameHeight), TimeSpan.FromSeconds(3.75));

            StandDown = new Animation();
            StandDown.AddFrame(new Rectangle(0, 0, FrameWidth, FrameHeight), TimeSpan.FromSeconds(3.75));
            StandDown.AddFrame(new Rectangle(0, FrameHeight * 4, FrameWidth, FrameHeight), TimeSpan.FromSeconds(0.25));

            StandLeft = new Animation();
            StandLeft.AddFrame(new Rectangle(0, FrameHeight, FrameWidth, FrameHeight), TimeSpan.FromSeconds(3.75));
            StandLeft.AddFrame(new Rectangle(0, FrameHeight * 5, FrameWidth, FrameHeight), TimeSpan.FromSeconds(0.25));

            StandRight = new Animation();
            StandRight.AddFrame(new Rectangle(0, FrameHeight * 2, FrameWidth, FrameHeight), TimeSpan.FromSeconds(3.75));
            StandRight.AddFrame(new Rectangle(0, FrameHeight * 6, FrameWidth, FrameHeight), TimeSpan.FromSeconds(0.25));

            CurrentAnimation = StandDown;

            QuestionMarkAnimation = new Animation();
            QuestionMarkAnimation.AddFrame(new Rectangle(24 * 2, 0, 24, 24), TimeSpan.FromSeconds(0.125));
            QuestionMarkAnimation.AddFrame(new Rectangle(24 * 1, 0, 24, 24), TimeSpan.FromSeconds(0.125));
            QuestionMarkAnimation.AddFrame(new Rectangle(24 * 0, 0, 24, 24), TimeSpan.FromSeconds(0.125));
            QuestionMarkAnimation.AddFrame(new Rectangle(24 * 1, 0, 24, 24), TimeSpan.FromSeconds(0.125));
            QuestionMarkAnimation.AddFrame(new Rectangle(24 * 2, 0, 24, 24), TimeSpan.FromSeconds(0.125));
            QuestionMarkAnimation.AddFrame(new Rectangle(24 * 3, 0, 24, 24), TimeSpan.FromSeconds(0.125));
            QuestionMarkAnimation.AddFrame(new Rectangle(24 * 4, 0, 24, 24), TimeSpan.FromSeconds(0.125));
            QuestionMarkAnimation.AddFrame(new Rectangle(24 * 3, 0, 24, 24), TimeSpan.FromSeconds(0.125));
            QuestionMarkAnimation.AddFrame(new Rectangle(24 * 2, 0, 24, 24), TimeSpan.FromSeconds(0.125));
        }

        public override void Update(GameTime gameTime)
        {
            CurrentAnimation.Update(gameTime);
            QuestionMarkAnimation.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Draw(spriteBatch, false);
        }

        public void Draw(SpriteBatch spriteBatch, bool isInActionZone)
        {
            var currentAnimationSourceRectangle = CurrentAnimation.CurrentRectangle;
            spriteBatch.Draw(TextureSheet, _position, currentAnimationSourceRectangle, Color.White);
            if (!isInActionZone) return;
            var questionSourceRectangle = QuestionMarkAnimation.CurrentRectangle;
            spriteBatch.Draw(QuestionMarkTexture, _questionMarkPosition, questionSourceRectangle, Color.White);
        }

        public virtual void BeatenBy(Player player)
        {
            player.SetCrystalCount(++player.CrystalCount);
            if (Fights == 0)
            {
                // Behaviour when no more questions
            }
        }

        public virtual void Beat(Player player)
        {
            player.SetLifeCount(--player.LifeCount);
            // TODO: Insert dialogue when you lose against enemy
        }
    }
}
