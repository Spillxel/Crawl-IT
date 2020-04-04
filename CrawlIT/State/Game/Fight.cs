using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using Newtonsoft.Json;

namespace CrawlIT.Shared
{
    public class Fight : GameState
    {
        private Texture2D _question;
        private Texture2D _screen;
        private Texture2D _crystal;
        private Texture2D _enemy;
        private Texture2D _blackScreen;
        private Texture2D _popUp;

        private Vector2 _questionPosition;
        private Vector2 _crystalPosition;
        private Vector2 _enemyPosition;
        private Vector2 _crystalScale;
        private Vector2 _popUpPosition;
        private Vector2 _answer1Position;
        private Vector2 _answer2Position;
        private Vector2 _answer3Position;
        private Vector2 _answer4Position;

        private Rectangle _questionRec;
        public Rectangle AnswerRectangle { get; private set; }
        public Rectangle Answer1Rectangle { get; private set; }
        public Rectangle Answer2Rectangle { get; private set; }
        public Rectangle Answer3Rectangle { get; private set; }
        public Rectangle Answer4Rectangle { get; private set; }
        public Rectangle CrystalRectangle { get; private set; }
        private Rectangle _enemyRec;
        private readonly List<Rectangle> _answerRec = new List<Rectangle>();
        private Rectangle _popUpRec;

        private string _questionString;
        private string _firstAnswer;
        private string _secondAnswer;
        private string _thirdAnswer;
        private string _fourthAnswer;

        private SpriteFont _font;
        private bool _win;

        private int _correct;
        private int _wrong1;
        private int _wrong2;
        private int _wrong3;

        public readonly Animation NoAnswer;
        private readonly Animation _correctAnswer;
        private readonly Animation _wrongAnswer;

        public Animation QuestionCurrentAnimation;

        private readonly Matrix _transform;
        private readonly Point _resolution;

        private float _scale;
        private float _crystalRatio;
        public override GameStateType State { get; }

        private bool _life;

        public Enemy Enemy;

        public Fight(GraphicsDevice graphicsDevice, Point resolution, Matrix transform,
                     Player player)
            : base(graphicsDevice)
        {
            _resolution = resolution;
            _transform = transform;
            Player = player;

            const int questionFrameWidth = 600;
            const int questionFrameHeight = 150;

            _correctAnswer = new Animation();
            _correctAnswer.AddFrame(new Rectangle(0, questionFrameHeight,
                                                  questionFrameWidth, questionFrameHeight),
                                    TimeSpan.FromSeconds(1));
            NoAnswer = new Animation();
            NoAnswer.AddFrame(new Rectangle(0, 0,
                                            questionFrameWidth, questionFrameHeight),
                              TimeSpan.FromSeconds(1));

            _wrongAnswer = new Animation();
            _wrongAnswer.AddFrame(new Rectangle(0, questionFrameHeight * 2,
                                                questionFrameWidth, questionFrameHeight),
                                  TimeSpan.FromSeconds(1));

            QuestionCurrentAnimation = NoAnswer;
            State = GameStateType.Fighting;
        }


        public override void Initialize()
        {
            _scale = _resolution.X / 1200f;
            _crystalRatio = _resolution.X / 200f;
            _life = true;
        }

