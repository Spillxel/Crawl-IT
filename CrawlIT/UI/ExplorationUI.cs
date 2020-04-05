using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CrawlIT.Shared
{
    public class ExplorationUi
    {
        private UiIcon _lifeBar;
        private UiIcon _level;
        private UiIcon _surgeCrystal;

        private UiIcon _save;
        private UiIcon _settings;
        private UiIcon _help;
        private UiIcon _badges;

        private Texture2D _lifeBarTexture;
        private Texture2D _levelTexture;
        private Texture2D _surgeCrystalTexture;

        private Texture2D _helpTexture;
        private Texture2D _saveTexture;
        private Texture2D _settingsTexture;
        private Texture2D _badgesTexture;

        private SpriteFont _font;
        private readonly float _fontScale;

        private readonly Matrix _transform;
        private readonly float _scale;
        private readonly Point _resolution;

        private readonly ContentManager _content;

        private readonly Player _player;

        public ExplorationUi(Matrix transform, float scale, Point resolution, ContentManager content,
                             Player player)
        {
            _transform = transform;
            _scale = scale;
            _fontScale = _scale * 0.3f;
            _resolution = resolution;
            _content = content;
            _player = player;
        }

        public void Load()
        {
            _lifeBarTexture = _content.Load<Texture2D>("Sprites/lifebarspritesheet");
            _levelTexture = _content.Load<Texture2D>("Sprites/semester1");
            _surgeCrystalTexture = _content.Load<Texture2D>("Sprites/crystalspritesheet");

            _helpTexture = _content.Load<Texture2D>("Sprites/help");
            _saveTexture = _content.Load<Texture2D>("Sprites/save");
            _settingsTexture = _content.Load<Texture2D>("Sprites/settings");
            _badgesTexture = _content.Load<Texture2D>("Sprites/badges");

            _font = _content.Load<SpriteFont>("Fonts/Pixel");

            // icon positions
            var border = _resolution.X * 0.05f;

            var textureWidth = _saveTexture.Width * _scale;
            var textureHeight = _saveTexture.Height * _scale;

            var levelTextureWidth = _levelTexture.Width * _scale;

            var bottomHeight = _resolution.Y - border - textureHeight;

            _lifeBar = new LifeBarIcon(_lifeBarTexture, _scale, new Vector2(border),  _player);
            _level = new UiIcon(
                _levelTexture, _scale,
                new Vector2((_resolution.X - levelTextureWidth) * 0.5f, border));
            _surgeCrystal = new CrystalIcon(
                _surgeCrystalTexture, _scale, 
                new Vector2(_resolution.X - border - textureWidth, border),
                _player);
            
            // free space between leftmost and rightmost icons, minus textureWidth of the other two
            var remainingSpace = _resolution.X - 2 * border - 4 * textureWidth;
            // we want to put two icons in this space, hence we divide the space by three
            var spacing = remainingSpace / 3 + textureWidth;

            var helpPos = border;
            var savePos = helpPos + spacing;
            var settingsPos = savePos + spacing;
            var badgesPos = settingsPos + spacing;
            
            _help = new UiIcon(_helpTexture, _scale, new Vector2(helpPos, bottomHeight));
            _save = new UiIcon(_saveTexture, _scale, new Vector2(savePos, bottomHeight));
            _settings = new UiIcon(_settingsTexture, _scale, new Vector2(settingsPos, bottomHeight));
            _badges = new UiIcon(_badgesTexture, _scale, new Vector2(badgesPos, bottomHeight));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(transformMatrix: _transform, samplerState: SamplerState.PointClamp);

            _lifeBar.Draw(spriteBatch);
            _level.Draw(spriteBatch);
            _surgeCrystal.Draw(spriteBatch);

            _help.Draw(spriteBatch);
            _save.Draw(spriteBatch);
            _settings.Draw(spriteBatch);
            _badges.Draw(spriteBatch);

            spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            _lifeBar.Update(gameTime);
            _surgeCrystal.Update(gameTime);
        }
    }
}
