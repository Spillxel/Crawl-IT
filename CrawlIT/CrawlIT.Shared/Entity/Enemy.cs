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

        private readonly Vector2 _questionMarkPosition;

        public int Fights;
        public int Questions;

        public Rectangle CollisionRectangle;
        public Rectangle FightRectangle;
        public Rectangle FightZoneRectangle;

        public readonly Texture2D CloseUpTexture;
        public readonly Texture2D QuestionMarkTexture;

        public Enemy(ContentManager contentManager, string texture, string closeUpTexture, Vector2 position, Point frame, int fights, int questions)
        {
            TextureSheet = contentManager.Load<Texture2D>(texture);
            Position = position;
            Frame = frame;

            CloseUpTexture = contentManager.Load<Texture2D>(closeUpTexture);
            QuestionMarkTexture = contentManager.Load<Texture2D>("Sprites/placeholder_questionmark_spritesheet");
            Fights = fights;
            Questions = questions;

            CollisionRectangle = new Rectangle(Position.ToPoint(), new Point(Frame.X, (int) (Frame.Y * 0.65) ));

            // The question mark texture sheet's frames are drawn width-wise, since its frames are squares, we just
            // use height wherever needed.
            _questionMarkPosition = new Vector2(
                Position.X + CollisionRectangle.Width * 0.5f - QuestionMarkTexture.Height * 0.5f,
                Position.Y - QuestionMarkTexture.Height);
            // Clickable area where question mark is drawn
            FightRectangle = new Rectangle(_questionMarkPosition.ToPoint(), new Point(QuestionMarkTexture.Height));

            // Enemy center coordinate
            var (centerX, centerY) = new Rectangle(Position.ToPoint(), Frame).Center;
            FightZoneRectangle = new Rectangle(centerX - Frame.Y, centerY - Frame.Y, Frame.Y * 2, Frame.Y * 2);

            StandUp = new Animation();
            StandUp.AddFrame(new Rectangle(0, Frame.Y * 3, Frame.X, Frame.Y), TimeSpan.FromSeconds(3.75));

            StandDown = new Animation();
            StandDown.AddFrame(new Rectangle(0, 0, Frame.X, Frame.Y), TimeSpan.FromSeconds(3.75));
            StandDown.AddFrame(new Rectangle(0, Frame.Y * 4, Frame.X, Frame.Y), TimeSpan.FromSeconds(0.25));

            StandLeft = new Animation();
            StandLeft.AddFrame(new Rectangle(0, Frame.Y, Frame.X, Frame.Y), TimeSpan.FromSeconds(3.75));
            StandLeft.AddFrame(new Rectangle(0, Frame.Y * 5, Frame.X, Frame.Y), TimeSpan.FromSeconds(0.25));

            StandRight = new Animation();
            StandRight.AddFrame(new Rectangle(0, Frame.Y * 2, Frame.X, Frame.Y), TimeSpan.FromSeconds(3.75));
            StandRight.AddFrame(new Rectangle(0, Frame.Y * 6, Frame.X, Frame.Y), TimeSpan.FromSeconds(0.25));

            CurrentAnimation = StandDown;

            QuestionMarkAnimation = new Animation();
            QuestionMarkAnimation.AddFrame(
                new Rectangle(
                    QuestionMarkTexture.Height * 2, 0,
                    QuestionMarkTexture.Height, QuestionMarkTexture.Height),
                TimeSpan.FromSeconds(0.125));
            QuestionMarkAnimation.AddFrame(
                new Rectangle(
                    QuestionMarkTexture.Height * 1, 0,
                    QuestionMarkTexture.Height, QuestionMarkTexture.Height),
                TimeSpan.FromSeconds(0.125));
            QuestionMarkAnimation.AddFrame(
                new Rectangle(
                    QuestionMarkTexture.Height * 0, 0,
                    QuestionMarkTexture.Height, QuestionMarkTexture.Height),
                TimeSpan.FromSeconds(0.125));
            QuestionMarkAnimation.AddFrame(
                new Rectangle(
                    QuestionMarkTexture.Height * 1, 0,
                    QuestionMarkTexture.Height, QuestionMarkTexture.Height),
                TimeSpan.FromSeconds(0.125));
            QuestionMarkAnimation.AddFrame(
                new Rectangle(
                    QuestionMarkTexture.Height * 2, 0,
                    QuestionMarkTexture.Height, QuestionMarkTexture.Height),
                TimeSpan.FromSeconds(0.125));
            QuestionMarkAnimation.AddFrame(
                new Rectangle(
                    QuestionMarkTexture.Height * 3, 0,
                    QuestionMarkTexture.Height, QuestionMarkTexture.Height),
                TimeSpan.FromSeconds(0.125));
            QuestionMarkAnimation.AddFrame(
                new Rectangle(
                    QuestionMarkTexture.Height * 4, 0,
                    QuestionMarkTexture.Height, QuestionMarkTexture.Height),
                TimeSpan.FromSeconds(0.125));
            QuestionMarkAnimation.AddFrame(
                new Rectangle(
                    QuestionMarkTexture.Height * 3, 0,
                    QuestionMarkTexture.Height, QuestionMarkTexture.Height),
                TimeSpan.FromSeconds(0.125));
            QuestionMarkAnimation.AddFrame(
                new Rectangle(
                    QuestionMarkTexture.Height * 2, 0,
                    QuestionMarkTexture.Height, QuestionMarkTexture.Height),
                TimeSpan.FromSeconds(0.125));
        }

        public override void Update(GameTime gameTime)
        {
            CurrentAnimation.Update(gameTime);
            QuestionMarkAnimation.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            var currentAnimationSourceRectangle = CurrentAnimation.CurrentRectangle;
            spriteBatch.Draw(TextureSheet, Position, currentAnimationSourceRectangle, Color.White);
        }

        public void DrawActionIcons(SpriteBatch spriteBatch, bool isInActionZone)
        {
            // TODO: implement talk icon
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
