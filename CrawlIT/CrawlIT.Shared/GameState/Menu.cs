using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CrawlIT.Shared
{
    public class Menu : GameState
    {
        private Enum _state;

        private Texture2D _startButton;
        private Texture2D _exitButton;
        private Texture2D _logo;

        private Vector2 _startButtonPosition;
        public Point StartButtonPoint { get; private set; }
        private Vector2 _exitButtonPosition;
        public Point ExitButtonPoint { get; private set; }
        private Vector2 _logoPosition;

        private readonly Matrix _transform;
        private readonly Point _resolution;

        public Menu(GraphicsDevice graphicsDevice, Point resolution, Matrix transform)
        : base(graphicsDevice)
        {
            _transform = transform;
            _resolution = resolution;
        }

        public override void Initialize()
        {
        }

        public override void LoadContent(ContentManager content)
        {
            _startButton = content.Load<Texture2D>("Buttons/start");
            _exitButton = content.Load<Texture2D>("Buttons/exit");
            _logo = content.Load<Texture2D>("Images/logo");

            _startButtonPosition = new Vector2((_resolution.X - _startButton.Width) * 0.5f,
                                               _resolution.Y * 0.7f - _startButton.Height * 0.5f);
            _exitButtonPosition = new Vector2((_resolution.X - _exitButton.Width) * 0.5f,
                                              _resolution.Y * 0.8f - _exitButton.Height * 0.5f);
            _logoPosition = new Vector2((_resolution.X - _logo.Width) * 0.5f,
                                        _resolution.Y * 0.3f - _logo.Height * 0.5f);

            StartButtonPoint = _startButtonPosition.ToPoint();
            ExitButtonPoint = _exitButtonPosition.ToPoint();
        }

        public override void SetState(Enum gameState)
        {
            _state = gameState;
        }

        public override Enum GetState()
        {
            return _state;
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
            spriteBatch.Draw(_exitButton, _exitButtonPosition, Color.White);
            spriteBatch.Draw(_logo, _logoPosition, Color.White);
            spriteBatch.End();
        }

        public override Point GetPosition(Texture2D button)
        {
            throw new NotImplementedException();
        }

        public override bool GetAnswer(Rectangle touch)
        {
            throw new NotImplementedException();
        }

        public override void ChangeColour(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        public override void Help(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

    }
}
