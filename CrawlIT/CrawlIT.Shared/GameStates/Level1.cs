﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace CrawlIT.Shared.GameStates
{
    public class Level1 : GameState
    {
        public Level1(GraphicsDevice graphicsDevice)
        : base(graphicsDevice)
        {
        }

        public override void Initialize()
        {
        }

        public override void LoadContent(ContentManager content)
        {
        }

        public override void UnloadContent()
        {
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            _graphicsDevice.Clear(Color.DarkSalmon);
            spriteBatch.Begin();
            //Draw the sprites
            spriteBatch.End();
        }
    }
}