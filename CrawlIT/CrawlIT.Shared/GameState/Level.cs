﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CrawlIT.Shared.GameState
{
    public class Level : GameState
    {
        private Enum _state;

        private Texture2D _pauseButton;

        private Vector2 _pauseButtonPosition;

        public Level(GraphicsDevice graphicsDevice)
        : base(graphicsDevice)
        {
        }

        public override void Initialize()
        {
        }

        public override void LoadContent(ContentManager content)
        {
            _pauseButton = content.Load<Texture2D>("Buttons/pause");
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
            _pauseButtonPosition = new Vector2(10, 10);

            GraphicsDevice.Clear(Color.DarkSalmon);
            spriteBatch.Begin();
            spriteBatch.Draw(_pauseButton, _pauseButtonPosition, Color.White);
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

        public override void ChangeTexture(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        public override void Help(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }
    }
}
