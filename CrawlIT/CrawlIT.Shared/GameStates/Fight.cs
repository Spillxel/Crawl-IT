using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace CrawlIT.Shared.GameStates
{
    public class Fight : GameState
    {
        private Enum _state;

        private Texture2D _pauseButton;
        private Texture2D _fight;

        private Vector2 _pauseButtonPosition;
        private Vector2 _fightPosition;
        private Vector2 _scale;

        private float _zoom;

        public Fight(GraphicsDevice graphicsDevice)
        : base(graphicsDevice)
        {
        }

        public override void Initialize()
        {
        }

        public override void LoadContent(ContentManager content)
        {
            _pauseButton = content.Load<Texture2D>(@"pause");
            _fight = content.Load<Texture2D>(@"fight");
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
            _pauseButtonPosition = new Vector2((_graphicsDevice.Viewport.Width - _pauseButton.Width - 10), 10);
            _fightPosition = new Vector2(0, 0);

            _graphicsDevice.Clear(Color.DarkKhaki);
            spriteBatch.Begin();
            spriteBatch.Draw(_pauseButton, _pauseButtonPosition, Color.White);
            spriteBatch.Draw(_fight, _graphicsDevice.Viewport.Bounds, color: Color.White);
            spriteBatch.End();
        }

        public override Point GetPosition(Texture2D button)
        {
            if (button.Equals(_pauseButton))
            {
                return new Point((int)_pauseButtonPosition.X, (int)_pauseButtonPosition.Y);
            }
            else
            {
                return new Point(0, 0);
            }
        }
    }
}
