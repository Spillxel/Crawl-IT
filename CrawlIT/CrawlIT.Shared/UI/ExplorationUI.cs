using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CrawlIT.Shared
{
    public class ExplorationUi
    {
        private UiIcon _lifeBar;
        private UiIcon _surgeCrystal;
        private UiIcon _save;
        private UiIcon _inventory;
        private UiIcon _help;
        private UiIcon _badges;

        private Texture2D _lifeBarTexture;
        private Texture2D _surgeCrystalTexture;
        private Texture2D _saveTexture;
        private Texture2D _inventoryTexture;
        private Texture2D _helpTexture;
        private Texture2D _badgesTexture;

        private Matrix _scale;
        private Point _resolution;
        private ContentManager _content;
        private Camera _staticCamera;
        private SpriteFont _font;
        private SpriteBatch _spriteBatch;
        private Player _player;

        public ExplorationUi(Matrix scale, Point resolution, ContentManager content, Camera staticCamera, Player player)
        {
            _scale = scale;
            _resolution = resolution;
            _content = content;
            _staticCamera = staticCamera;
            _player = player;
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void Load()
        {
            _lifeBarTexture = _content.Load<Texture2D>("Sprites/lifebarspritesheet");
            _saveTexture = _content.Load<Texture2D>("Sprites/save");
            _inventoryTexture = _content.Load<Texture2D>("Sprites/iconplaceholder");
            _helpTexture = _content.Load<Texture2D>("Sprites/help");
            _badgesTexture = _content.Load<Texture2D>("Sprites/iconplaceholder");
            _surgeCrystalTexture = _content.Load<Texture2D>("Sprites/surgecrystal");

            _font = _content.Load<SpriteFont>("Fonts/File");

            var zoom = 1;
            _lifeBar = new LifeBarIcon(_lifeBarTexture, _scale, 50, 50, _player);
            _surgeCrystal = new UiIcon(_surgeCrystalTexture, _scale, _resolution.X - (32 * zoom + 50), 50);
            _save = new UiIcon(_saveTexture, _scale, _resolution.X / 4 + 50, _resolution.Y - 50 - 32 * zoom);
            _inventory = new UiIcon(_inventoryTexture, _scale, _resolution.X - (32 * zoom + 50), _resolution.Y - 50 - 32 * zoom);
            _help = new UiIcon(_helpTexture, _scale, 50, _resolution.Y - 50 - 32 * zoom);
            _badges = new UiIcon(_badgesTexture, _scale, _resolution.X * 0.5f, _resolution.Y - 50);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;

            var levelString = "Semester 1";
            var (levelStringDimensionX, levelStringDimensionY) = _font.MeasureString(levelString);
            var levelStringPosX = (_resolution.X - levelStringDimensionX) / 2;
            var levelStringPosY = 50;

            _spriteBatch.Begin();
            _spriteBatch.DrawString(_font, levelString, new Vector2(levelStringPosX, levelStringPosY), Color.White);
            _spriteBatch.End();

            _spriteBatch.Begin(SpriteSortMode.BackToFront,
                  BlendState.AlphaBlend,
                  SamplerState.PointClamp, null, null, null,
                  _scale);
            _lifeBar.Draw(_spriteBatch);
            _surgeCrystal.Draw(_spriteBatch);
            _save.Draw(_spriteBatch);
            _inventory.Draw(_spriteBatch);
            _help.Draw(_spriteBatch);
            _badges.Draw(_spriteBatch);
            _spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            _lifeBar.Update(gameTime);
        }
    }
}