        public override void LoadContent(ContentManager content)
        {
            var questionDict = new Dictionary<string, Question>();
            var filePath = Path.Combine(content.RootDirectory, "questions.json");

            string jsonString;
            using (var stream = TitleContainer.OpenStream(filePath))
            using (var reader = new StreamReader(stream))
                jsonString = reader.ReadToEnd();

            var questionList = JsonConvert.DeserializeObject<QuestionList>(jsonString);

            foreach (var q in questionList.Questions)
                questionDict.Add(q.QuestionSubject, q);

            var questionSample = questionDict["Maths"];
            var number = UniqueRandom();

            switch (number[0])
            {
                case 0:
                    questionSample = questionDict["Maths"];
                    break;
                case 1:
                    questionSample = questionDict["Algorithms"];
                    break;
                case 2:
                    questionSample = questionDict["Database"];
                    break;
                case 3:
                    questionSample = questionDict["Programming"];
                    break;
            }


            _questionString = questionSample.QuestionText;
            _firstAnswer = questionSample.Answer1;
            _secondAnswer = questionSample.Answer2;
            _thirdAnswer = questionSample.Answer3;
            _fourthAnswer = questionSample.Answer4;

            var numbers = UniqueRandom();
            _correct = numbers[0];
            _wrong1 = numbers[1];
            _wrong2 = numbers[2];
            _wrong3 = numbers[3];

            _font = content.Load<SpriteFont>("Fonts/Pixel");
            _crystal = content.Load<Texture2D>("Sprites/surgecrystal");
            _screen = content.Load<Texture2D>("Sprites/newscreentexture");
            _blackScreen = content.Load<Texture2D>("Sprites/blackscreentexture");
            _enemy = Enemy.CloseUpTexture;
            _question = content.Load<Texture2D>("Sprites/questiontexturesheet");
            _popUp = new Texture2D(GraphicsDevice, _resolution.X, _resolution.Y / 4);

            //Initialization of the vectors responsible of the initial position of the question and answers
            _questionPosition = new Vector2(0, _resolution.Y / 11 * 5 - 6);
            _answer1Position = new Vector2(0, _resolution.Y / 10 * 6);
            _answer2Position = new Vector2(_resolution.X / 2 + 3, _resolution.Y / 10 * 6);
            _answer3Position = new Vector2(0, _resolution.Y / 10 * 8 + 3);
            _answer4Position = new Vector2(_resolution.X / 2 + 3, _resolution.Y / 10 * 8 + 3);
            _crystalPosition = new Vector2(_resolution.X / 2f - _crystal.Width * _crystalRatio / 2,
                                           _resolution.Y / 10 * 8 - _crystal.Height * _crystalRatio / 2);
            _enemyPosition = new Vector2(0, 0);
            _crystalScale = new Vector2(_crystalRatio, _crystalRatio);
            _popUpPosition = new Vector2(0, _resolution.Y / 2 - _popUp.Height / 2);

            //Initialization of the size of the question and answers
            var questionPoint = new Point(_resolution.X, _resolution.Y / 20 * 3);
            var answerPoint = new Point(_resolution.X / 2 - 3,
                                        _resolution.Y / 10 * 2 - 3);
            var enemyPoint = new Point(_resolution.X, _resolution.Y / 2);
            var popUpPoint = new Point(_popUp.Width, _popUp.Height);

            var popUpData = new Color[_popUp.Width * _popUp.Height];
            popUpData = popUpData.Select(i => Color.White).ToArray();
            _popUp.SetData(popUpData);


            //Initialization of the rectangles using the initial position and the size of the question and answers
            _questionRec = new Rectangle(_questionPosition.ToPoint(), questionPoint);
            AnswerRectangle = new Rectangle(_answer1Position.ToPoint(),
                                            new Point(_resolution.X - 3, _resolution.Y / 2 - questionPoint.Y - 3));
            Answer1Rectangle = new Rectangle(_answer1Position.ToPoint(), answerPoint);
            Answer2Rectangle = new Rectangle(_answer2Position.ToPoint(), answerPoint);
            Answer3Rectangle = new Rectangle(_answer3Position.ToPoint(), answerPoint);
            Answer4Rectangle = new Rectangle(_answer4Position.ToPoint(), answerPoint);
            _enemyRec = new Rectangle(_enemyPosition.ToPoint(), enemyPoint);
            _popUpRec = new Rectangle(_popUpPosition.ToPoint(), popUpPoint);

            _answerRec.Add(Answer1Rectangle);
            _answerRec.Add(Answer2Rectangle);
            _answerRec.Add(Answer3Rectangle);
            _answerRec.Add(Answer4Rectangle);

            CrystalRectangle = new Rectangle(_crystalPosition.ToPoint(), _crystal.Bounds.Size * _crystalScale.ToPoint());
        }

