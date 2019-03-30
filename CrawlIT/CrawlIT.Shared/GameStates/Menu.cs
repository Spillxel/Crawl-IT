using System;
using Java.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace CrawlIT.Shared.GameStates
{
    public class Menu : GameState
    {
        private Enum _state;

        private Texture2D _startButton;
        private Texture2D _exitButton;
        private Texture2D _logo;

        private Vector2 _startButtonPosition;
        private Vector2 _exitButtonPosition;
        private Vector2 _logoPosition;

        private float _zoom;

        private Vector2 _scale => new Vector2(_zoom, _zoom);

        public Menu(GraphicsDevice graphicsDevice, float zoom)
        : base(graphicsDevice)
        {
            _zoom = zoom;
        }

        public override void Initialize()
        {
        }

        public override void LoadContent(ContentManager content)
        {
            _startButton = content.Load<Texture2D>(@"start");
            _exitButton = content.Load<Texture2D>(@"exit");
            _logo = content.Load<Texture2D>(@"logo");
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

            _startButtonPosition = new Vector2(_graphicsDevice.Viewport.Width / 2 - (_startButton.Width * _zoom) / 2,
                                               _graphicsDevice.Viewport.Height * 0.8f);
            _exitButtonPosition = new Vector2(_graphicsDevice.Viewport.Width / 2 - (_exitButton.Width * _zoom) / 2,
                                              _graphicsDevice.Viewport.Height * 0.9f);
            _logoPosition = new Vector2(_graphicsDevice.Viewport.Width / 2 - (_logo.Width * _zoom/2.5f) / 2,
                                        _graphicsDevice.Viewport.Height * 0.3f);

            _graphicsDevice.Clear(Color.Aquamarine);
            spriteBatch.Begin();
            spriteBatch.Draw(texture: _startButton, position: _startButtonPosition, scale: _scale, color: Color.White);
            spriteBatch.Draw(texture: _exitButton, position: _exitButtonPosition, scale: _scale, color: Color.White);
            spriteBatch.Draw(texture: _logo, position: _logoPosition, scale: _scale/2.5f, color: Color.White);
            spriteBatch.End();
        }

        public override Point GetPosition(Texture2D button)
        {
            if(button.Equals(_startButton))
            {
                return new Point((int)_startButtonPosition.X, (int)_startButtonPosition.Y);
            }
            else if(button.Equals(_exitButton))
            {
                return new Point((int)_exitButtonPosition.X, (int)_exitButtonPosition.Y);
            }
            else
            {
                return new Point(0, 0);
            }
        }

    }
}
