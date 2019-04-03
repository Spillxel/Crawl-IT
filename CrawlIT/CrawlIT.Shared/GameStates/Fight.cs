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

        private Vector2 _pauseButtonPosition;

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
            _pauseButtonPosition = new Vector2(100, 100);

            _graphicsDevice.Clear(Color.DarkKhaki);
            spriteBatch.Begin();
            //Draw the sprites
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
