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
        private UiIcon _inventory;
        private UiIcon _help;
        private UiIcon _badges;

        private Texture2D _lifeBarTexture;
        private Texture2D _surgeCrystalTexture;
        private Texture2D _saveTexture;
        private Texture2D _settingsTexture;
        private Texture2D _helpTexture;
        private Texture2D _badgesTexture;

        private readonly Matrix _scale;
        private readonly Point _resolution;
        private readonly ContentManager _content;
        private Camera _staticCamera;
        private SpriteFont _font;
        private SpriteBatch _spriteBatch;
        private readonly Player _player;

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
            _settingsTexture = _content.Load<Texture2D>("Sprites/settings");
            _helpTexture = _content.Load<Texture2D>("Sprites/newhelp");
            _badgesTexture = _content.Load<Texture2D>("Sprites/badges");
            _surgeCrystalTexture = _content.Load<Texture2D>("Sprites/surgecrystal");

            _font = _content.Load<SpriteFont>("Fonts/File");

            int _border = 100;
            float _height = _graphics.PreferredBackBufferHeight - 50 - (32 * _zoom);
            float _screenWithoutBorder = _graphics.PreferredBackBufferWidth - 2 * _border;
            float _iconWidth = 32 * _zoom;

            _lifeBar = new LifeBarIcon(_lifeBarTexture, _zoom, 32 * _zoom, 50, _player);
            _surgeCrystal = new UIIcon(_surgeCrystalTexture, _zoom, _graphics.PreferredBackBufferWidth - (2 * 32 * _zoom), 50);
            //_save = new UIIcon(_saveTexture, _zoom, (_graphics.PreferredBackBufferWidth / 2) + 50 + (2 * 32), _graphics.PreferredBackBufferHeight - 50 - (32 * _zoom));
            //_settings = new UIIcon(_settingsTexture, _zoom, (_graphics.PreferredBackBufferWidth / 4) + 32 + 50, _graphics.PreferredBackBufferHeight - 50 - (32 * _zoom));
            //_help = new UIIcon(_helpTexture, _zoom, 50, _graphics.PreferredBackBufferHeight - 50 - (32 * _zoom));
            //_badges = new UIIcon(_badgesTexture, _zoom, _graphics.PreferredBackBufferWidth - (32 * _zoom + 50), _graphics.PreferredBackBufferHeight - 50 - (32 * _zoom));
            //_save = new UIIcon(_saveTexture, _zoom, (_graphics.PreferredBackBufferWidth - 32 * _zoom) * 0.6f, _graphics.PreferredBackBufferHeight - 50 - (32 * _zoom));
            //_settings = new UIIcon(_settingsTexture, _zoom, (_graphics.PreferredBackBufferWidth - 32 * _zoom) * 0.3f , _graphics.PreferredBackBufferHeight - 50 - (32 * _zoom));
            //_help = new UIIcon(_helpTexture, _zoom, 32 * _zoom, _graphics.PreferredBackBufferHeight - 50 - (32 * _zoom));
            //_badges = new UIIcon(_badgesTexture, _zoom, _graphics.PreferredBackBufferWidth - (2 * 32 * _zoom), _graphics.PreferredBackBufferHeight - 50 - (32 * _zoom));
            _save = new UIIcon(_saveTexture, _zoom,(_screenWithoutBorder / 3) * 2 + _border - _iconWidth / 2, _height);
            _settings = new UIIcon(_settingsTexture, _zoom, (_screenWithoutBorder / 3) + _border - _iconWidth / 2, _height);
            _help = new UIIcon(_helpTexture, _zoom, _border - _iconWidth / 2, _height);
            _badges = new UIIcon(_badgesTexture, _zoom, (_screenWithoutBorder / 3) * 3 + _border - _iconWidth / 2, _height);    
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
