﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CrawlIT.Shared.State.Game
{
    // TODO: This class should probably contain everything related to the exploration mode
    public class Level : GameState
    {

        public override GameStateType State { get; }
        public Level(GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
            State = GameStateType.Playing;
        }

        public override void Initialize()
        {
        }

        public override void LoadContent(ContentManager content)
        {
        }
        public override void Dispose()
        {
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            spriteBatch.End();
        }
    }
}
