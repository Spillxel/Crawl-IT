﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using CrawlIT.Shared.Combat;
using CrawlIT.Shared.Entity;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

namespace CrawlIT.Shared.GameState
{
    public class Fight : GameState
    {
        private Enum _state;

        private Texture2D _questionTexture;
        private Texture2D _screen;
        private Texture2D _crystal;
        private Texture2D _enemy;
        private Texture2D _blackscreen;
        private Texture2D _rectangle;

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
        private Rectangle _rectanglePos;
        private Rectangle[] _answerRec = new Rectangle[4];

        private String _questionString;
        private String _firstAnswer;
        private String _secondAnswer;
        private String _thirdAnswer;
        private String _fourthAnswer;

        private SpriteFont _font;
        private bool _win = false;

        private int _questionFrameWidth;
        private int _questionFrameHeight;
        private int correct;
        private int wrong1;
        private int wrong2;
        private int wrong3;

        private readonly Animation.Animation _noAnswer;
        private readonly Animation.Animation _correctAnswer;
        private readonly Animation.Animation _wrongAnswer;

        private Animation.Animation _questionCurrentAnimation;

        private float _scale;
        private float _crystalRatio;

        public Fight(GraphicsDevice graphicsDevice, Player player)
        : base(graphicsDevice)
        {
            Player = player;

            _questionFrameWidth = 600;
            _questionFrameHeight = 150;

            _noAnswer = new Animation.Animation();
            _noAnswer.AddFrame(new Rectangle(0, 0, _questionFrameWidth, _questionFrameHeight), TimeSpan.FromSeconds(1));

            _correctAnswer = new Animation.Animation();
            _correctAnswer.AddFrame(new Rectangle(0, _questionFrameHeight, _questionFrameWidth, _questionFrameHeight), TimeSpan.FromSeconds(1));

            _wrongAnswer = new Animation.Animation();
            _wrongAnswer.AddFrame(new Rectangle(0, _questionFrameHeight * 2, _questionFrameWidth, _questionFrameHeight), TimeSpan.FromSeconds(1));

            _questionCurrentAnimation = _noAnswer;
        }

        public override void Initialize()
        {
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

            List<int> numbers = UniqueRandom();
            correct = numbers[0];
            wrong1 = numbers[1];
            wrong2 = numbers[2];
            wrong3 = numbers[3];

            _font = content.Load<SpriteFont>("Fonts/File");
            _crystal = content.Load<Texture2D>("Sprites/surgecrystal");
            _enemy = content.Load<Texture2D>("Sprites/tutorfight");
            _screen = content.Load<Texture2D>("Sprites/newscreentexture");
            _blackscreen = content.Load<Texture2D>("Sprites/blackscreentexture");
            _questionTexture = content.Load<Texture2D>("Sprites/questiontexturesheet");
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
            _questionCurrentAnimation.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //Initialization of the vectors responsible of the initial position of the question and answers
            _questionPosition = new Vector2(0, (GraphicsDevice.Viewport.Height / 11 * 5) - 6);
            _answer1Position = new Vector2(0, GraphicsDevice.Viewport.Height / 10 * 6);
            _answer2Position = new Vector2((GraphicsDevice.Viewport.Width / 2) + 3, GraphicsDevice.Viewport.Height / 10 * 6);
            _answer3Position = new Vector2(0, (GraphicsDevice.Viewport.Height / 10 * 8) + 3);
            _answer4Position = new Vector2((GraphicsDevice.Viewport.Width / 2) + 3, (GraphicsDevice.Viewport.Height / 10 * 8) + 3);
            _crystalPosition = new Vector2(((GraphicsDevice.Viewport.Width / 2) - (_crystal.Width * _crystalRatio / 2)),
                                           ((GraphicsDevice.Viewport.Height / 10 * 8) - (_crystal.Height * _crystalRatio / 2)));
            _enemyPosition = new Vector2(0, 0);
            _crystalScale = new Vector2(_crystalRatio, _crystalRatio);

            //Initialization of the size of the question and answers
            Point _questionPoint = new Point(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height / 20 * 3);
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

            _answerRec[0] = _answer1Rec;
            _answerRec[1] = _answer2Rec;
            _answerRec[2] = _answer3Rec;
            _answerRec[3] = _answer4Rec;

            GraphicsDevice.Clear(Color.Black);

            // Render enemy screenshot
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp);
            spriteBatch.Draw(_enemy, _enemyRec, Color.White);
            // Render question
            var sourceRectangle = _questionCurrentAnimation.CurrentRectangle;
            spriteBatch.Draw(_questionTexture, _questionRec, sourceRectangle, Color.White);
            // Render answers
            spriteBatch.Draw(_screen, _answer1Rec, Color.White);
            spriteBatch.Draw(_screen, _answer2Rec, Color.White);
            spriteBatch.Draw(_screen, _answer3Rec, Color.White);
            spriteBatch.Draw(_screen, _answer4Rec, Color.White);
            spriteBatch.End();