        public override void Dispose()
        {
        }

        public override void Update(GameTime gameTime)
        {
            QuestionCurrentAnimation.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            GraphicsDevice.Clear(Color.Black);

            // Render enemy ScreenShot
            spriteBatch.Begin(transformMatrix: _transform, samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(_enemy, _enemyRec, Color.White);
            // Render question
            var sourceRectangle = QuestionCurrentAnimation.CurrentRectangle;
            spriteBatch.Draw(_question, _questionRec, sourceRectangle, Color.White);
            // Render answers
            spriteBatch.Draw(_screen, Answer1Rectangle, Color.White);
            spriteBatch.Draw(_screen, Answer2Rectangle, Color.White);
            spriteBatch.Draw(_screen, Answer3Rectangle, Color.White);
            spriteBatch.Draw(_screen, Answer4Rectangle, Color.White);

            // Draw the text on the rectangles of the answers
            DrawString(spriteBatch, _font, _questionString, _questionRec, Color.Cyan);
            DrawString(spriteBatch, _font, _firstAnswer, _answerRec[_correct], Color.Cyan);
            DrawString(spriteBatch, _font, _secondAnswer, _answerRec[_wrong1], Color.Cyan);
            DrawString(spriteBatch, _font, _thirdAnswer, _answerRec[_wrong2], Color.Cyan);

            if (_fourthAnswer == "")
            {
                spriteBatch.Draw(_blackScreen, _answerRec[_wrong3], Color.White);
                spriteBatch.Draw(_crystal, _crystalPosition, null, Color.White, 0,
                                 Vector2.Zero, _crystalScale, SpriteEffects.None, 0);
            }
            else
            {
                DrawString(spriteBatch, _font, _fourthAnswer, _answerRec[_wrong3], Color.Cyan);
            }

            // Render crystal sprite
            spriteBatch.Draw(_crystal, _crystalPosition, null, Color.White, 0,
                             Vector2.Zero, _crystalScale, SpriteEffects.None, 0);
            spriteBatch.End();
        }

        public void CheckAnswer(Rectangle touch)
        {
            foreach (var rect in _answerRec)
            {
                if (!touch.Intersects(rect)) continue;
                if (_answerRec.IndexOf(rect) == _correct && _life)
                {
                    Enemy.BeatenBy(Player);                      
                    _win = true;
                    _life = false;
                    break;
                }
                else if (_life)
                {
                    Enemy.Beat(Player);
                    _life = false;
                    _win = false;
                    break;
                }
            }
        }

        public void ChangeColour(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(transformMatrix: _transform, samplerState: SamplerState.PointClamp);
            if (_fourthAnswer == "")
            {
                spriteBatch.Draw(_blackScreen, _answerRec[_wrong3], Color.White);
                spriteBatch.Draw(_crystal, _crystalPosition, null, Color.White, 0,
                                 Vector2.Zero, _crystalScale, SpriteEffects.None, 0);
            }

            QuestionCurrentAnimation = _win ? _correctAnswer : _wrongAnswer;

            var sourceRectangle = QuestionCurrentAnimation.CurrentRectangle;
            spriteBatch.Draw(_question, _questionRec, sourceRectangle, Color.White);
            DrawString(spriteBatch, _font, _questionString, _questionRec, Color.Cyan);
            DrawString(spriteBatch, _font, _firstAnswer, _answerRec[_correct], Color.LimeGreen);
            DrawString(spriteBatch, _font, _secondAnswer, _answerRec[_wrong1], Color.Red);
            DrawString(spriteBatch, _font, _thirdAnswer, _answerRec[_wrong2], Color.Red);
            DrawString(spriteBatch, _font, _fourthAnswer, _answerRec[_wrong3], Color.Red);
            spriteBatch.End();
        }

        public void Help(SpriteBatch spriteBatch)
        {
            _fourthAnswer = "";

            spriteBatch.Begin(transformMatrix: _transform, samplerState: SamplerState.PointClamp);
            spriteBatch.Draw(_blackScreen, _answerRec[_wrong3], Color.White);
            spriteBatch.Draw(_crystal, _crystalPosition, null, Color.White, 0,
                             Vector2.Zero, _crystalScale, SpriteEffects.None, 0);

            DrawString(spriteBatch, _font, _firstAnswer, _answerRec[_correct], Color.Cyan);
            DrawString(spriteBatch, _font, _secondAnswer, _answerRec[_wrong1], Color.Cyan);
            DrawString(spriteBatch, _font, _thirdAnswer, _answerRec[_wrong2], Color.Cyan);
            DrawString(spriteBatch, _font, _fourthAnswer, _answerRec[_wrong3], Color.Cyan);
            spriteBatch.End();
        }

        public void PopUp(SpriteBatch spriteBatch)
        {
            var test = _win ? "YOU WON!!!" : "YOU LOST";

            spriteBatch.Begin(transformMatrix: _transform, samplerState: SamplerState.PointClamp);
            //spriteBatch.Draw(_popUp, _popUpRec, Color.White);
            spriteBatch.Draw(_screen, _popUpRec, Color.White);
            DrawString(spriteBatch, _font, test, _popUpRec, Color.Cyan);
            spriteBatch.End();
        }

        /// <summary>
        /// Draws the given string inside the boundaries
        /// </summary>
        /// If the string is not a perfect match inside of the boundaries (which it would rarely be), then
        /// the string will be absolutely-centered inside of the boundaries.
        public void DrawString(SpriteBatch spriteBatch, SpriteFont font, string strToDraw, Rectangle boundaries, Color color)
        {
            if(boundaries.Width==_resolution.X)
            {
                boundaries.Width = (int)((int)_resolution.X * 0.8);
                boundaries.X += (int)((int)_resolution.X * 0.1);
            }

            // Code for parsing the text depending on the width of the screen
            var line = string.Empty;
            var returnString = string.Empty;
            var wordArray = strToDraw.Split(' ');
            foreach (var word in wordArray)
            {
                if (_font.MeasureString(line + word).Length() > boundaries.Width / _scale)
                {
                    returnString = returnString + line + '\n';
                    line = string.Empty;
                }

                line = line + word + ' ';
            }
            returnString += line;

            var (x, y) = font.MeasureString(strToDraw);

            var lines = returnString.Split('\n');

            // Figure out the location to absolutely-center it in the boundaries rectangle.
            var strWidth = (int)Math.Round(x * _scale);
            var strHeight = (int)Math.Round(y * _scale);
            var position = Vector2.Zero;
            position.X = (boundaries.Width - strWidth / lines.Length) / 2 + boundaries.X;
            position.Y = (boundaries.Height - strHeight * lines.Length) / 2 + boundaries.Y;

            // A bunch of settings where we just want to use reasonable defaults.
            const float rotation = 0.0f;
            var spriteOrigin = new Vector2(0, 0);
            const float spriteLayer = 0.0f; // all the way in the front
            const SpriteEffects spriteEffects = new SpriteEffects();

            foreach (var l in lines)
            {
                var lineSize = font.MeasureString(l);
                var lineWidth = (int)Math.Round(lineSize.X * _scale);
                position.X = (boundaries.Width - lineWidth) / 2 + boundaries.X + 5;
                spriteBatch.DrawString(font, l, position, color, rotation, spriteOrigin, _scale,
                                       spriteEffects, spriteLayer);
                position.Y += strHeight;
            }
        }

        /// <summary>
        /// Returns all numbers, between min and max inclusive, once in a random sequence.
        /// </summary>
        private List<int> UniqueRandom()
        {
            var candidates = new List<int>();
            var result = new List<int>();
            for (var i = 0; i <= 3; i++)
            {
                candidates.Add(i);
            }
            var rnd = new Random();
            while (candidates.Count > 0)
            {
                var index = rnd.Next(candidates.Count);
                result.Add(candidates[index]);
                candidates.RemoveAt(index);
            }
            return result;
        }
    }
}
