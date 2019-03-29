using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace CrawlIT.Shared.GameStates
{
    public class Menu : GameState
    {
        private string _state;

        private Texture2D _startButton;
        private Texture2D _exitButton;

        private Vector2 _startButtonPosition;
        private Vector2 _exitButtonPosition;

        private Vector2 _scale;

        public Menu(GraphicsDevice graphicsDevice)
        : base(graphicsDevice)
        {
        }

        public override void Initialize()
        {
        }

        public override void LoadContent(ContentManager content)
        {
            _startButton = content.Load<Texture2D>(@"start");
            _exitButton = content.Load<Texture2D>(@"exit");
        }

        public override void SetState(string state)
        {
            _state = state;
        }

        public override string GetState()
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
            _graphicsDevice.Clear(Color.Aquamarine);
            spriteBatch.Begin();

            _startButtonPosition = new Vector2(80, 800);
            _exitButtonPosition = new Vector2(80, 1000);

            _scale = new Vector2(6.0f, 6.0f);

            spriteBatch.Draw(texture: _startButton, position: _startButtonPosition, scale: _scale, color: Color.White);
            spriteBatch.Draw(texture: _exitButton, position: _exitButtonPosition, scale: _scale, color: Color.White);
            spriteBatch.End();
        }
    
    }
}
