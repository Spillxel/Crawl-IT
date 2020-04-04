using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CrawlIT.Shared
{
    public class Menu : GameState
    {
        private Texture2D _buttonsSprite;
        private int _buttonsSpriteOffset;
        private Point _buttonSize;
        private Vector2 _newGameButtonPosition;
        private Rectangle _newGameButton;
        public Rectangle NewGameRectangle;
        private Vector2 _continueButtonPosition;
        private Rectangle _continueButton;
        public Rectangle ContinueRectangle;
        private Vector2 _creditsButtonPosition;
        private Rectangle _creditsButton;
        public Rectangle CreditsRectangle;
        private Vector2 _blankButtonPosition;
        private Rectangle _blankButton;
        public Rectangle BlankRectangle;
        private Texture2D _logo;
        private Vector2 _logoPosition;

        private readonly Matrix _transform;
        private readonly float _scale;
        private readonly Point _resolution;

        public override GameStateType State { get; }

        public Menu(GraphicsDevice graphicsDevice, Point resolution, Matrix transform, float scale)
            : base(graphicsDevice)
        {
            _transform = transform;
            _resolution = resolution;
            _scale = scale;
            State = GameStateType.Menu;
        }

        public override void Initialize()
        {
        }

        public override void LoadContent(ContentManager content)
        {
            _buttonsSprite = content.Load<Texture2D>("Sprites/mainbuttons");
            _buttonsSpriteOffset = _buttonsSprite.Height / 4;
            _buttonSize = new Point((int) (_buttonsSprite.Width * _scale), (int) (_buttonsSpriteOffset * _scale));
            
            _newGameButton = new Rectangle(0, 0, _buttonsSprite.Width, _buttonsSpriteOffset);
            _newGameButtonPosition = new Vector2((_resolution.X - _buttonsSprite.Width * _scale) * 0.5f,
                _resolution.Y * 0.6f);
            NewGameRectangle = new Rectangle(_newGameButtonPosition.ToPoint(), _buttonSize);

            // TODO: Blank #2 for now kek
            _creditsButton = new Rectangle(0, _buttonsSpriteOffset * 3, _buttonsSprite.Width, _buttonsSpriteOffset);
            _creditsButtonPosition = new Vector2(_newGameButtonPosition.X, _resolution.Y * 0.7f);

            _blankButton = new Rectangle(0, _buttonsSpriteOffset * 3, _buttonsSprite.Width, _buttonsSpriteOffset);
            _blankButtonPosition = new Vector2(_newGameButtonPosition.X, _resolution.Y * 0.8f);
            BlankRectangle = new Rectangle(_blankButtonPosition.ToPoint(), _buttonSize);

            _logo = content.Load<Texture2D>("Sprites/mainlogo");
            _logoPosition = new Vector2((_resolution.X - _logo.Width * _scale) * 0.5f,
                _resolution.Y * 0.3f - _logo.Height * _scale * 0.5f);
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
            spriteBatch.Begin(transformMatrix: _transform, samplerState: SamplerState.PointClamp);
            Helper.DrawWrapper(spriteBatch, _buttonsSprite, _newGameButtonPosition, _newGameButton, _scale);
            Helper.DrawWrapper(spriteBatch, _buttonsSprite, _creditsButtonPosition, _creditsButton, _scale);
            Helper.DrawWrapper(spriteBatch, _buttonsSprite, _blankButtonPosition, _blankButton, _scale);
            Helper.DrawWrapper(spriteBatch, _logo, _logoPosition, null, _scale);
            spriteBatch.End();
        }
    }
}