            // Draw the text on the rectangles of the answers
            spriteBatch.Begin();
            DrawString(spriteBatch, _font, _questionString, _questionRec, Color.Cyan);
            DrawString(spriteBatch, _font, _firstAnswer, _answerRec[correct], Color.Cyan);
            DrawString(spriteBatch, _font, _secondAnswer, _answerRec[wrong1], Color.Cyan);
            DrawString(spriteBatch, _font, _thirdAnswer, _answerRec[wrong2], Color.Cyan);
            if (_fourthAnswer == "")
            {
                spriteBatch.End();
                spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp);
                spriteBatch.Draw(_blackscreen, _answerRec[wrong3], Color.White);
                spriteBatch.Draw(texture: _crystal, position: _crystalPosition, color: Color.White, scale: _crystalScale);
                spriteBatch.End();
            }
            else
            {
                DrawString(spriteBatch, _font, _fourthAnswer, _answerRec[wrong3], Color.Cyan);
                spriteBatch.End();
            }

            // Render crystal sprite
            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp);
            spriteBatch.Draw(texture: _crystal, position: _crystalPosition, color: Color.White, scale: _crystalScale);
            spriteBatch.End();
        }

        public override Point GetPosition(Texture2D button)
        {
            if (button.Equals(_screen))
            {
                return new Point((int)_answer1Position.X,
                                 (int)_answer1Position.Y);
            }
            else if (button.Equals(_crystal))
            {
                return new Point((int)_crystalPosition.X,
                                 (int)_crystalPosition.Y);
            }
            else
            {
                return new Point(0, 0);
            }
        }

        public override bool GetAnswer(Rectangle touch)
        {
            if (touch.Intersects(_answerRec[correct]))
            {
                Player.SetLifeCount(Player.lifeCount + 1);
                _win = true;
                return true;
            }
            else
            {
                Player.SetLifeCount(Player.lifeCount - 1);
                return false;
            }
        }

        public override void ChangeColour(SpriteBatch spriteBatch)
        {
            if (_fourthAnswer == "")
            {
                spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp);
                spriteBatch.Draw(_blackscreen, _answerRec[wrong3], Color.White);
                spriteBatch.Draw(texture: _crystal, position: _crystalPosition, color: Color.White, scale: _crystalScale);
                spriteBatch.End();
            }

            if (_win)
                _questionCurrentAnimation = _correctAnswer;
            else
                _questionCurrentAnimation = _wrongAnswer;

            spriteBatch.Begin();
            var sourceRectangle = _questionCurrentAnimation.CurrentRectangle;
            spriteBatch.Draw(_questionTexture, _questionRec, sourceRectangle, Color.White);
            DrawString(spriteBatch, _font, _questionString, _questionRec, Color.Cyan);
            DrawString(spriteBatch, _font, _firstAnswer, _answerRec[correct], Color.LimeGreen);
            DrawString(spriteBatch, _font, _secondAnswer, _answerRec[wrong1], Color.Red);
            DrawString(spriteBatch, _font, _thirdAnswer, _answerRec[wrong2], Color.Red);
            DrawString(spriteBatch, _font, _fourthAnswer, _answerRec[wrong3], Color.Red);
            spriteBatch.End();
        }

        public override void Help(SpriteBatch spriteBatch)
        {
            _fourthAnswer = "";

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp);
            spriteBatch.Draw(_blackscreen, _answerRec[wrong3], Color.White);
            spriteBatch.Draw(texture: _crystal, position: _crystalPosition, color: Color.White, scale: _crystalScale);
            spriteBatch.End();

            spriteBatch.Begin();
            DrawString(spriteBatch, _font, _firstAnswer, _answerRec[correct], Color.Cyan);
            DrawString(spriteBatch, _font, _secondAnswer, _answerRec[wrong1], Color.Cyan);
            DrawString(spriteBatch, _font, _thirdAnswer, _answerRec[wrong2], Color.Cyan);
            DrawString(spriteBatch, _font, _fourthAnswer, _answerRec[wrong3], Color.Cyan);
            spriteBatch.End();
        }

        public override void PopUp(SpriteBatch spriteBatch)
        {
            String test;

            if (_win)
                test = "YOU WON!!!";
            else
                test = "YOU LOST";

            _rectangle = new Texture2D(GraphicsDevice, GraphicsDevice.Viewport.Width,
                                      GraphicsDevice.Viewport.Height/4);

            _rectanglePos = new Rectangle(0, (GraphicsDevice.Viewport.Height / 2) - (_rectangle.Height / 2),
                                          _rectangle.Width,  _rectangle.Height);

            Color[] data = new Color[_rectangle.Width * _rectangle.Height];

            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = Color.White;
            }

            _rectangle.SetData(data);

            spriteBatch.Begin();
            spriteBatch.Draw(_rectangle, _rectanglePos, Color.White);
            DrawString(spriteBatch, _font, test, _rectanglePos, Color.Black);
            spriteBatch.End();

        }

        /// <summary>
        /// Draws the given string inside the boundaries
        /// </summary>
        /// If the string is not a perfect match inside of the boundaries (which it would rarely be), then
        /// the string will be absolutely-centered inside of the boundaries.
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

        /// <summary>
        /// Returns all numbers, between min and max inclusive, once in a random sequence.
        /// </summary>
        List<int> UniqueRandom()
        {
            List<int> candidates = new List<int>();
            List<int> result = new List<int>();
            for (int i = 0; i <= 3; i++)
            {
                candidates.Add(i);
            }
            Random rnd = new Random();
            while (candidates.Count > 0)
            {
                int index = rnd.Next(candidates.Count);
                result.Add(candidates[index]);
                candidates.RemoveAt(index);
            }
            return result;
        }
    }
}
