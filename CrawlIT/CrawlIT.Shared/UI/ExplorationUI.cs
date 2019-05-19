using System;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CrawlIT.Shared
{
    public class ExplorationUi
    {
        private UiIcon _lifeBar;
        private UiIcon _surgeCrystal;

        private UiIcon _save;
        private UiIcon _settings;
        private UiIcon _help;
        private UiIcon _badges;

        private Texture2D _lifeBarTexture;
        private Texture2D _surgeCrystalTexture;

        private Texture2D _helpTexture;
        private Texture2D _saveTexture;
        private Texture2D _settingsTexture;
        private Texture2D _badgesTexture;

        private SpriteFont _font;

        private readonly Matrix _transform;
        private readonly float _scale;
        private readonly Point _resolution;

        private readonly ContentManager _content;
        private SpriteBatch _spriteBatch;

        private readonly Player _player;

        // TODO: remove
        private string _bottomHeight;

        public ExplorationUi(Matrix transform, float scale, Point resolution, ContentManager content, Player player)
        {
            _transform = transform;
            _scale = scale;
            _resolution = resolution;
            _content = content;
            _player = player;
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void Load()
        {
            _lifeBarTexture = _content.Load<Texture2D>("Sprites/lifebarspritesheet");
            _surgeCrystalTexture = _content.Load<Texture2D>("Sprites/surgecrystal");

            _helpTexture = _content.Load<Texture2D>("Sprites/newhelp");
            _saveTexture = _content.Load<Texture2D>("Sprites/save");
            _settingsTexture = _content.Load<Texture2D>("Sprites/settings");
            _badgesTexture = _content.Load<Texture2D>("Sprites/badges");

            _font = _content.Load<SpriteFont>("Fonts/File");

            // calculating icon positions
            var border = _resolution.X * 0.05f;

            var textureWidth = _saveTexture.Width * _scale;
            var textureHeight = _saveTexture.Height * _scale;

            var bottomHeight = _resolution.Y - border - textureHeight;
            _bottomHeight = bottomHeight.ToString(CultureInfo.InvariantCulture);

            _lifeBar = new LifeBarIcon(_lifeBarTexture, _scale, border, border, _player);
            _surgeCrystal = new UiIcon(_surgeCrystalTexture, _scale, _resolution.X - border - textureWidth, border);
            
            // the space between help and badges icons
            var remainingSpace = _resolution.X - 2 * border - 4 * textureWidth;
            // we want to put two icons in this space, hence we divide the space by three
            var spacing = remainingSpace / 3 + textureWidth;

            var helpPos = border;
            var savePos = helpPos + spacing;
            var settingsPos = savePos + spacing;
            var badgesPos = settingsPos + spacing;
            
            _help = new UiIcon(_helpTexture, _scale, helpPos, bottomHeight);
            _save = new UiIcon(_saveTexture, _scale, savePos, bottomHeight);
            _settings = new UiIcon(_settingsTexture, _scale, settingsPos, bottomHeight);
            _badges = new UiIcon(_badgesTexture, _scale, badgesPos, bottomHeight);    
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;

            // TODO: we probably need to extract level string drawing from here for future purposes
            var levelString = _bottomHeight;
            var (levelStringDimensionX, levelStringDimensionY) = _font.MeasureString(levelString);
            var levelStringPosX = (_resolution.X - levelStringDimensionX) / 2;
            const int levelStringPosY = 50;

            _spriteBatch.Begin();
            _spriteBatch.DrawString(_font, levelString, new Vector2(levelStringPosX, levelStringPosY), Color.White);
            _spriteBatch.End();

            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend,
                               SamplerState.PointClamp, null, null, null,
                               _transform);

            _lifeBar.Draw(_spriteBatch);
            _surgeCrystal.Draw(_spriteBatch);

            _save.Draw(_spriteBatch);
            _settings.Draw(_spriteBatch);
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
