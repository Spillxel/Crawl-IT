using System;

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
        private readonly float _fontScale;
        private Vector2 _levelStringPos;
        private const string LevelString = "SEMESTER 1";

        private readonly Matrix _transform;
        private readonly float _scale;
        private readonly Point _resolution;

        private readonly ContentManager _content;

        private readonly Player _player;

        public ExplorationUi(Matrix transform, float scale, Point resolution, ContentManager content, Player player)
        {
            _transform = transform;
            _scale = scale;
            _fontScale = _scale * 0.3f;
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

            // icon positions
            var border = _resolution.X * 0.05f;

            var textureWidth = _saveTexture.Width * _scale;
            var textureHeight = _saveTexture.Height * _scale;

            var bottomHeight = _resolution.Y - border - textureHeight;

            _lifeBar = new LifeBarIcon(_lifeBarTexture, _scale, border, border, _player);
            _surgeCrystal = new UiIcon(_surgeCrystalTexture, _scale, _resolution.X - border - textureWidth, border);
            
            // free space between leftmost and rightmost icons, minus textureWidth of the other two
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

            
            var (x, y) = _font.MeasureString(LevelString) * _fontScale;
            _levelStringPos = new Vector2((_resolution.X - x) * 0.5f, border + (textureHeight - y) * 0.5f);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(transformMatrix: _transform, samplerState: SamplerState.PointClamp);
            spriteBatch.DrawString(_font, LevelString, _levelStringPos, Color.White,
                                    0, Vector2.Zero, _fontScale, SpriteEffects.None, 0);

            _lifeBar.Draw(spriteBatch);
            _surgeCrystal.Draw(spriteBatch);

            _save.Draw(spriteBatch);
            _settings.Draw(spriteBatch);
            _help.Draw(spriteBatch);
            _badges.Draw(spriteBatch);

            spriteBatch.End();
        }

        public void Update(GameTime gameTime)
        {
            _lifeBar.Update(gameTime);
        }
    }
}
