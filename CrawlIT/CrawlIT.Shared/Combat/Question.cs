using System.Collections.Generic;

namespace CrawlIT.Shared
{
    public class QuestionList
    {
        public List<Question> Questions { get; set; }
    }
    
    // TODO: there's probably better ways of defining some of the variables below
    // i.e. QuestionSubject => Subject
    // would also be nice if we don't have to use numbers in variables
    public class Question
    {
        //public enum Subject { Maths, Programming, Algorithms, Security, Networks, Database };
        //public enum Difficulty { easy, normal, hard };

        public string QuestionSubject { get; set; }

        public string QuestionDifficulty { get; set; }

        public string QuestionText { get; set; }

        public string Answer1 { get; set; }
        public string Answer2 { get; set; }
        public string Answer3 { get; set; }
        public string Answer4 { get; set; }

        public Question(string questionSubject, string questionDifficulty, string questionText, string answer1, string answer2, string answer3, string answer4)
        {
            QuestionSubject = questionSubject;
            QuestionDifficulty = questionDifficulty;
            QuestionText = questionText;
            Answer1 = answer1;
            Answer2 = answer2;
            Answer3 = answer3;
            Answer4 = answer4;
        }
    }
}
