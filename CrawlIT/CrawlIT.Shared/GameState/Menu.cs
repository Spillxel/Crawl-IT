using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CrawlIT.Shared.GameState
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

        private readonly float _zoom;

        private Vector2 Scale => new Vector2(_zoom, _zoom);

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
            _startButton = content.Load<Texture2D>("Buttons/start");
            _exitButton = content.Load<Texture2D>("Buttons/exit");
            _logo = content.Load<Texture2D>("Images/logo");
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

            _startButtonPosition = new Vector2((GraphicsDevice.Viewport.Width - _startButton.Width * _zoom) / 2,
                                               GraphicsDevice.Viewport.Height * 0.8f);
            _exitButtonPosition = new Vector2((GraphicsDevice.Viewport.Width - _exitButton.Width * _zoom) / 2,
                                              GraphicsDevice.Viewport.Height * 0.9f);
            _logoPosition = new Vector2((GraphicsDevice.Viewport.Width - _logo.Width * _zoom / 2.5f) / 2,
                                        GraphicsDevice.Viewport.Height * 0.3f);

            GraphicsDevice.Clear(Color.Aquamarine);
            spriteBatch.Begin();
            spriteBatch.Draw(_startButton, _startButtonPosition, null, Color.White, 0, Vector2.Zero, 
                             Scale,
                             SpriteEffects.None, 0);
            spriteBatch.Draw(_exitButton, _exitButtonPosition, null, Color.White, 0, Vector2.Zero, 
                             Scale,
                             SpriteEffects.None, 0);
            spriteBatch.Draw(_logo, _logoPosition, null, Color.White, 0, Vector2.Zero, 
                             Scale / 2.5f,
                             SpriteEffects.None, 0);
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

        public override void PopUp(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }
    }
}
