using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CrawlIT.Shared.GameStates
{
    public abstract class GameState : IGameState
    {
        protected GraphicsDevice _graphicsDevice;

        public GameState(GraphicsDevice graphicsDevice)
        {
            _graphicsDevice = graphicsDevice;
        }

        public abstract void Initialize();

        public abstract void LoadContent(ContentManager content);

        public abstract void SetState(Enum gameState);

        public abstract Enum GetState();

        public abstract void UnloadContent();

        public abstract void Update(GameTime gameTime);

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
