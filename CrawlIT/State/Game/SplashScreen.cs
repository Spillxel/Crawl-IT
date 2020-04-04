using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;
using Point = Microsoft.Xna.Framework.Point;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace CrawlIT.Shared
{
    public class SplashScreen : GameState
    {
        private Color _splashColor;
        private Texture2D _splashTexture;
        private Animation _splashAnimation;

        private Vector2 _splashPosition;

        private readonly Matrix _transform;
        private readonly float _scale;
        private readonly Point _resolution;

        public override GameStateType State { get; }

        public SplashScreen(GraphicsDevice graphicsDevice, Point resolution, Matrix transform, float scale)
            : base(graphicsDevice)
        {
            _transform = transform;
            _scale = scale;
            _resolution = resolution;
            State = GameStateType.Splash;
        }


        public override void Initialize()
        {
            _splashColor = new Color(0, 28, 32); // #001C20
        }

        public override void LoadContent(ContentManager content)
        {
            _splashTexture = content.Load<Texture2D>("Sprites/splash_sprite_sheet");

            _splashAnimation = new Animation();
            foreach (var i in Enumerable.Range(0, 10))
            {
                _splashAnimation.AddFrame(
                    new Rectangle(
                        0,
                        i * _splashTexture.Width,
                        _splashTexture.Width, 
                        _splashTexture.Width),
                        TimeSpan.FromSeconds(.200));
            }

            _splashPosition = new Vector2(
                (_resolution.X - _splashTexture.Width * _scale) * 0.5f,
                (_resolution.Y - _splashTexture.Width * _scale) * 0.5f);
        }

        public override void Dispose()
        {
        }

        public override void Update(GameTime gameTime)
        {
            _splashAnimation.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            GraphicsDevice.Clear(_splashColor);
            spriteBatch.Begin(transformMatrix: _transform, samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(
                _splashTexture,
                _splashPosition,
                _splashAnimation.CurrentRectangle,
                Color.White, 0,
                Vector2.Zero,
                _scale,
                SpriteEffects.None,
                0);
            spriteBatch.End();
        }
    }
}
