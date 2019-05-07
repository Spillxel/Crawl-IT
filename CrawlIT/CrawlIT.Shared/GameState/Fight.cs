using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CrawlIT.Shared.GameState
{
    public class Fight : GameState
    {
        private Enum _state;

        private Texture2D _pauseButton;
        private Texture2D _question;
        private Texture2D _answer;
        private Texture2D _correct;
        private Texture2D _incorrect;

        private Vector2 _pauseButtonPosition;
        private Vector2 _questionPosition;
        private Vector2 _answer1Position;
        private Vector2 _answer2Position;
        private Vector2 _answer3Position;
        private Vector2 _answer4Position;

        private Rectangle _questionRec;
        private Rectangle _answer1Rec;
        private Rectangle _answer2Rec;
        private Rectangle _answer3Rec;
        private Rectangle _answer4Rec;

        private SpriteFont _font;

        private float _scale;

        public Fight(GraphicsDevice graphicsDevice)
        : base(graphicsDevice)
        {
        }

        public override void Initialize()
        {
            _question = new Texture2D(GraphicsDevice, GraphicsDevice.Viewport.Width,
                                      GraphicsDevice.Viewport.Height / 10);

            _answer = new Texture2D(GraphicsDevice, GraphicsDevice.Viewport.Width / 2,
                                    GraphicsDevice.Viewport.Height / 10 * 2);

            _correct = new Texture2D(GraphicsDevice, GraphicsDevice.Viewport.Width / 2,
                                    GraphicsDevice.Viewport.Height / 10 * 2);

            _incorrect = new Texture2D(GraphicsDevice, GraphicsDevice.Viewport.Width / 2,
                                    GraphicsDevice.Viewport.Height / 10 * 2);

            Color[] data1 = new Color[_question.Width * _question.Height];
            Color[] data2 = new Color[_answer.Width * _answer.Height];
            Color[] data3 = new Color[_correct.Width * _correct.Height];
            Color[] data4 = new Color[_incorrect.Width * _incorrect.Height];

            for (int i = 0; i < data1.Length; ++i)
            {
                data1[i] = Color.LightGray;
            }
            _question.SetData(data1);

            for (int i = 0; i < data2.Length; ++i)
            {
                data2[i] = Color.White;
            }
            _answer.SetData(data2);

            for (int i = 0; i < data3.Length; ++i)
            {
                data3[i] = Color.Green;
            }
            _correct.SetData(data3);

            for (int i = 0; i < data4.Length; ++i)
            {
                data4[i] = Color.Red;
            }
            _incorrect.SetData(data4);

            _scale = GraphicsDevice.Viewport.Width / 1200f;
        }

        public override void LoadContent(ContentManager content)
        {
            _pauseButton = content.Load<Texture2D>("Buttons/pause");
            _font = content.Load<SpriteFont>("Fonts/File");
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
            _pauseButtonPosition = new Vector2((GraphicsDevice.Viewport.Width - _pauseButton.Width - 10), 10);
            _questionPosition = new Vector2(0, (GraphicsDevice.Viewport.Height / 10 * 5) - 5);
            _answer1Position = new Vector2(0, GraphicsDevice.Viewport.Height / 10 * 6);
            _answer2Position = new Vector2((GraphicsDevice.Viewport.Width / 2) + 5, GraphicsDevice.Viewport.Height / 10 * 6);
            _answer3Position = new Vector2(0, (GraphicsDevice.Viewport.Height / 10 * 8) + 5);
            _answer4Position = new Vector2((GraphicsDevice.Viewport.Width / 2) + 5, (GraphicsDevice.Viewport.Height / 10 * 8) + 5);

            //Initialization of the size of the question and answers
            Point _questionPoint = new Point(_question.Width, _question.Height);
            Point _answerPoint = new Point(_answer.Width, _answer.Height);

            //Initialization of the rectangles using the initial position and the size of the question and answers
            _questionRec = new Rectangle((int)_questionPosition.X, (int)_questionPosition.Y, _questionPoint.X, _questionPoint.Y);
            _answer1Rec = new Rectangle((int)_answer1Position.X, (int)_answer1Position.Y, _answerPoint.X, _answerPoint.Y);
            _answer2Rec = new Rectangle((int)_answer2Position.X, (int)_answer2Position.Y, _answerPoint.X, _answerPoint.Y);
            _answer3Rec = new Rectangle((int)_answer3Position.X, (int)_answer3Position.Y, _answerPoint.X, _answerPoint.Y);
            _answer4Rec = new Rectangle((int)_answer4Position.X, (int)_answer4Position.Y, _answerPoint.X, _answerPoint.Y);

            String test = "Who is the best programmer in this project?";
            //String test = GraphicsDevice.Viewport.Width.ToString();
            String test1 = "Jason :S";
            String test2 = "Laurence :B";
            String test3 = "Pedro :)";
            String test4 = "Jiada :|";

            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
            spriteBatch.Draw(_pauseButton, _pauseButtonPosition, Color.White);
            spriteBatch.Draw(_question, _questionPosition, Color.White);
            spriteBatch.Draw(_answer, _answer1Position, Color.White);
            spriteBatch.Draw(_answer, _answer2Position, Color.White);
            spriteBatch.Draw(_answer, _answer3Position, Color.White);
            spriteBatch.Draw(_answer, _answer4Position, Color.White);
            DrawString(spriteBatch, _font, test, _questionRec, Color.Black);
            DrawString(spriteBatch, _font, test1, _answer1Rec, Color.Black);
            DrawString(spriteBatch, _font, test2, _answer2Rec, Color.Black);
            DrawString(spriteBatch, _font, test3, _answer3Rec, Color.Black);
            DrawString(spriteBatch, _font, test4, _answer4Rec, Color.Black);
            spriteBatch.End();
        }

        public override Point GetPosition(Texture2D button)
        {
            if (button.Equals(_pauseButton))
            {
                return new Point((int)_pauseButtonPosition.X,
                                 (int)_pauseButtonPosition.Y);
            }
            else if (button.Equals(_answer))
            {
                return new Point((int)_answer3Position.X,
                                 (int)_answer3Position.Y);
            }
            else
            {
                //return new Point(0, 0);
                return new Point((int)_answer3Position.X,
                                 (int)_answer3Position.Y);
            }
        }

        public override void ChangeTexture(SpriteBatch spriteBatch)
        {
            String test1 = "Jason :S";
            String test2 = "Laurence :B";
            String test3 = "Pedro :)";
            String test4 = "Jiada :|";

            spriteBatch.Begin();
            //spriteBatch.Draw(_incorrect, _answer1Position, Color.White);
            //spriteBatch.Draw(_incorrect, _answer2Position, Color.White);
            //spriteBatch.Draw(_correct, _answer3Position, Color.White);
            //spriteBatch.Draw(_incorrect, _answer4Position, Color.White);
            DrawString(spriteBatch, _font, test1, _answer1Rec, Color.Red);
            DrawString(spriteBatch, _font, test2, _answer2Rec, Color.Red);
            DrawString(spriteBatch, _font, test3, _answer3Rec, Color.Green);
            DrawString(spriteBatch, _font, test4, _answer4Rec, Color.Red);
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
