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

        private Vector2 _startButtonPosition;
        private Vector2 _exitButtonPosition;

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
            _exitButtonPosition = new Vector2(_graphicsDevice.Viewport.Width / 2 - (_startButton.Width * _zoom) / 2,
                                              _graphicsDevice.Viewport.Height * 0.9f);

            _graphicsDevice.Clear(Color.Aquamarine);
            spriteBatch.Begin();
            spriteBatch.Draw(texture: _startButton, position: _startButtonPosition, scale: _scale, color: Color.White);
            spriteBatch.Draw(texture: _exitButton, position: _exitButtonPosition, scale: _scale, color: Color.White);
            spriteBatch.End();
        }
    
    }
}
