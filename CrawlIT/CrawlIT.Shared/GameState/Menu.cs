using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CrawlIT.Shared
{
    public class Menu : GameState
    {
        private Texture2D _startButton;
        private Texture2D _logo;

        private Vector2 _startButtonPosition;
        public Point StartButtonPoint { get; private set; }
        private Vector2 _logoPosition;

        private readonly Matrix _transform;
        private readonly Point _resolution;

        public override StateType State { get; }

        public Menu(GraphicsDevice graphicsDevice, Point resolution, Matrix transform)
            : base(graphicsDevice)
        {
            _transform = transform;
            _resolution = resolution;
            State = StateType.Menu;
        }


        public override void Initialize()
        {
        }

        public override void LoadContent(ContentManager content)
        {
            _startButton = content.Load<Texture2D>("Buttons/start");
            _logo = content.Load<Texture2D>("Images/logo");

            _startButtonPosition = new Vector2((_resolution.X - _startButton.Width) * 0.5f,
                                               _resolution.Y * 0.7f - _startButton.Height * 0.5f);
            _logoPosition = new Vector2((_resolution.X - _logo.Width) * 0.5f,
                                        _resolution.Y * 0.3f - _logo.Height * 0.5f);

            StartButtonPoint = _startButtonPosition.ToPoint();
        }

        public override void UnloadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            GraphicsDevice.Clear(Color.Aquamarine);
            spriteBatch.Begin(transformMatrix: _transform, samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(_startButton, _startButtonPosition, Color.White);
            spriteBatch.Draw(_logo, _logoPosition, Color.White);
            spriteBatch.End();
        }
    }
}
