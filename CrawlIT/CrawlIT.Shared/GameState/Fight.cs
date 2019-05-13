using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using CrawlIT.Shared.Combat;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace CrawlIT.Shared.GameState
{
    public class Fight : GameState
    {
        private Enum _state;

        private Texture2D _question;
        private Texture2D _screen;
        private Texture2D _crystal;
        private Texture2D _enemy;
        private Texture2D _blackscreen;

        private Vector2 _questionPosition;
        private Vector2 _answer1Position;
        private Vector2 _answer2Position;
        private Vector2 _answer3Position;
        private Vector2 _answer4Position;
        private Vector2 _crystalPosition;
        private Vector2 _enemyPosition;
        private Vector2 _crystalScale;

        private Rectangle _questionRec;
        private Rectangle _answer1Rec;
        private Rectangle _answer2Rec;
        private Rectangle _answer3Rec;
        private Rectangle _answer4Rec;
        private Rectangle _enemyRec;
        private List<Rectangle> _answerRec = new List<Rectangle>();

        private String _questionString;
        private String _firstAnswer;
        private String _secondAnswer;
        private String _thirdAnswer;
        private String _fourthAnswer;

        private SpriteFont _font;

        private Random rnd = new Random();

        private float _scale;
        private float _crystalRatio;

        public Fight(GraphicsDevice graphicsDevice)
        : base(graphicsDevice)
        {
        }

        public override void Initialize()
        {
            _question = new Texture2D(GraphicsDevice, GraphicsDevice.Viewport.Width,
                                      GraphicsDevice.Viewport.Height / 10);

            Color[] data1 = new Color[_question.Width * _question.Height];

            for (int i = 0; i < data1.Length; ++i)
            {
                data1[i] = Color.LightGray;
            }
            _question.SetData(data1);

            _scale = GraphicsDevice.Viewport.Width / 1200f;

            _crystalRatio = GraphicsDevice.Viewport.Width / 200;
        }

        public override void LoadContent(ContentManager content)
        {
            #region
            var questionDict = new Dictionary<string, Question>();
            var filePath = Path.Combine(content.RootDirectory, "questions.json");

            string jsonString;
            using (var stream = TitleContainer.OpenStream(filePath))
            using (var reader = new StreamReader(stream))
                jsonString = reader.ReadToEnd();

            var questionList = JsonConvert.DeserializeObject<QuestionList>(jsonString);

            foreach (var q in questionList.Questions)
                questionDict.Add(q.QuestionSubject, q);
            #endregion

            Question questionSample = questionDict["Maths"];

            _questionString = questionSample.QuestionText;
            _firstAnswer = questionSample.Answer1;
            _secondAnswer = questionSample.Answer2;
            _thirdAnswer = questionSample.Answer3;
            _fourthAnswer = questionSample.Answer4;

            _font = content.Load<SpriteFont>("Fonts/File");
            _crystal = content.Load<Texture2D>("Sprites/surgecrystal");
            _enemy = content.Load<Texture2D>("Sprites/tutorfight");
            _screen = content.Load<Texture2D>("Sprites/newscreentexture");
            _blackscreen = content.Load<Texture2D>("Sprites/blackscreentexture");
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
            //Initialization of the vectors responsible of the initial position of the question and answers
            _questionPosition = new Vector2(0, (GraphicsDevice.Viewport.Height / 10 * 5) - 6);
            _answer1Position = new Vector2(0, GraphicsDevice.Viewport.Height / 10 * 6);
            _answer2Position = new Vector2((GraphicsDevice.Viewport.Width / 2) + 3, GraphicsDevice.Viewport.Height / 10 * 6);
            _answer3Position = new Vector2(0, (GraphicsDevice.Viewport.Height / 10 * 8) + 3);
            _answer4Position = new Vector2((GraphicsDevice.Viewport.Width / 2) + 3, (GraphicsDevice.Viewport.Height / 10 * 8) + 3);
            _crystalPosition = new Vector2(((GraphicsDevice.Viewport.Width / 2) - (_crystal.Width * _crystalRatio / 2)),
                                           ((GraphicsDevice.Viewport.Height / 10 * 8) - (_crystal.Height * _crystalRatio / 2)));
            _enemyPosition = new Vector2(0, 0);
            _crystalScale = new Vector2(_crystalRatio, _crystalRatio);

            //Initialization of the size of the question and answers
            Point _questionPoint = new Point(_question.Width, _question.Height);
            Point _answerPoint = new Point(GraphicsDevice.Viewport.Width / 2 - 3,
                                           GraphicsDevice.Viewport.Height / 10 * 2 - 3);
            Point _enemyPoint = new Point(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height / 2);

            //Initialization of the rectangles using the initial position and the size of the question and answers
            _questionRec = new Rectangle(_questionPosition.ToPoint(), _questionPoint);
            _answer1Rec = new Rectangle(_answer1Position.ToPoint(), _answerPoint);
            _answer2Rec = new Rectangle(_answer2Position.ToPoint(), _answerPoint);
            _answer3Rec = new Rectangle(_answer3Position.ToPoint(), _answerPoint);
            _answer4Rec = new Rectangle(_answer4Position.ToPoint(), _answerPoint);
            _enemyRec = new Rectangle(_enemyPosition.ToPoint(), _enemyPoint);

            _answerRec.Add(_answer1Rec);

            GraphicsDevice.Clear(Color.Black);

            // Render enemy screenshot
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp);
            spriteBatch.Draw(_enemy, _enemyRec, Color.White);
            // Render question and answers
            spriteBatch.Draw(_question, _questionPosition, Color.White);
            spriteBatch.Draw(_screen, _answer1Rec, Color.White);
            spriteBatch.Draw(_screen, _answer2Rec, Color.White);
            spriteBatch.Draw(_screen, _answer3Rec, Color.White);
            spriteBatch.Draw(_screen, _answer4Rec, Color.White);
            spriteBatch.End();

            // Draw the text on the rectangles of the answers
            spriteBatch.Begin();
            DrawString(spriteBatch, _font, _questionString, _questionRec, Color.Black);
            DrawString(spriteBatch, _font, _firstAnswer, _answer1Rec, Color.Cyan);
            DrawString(spriteBatch, _font, _secondAnswer, _answer2Rec, Color.Cyan);
            DrawString(spriteBatch, _font, _thirdAnswer, _answer3Rec, Color.Cyan);
            DrawString(spriteBatch, _font, _fourthAnswer, _answer4Rec, Color.Cyan);
            spriteBatch.End();

            // Render crystal sprite
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp);
            spriteBatch.Draw(texture: _crystal, position: _crystalPosition, color: Color.White, scale: _crystalScale);
            spriteBatch.End();
        }

        public override Point GetPosition(Texture2D button)
        {
            if (button.Equals(_screen))
            {
                return new Point((int)_answer3Position.X,
                                 (int)_answer3Position.Y);
            }
            else if (button.Equals(_crystal))
            {
                return new Point((int)_crystalPosition.X,
                                 (int)_crystalPosition.Y);
            }
            else
            {
                //return new Point(0, 0);
                return new Point((int)_answer3Position.X,
                                 (int)_answer3Position.Y);
            }
        }

        public override Point GetRectangle(Rectangle touch)
        {
            return new Point(0, 0);
        }

        public override void ChangeColour(SpriteBatch spriteBatch)
        {
            if (_fourthAnswer == "")
            {
                spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp);
                spriteBatch.Draw(_blackscreen, _answer4Rec, Color.White);
                spriteBatch.Draw(texture: _crystal, position: _crystalPosition, color: Color.White, scale: _crystalScale);
                spriteBatch.End();
            }

            spriteBatch.Begin();
            DrawString(spriteBatch, _font, _firstAnswer, _answer1Rec, Color.Red);
            DrawString(spriteBatch, _font, _secondAnswer, _answer2Rec, Color.Red);
            DrawString(spriteBatch, _font, _thirdAnswer, _answer3Rec, Color.LimeGreen);
            DrawString(spriteBatch, _font, _fourthAnswer, _answer4Rec, Color.Red);
            spriteBatch.End();
        }

        public override void Help(SpriteBatch spriteBatch)
        {
            _fourthAnswer = "";

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp);
            spriteBatch.Draw(_blackscreen, _answer4Rec, Color.White);
            spriteBatch.Draw(texture: _crystal, position: _crystalPosition, color: Color.White, scale: _crystalScale);
            spriteBatch.End();

            spriteBatch.Begin();
            DrawString(spriteBatch, _font, _firstAnswer, _answer1Rec, Color.Cyan);
            DrawString(spriteBatch, _font, _secondAnswer, _answer2Rec, Color.Cyan);
            DrawString(spriteBatch, _font, _thirdAnswer, _answer3Rec, Color.Cyan);
            DrawString(spriteBatch, _font, _fourthAnswer, _answer4Rec, Color.Cyan);
            spriteBatch.End();
        }


        // Draws the given string inside the boundaries Rectangle without going outside of it.
        // 
        // If the string is not a perfect match inside of the boundaries (which it would rarely be), then
        // the string will be absolutely-centered inside of the boundaries.
        public void DrawString(SpriteBatch spriteBatch, SpriteFont font, string strToDraw, Rectangle boundaries, Color color)
        {
            // Code for parsing the text depending on the width of the screen
            String line = String.Empty;
            String returnString = String.Empty;
            String[] wordArray = strToDraw.Split(' ');
            foreach (String word in wordArray)
            {
                if (_font.MeasureString(line + word).Length() > boundaries.Width / _scale)
                {
                    returnString = returnString + line + '\n';
                    line = String.Empty;
                }

                line = line + word + ' ';
            }
            returnString += line;

            Vector2 size = font.MeasureString(strToDraw);

            String[] lines = returnString.Split('\n');

            // Figure out the location to absolutely-center it in the boundaries rectangle.
            int strWidth = (int)Math.Round(size.X * _scale);
            int strHeight = (int)Math.Round(size.Y * _scale);
            Vector2 position = new Vector2();
            position.X = ((boundaries.Width - (strWidth / lines.Length)) / 2) + boundaries.X;
            position.Y = (((boundaries.Height - (strHeight * lines.Length)) / 2) + boundaries.Y);

            // A bunch of settings where we just want to use reasonable defaults.
            float rotation = 0.0f;
            Vector2 spriteOrigin = new Vector2(0, 0);
            float spriteLayer = 0.0f; // all the way in the front
            SpriteEffects spriteEffects = new SpriteEffects();

            for (int i = 0; i < lines.Length; i++)
            {
                Vector2 lineSize = font.MeasureString(lines[i]);
                int lineWidth = (int)Math.Round(lineSize.X * _scale);
                position.X = ((boundaries.Width - lineWidth) / 2) + boundaries.X + 5;
                spriteBatch.DrawString(font, lines[i], position, color, rotation, spriteOrigin, _scale, spriteEffects, spriteLayer);
                position.Y += strHeight;
            }

        }
    }
}
