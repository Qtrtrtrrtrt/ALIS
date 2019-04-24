using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public enum ExerciseType
    {
        MultipleСhoiceTest,
        TestWithoutChoices,
        MultipleСhoiceTestForTeacher,
        TestWithoutChoicesForTeacher,
        MultipleChoiceWithAnswer
    }

    [Serializable]
    public class Exercise : AbstractExercise
    {
        /// <summary>
        /// Формулировка задачи
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Варианты ответа
        /// </summary>
        public List<string> Answers { get; set; }

     
        /// <summary>
        /// Правильный вариант ответа
        /// </summary>
        public string RightAnswer { get; set; }

        /// <summary>
        /// Номер правильного ответа
        /// </summary>
        public int IndexOfRightAnswer
        {
            get { return Answers.IndexOf(RightAnswer)+1; }
        }

        public Exercise()
        {
            Type = "";
            Text = "";
            Answers = new List<string>();
            RightAnswer = "";
           
        }

        public Exercise(string type, string text, List<string> answers, string rightAnswer ):base(type)
        {
          
            Text = text;
            Answers = answers;
           
            RightAnswer = rightAnswer;
            
            
        }

        public string Print(ExerciseType type)
        {
            var str = Text;
            var abcd = "ABCD";
            switch (type)
            {
                case ExerciseType.MultipleСhoiceTest:
                    var rnd = new Random();
                    Answers = Answers.OrderBy(x => x.Length * rnd.NextDouble()).ToList();
                    var k = 0;
                    foreach (string answer in Answers)
                        str = str + "\t" + (abcd[k++] + ") " + answer + "\n");
                    break;
                case ExerciseType.MultipleСhoiceTestForTeacher:
                    str += "Правильный ответ: " + abcd[IndexOfRightAnswer-1] + "\n";
                    break;
                case ExerciseType.TestWithoutChoicesForTeacher:
                    str += "Правильный ответ: " + RightAnswer + "\n";
                    break;
                case ExerciseType.MultipleChoiceWithAnswer:
                    var rnd2 = new Random();
                    Answers = Answers.OrderBy(x => x.Length * rnd2.NextDouble()).ToList();
                    var k2 = 0;
                    foreach (string answer in Answers)
                        str = str + "\t" + abcd[k2++] + ") " + answer + "\n";
                    str += "Правильный ответ: " + abcd[IndexOfRightAnswer-1] + "\n";
                    break;

            }
            return str;
        }


    }

}
